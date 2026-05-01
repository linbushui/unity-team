using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class DragToPlace : MonoBehaviour
{
    [Header("平面显示物体（拖入Hierarchy中的AR物体）")]
    public GameObject arDefaultPlane;
    public GameObject arDefaultPointCloud;

    [Header("物品预制体")]
    public GameObject bedPrefab;
    public GameObject bowlPrefab;
    public GameObject bowPrefab;
    public GameObject foodPrefab;

    private ARRaycastManager raycastManager;
    private GameObject currentItem;
    private bool isDragging = false;

    void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // ⭐ 这个方法就是 BagMenuManager 要调用的
    public void StartPlacement(string itemType)
    {
        Debug.Log($"StartPlacement 被调用，物品类型：{itemType}");

        // 获取预制体
        GameObject prefab = null;
        switch (itemType)
        {
            case "Bed": prefab = bedPrefab; break;
            case "Bowl": prefab = bowlPrefab; break;
            case "Bow": prefab = bowPrefab; break;
            case "Food": prefab = foodPrefab; break;
            default:
                Debug.LogWarning($"未知物品类型：{itemType}");
                return;
        }

        if (prefab == null)
        {
            Debug.LogError($"预制体未赋值：{itemType}");
            return;
        }

        // 清除旧的
        if (currentItem != null) Destroy(currentItem);

        // 创建新物品
        currentItem = Instantiate(prefab);
        SetAlpha(currentItem, 0.5f);

        // 显示平面辅助
        if (arDefaultPlane != null) arDefaultPlane.SetActive(true);
        if (arDefaultPointCloud != null) arDefaultPointCloud.SetActive(true);

        isDragging = true;
        Debug.Log($"开始拖拽放置：{itemType}");
    }

    void Update()
    {
        if (!isDragging) return;
        if (currentItem == null) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 screenPos = touch.position;

            // 手指移动：跟随
            if (touch.phase == TouchPhase.Moved)
            {
                FollowTouch(screenPos);
            }
            // 手指抬起：放置
            else if (touch.phase == TouchPhase.Ended)
            {
                ConfirmPlacement(screenPos);
                isDragging = false;
            }
        }
    }

    void FollowTouch(Vector2 screenPos)
    {
        var hits = new List<ARRaycastHit>();
        if (raycastManager != null && raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            currentItem.transform.position = hits[0].pose.position;
            currentItem.transform.rotation = hits[0].pose.rotation;
        }
        else
        {
            // 没有检测到平面时，沿相机方向放置
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            currentItem.transform.position = ray.GetPoint(2f);
        }
    }

    void ConfirmPlacement(Vector2 screenPos)
    {
        var hits = new List<ARRaycastHit>();
        if (raycastManager != null && raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            currentItem.transform.position = hits[0].pose.position;
            currentItem.transform.rotation = hits[0].pose.rotation;
        }

        // 恢复不透明
        SetAlpha(currentItem, 1f);

        // 隐藏平面辅助
        if (arDefaultPlane != null) arDefaultPlane.SetActive(false);
        if (arDefaultPointCloud != null) arDefaultPointCloud.SetActive(false);

        currentItem = null;
        Debug.Log("物品已放置完成");
    }

    private void SetAlpha(GameObject obj, float alpha)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            // 创建一个临时透明材质（确保支持透明渲染）
            Material tempMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            Color color = tempMat.color;
            color.a = alpha;
            tempMat.color = color;
            renderer.material = tempMat;
        }
    }
}