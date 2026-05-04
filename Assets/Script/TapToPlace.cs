using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    [Header("入场兔子模型 Luna_Hat")]
    public GameObject entryRabbitPrefab;

    [Header("是否允许放置")]
    public bool canPlace = true;

    [Header("Animator中的Trigger名称")]
    public string triggerName = "appear";

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private ARPointCloudManager pointCloudManager;

    private GameObject currentRabbit;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        pointCloudManager = GetComponent<ARPointCloudManager>();
    }

    void Update()
    {
        if (!canPlace) return;
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            TryPlaceRabbit(touch.position);
        }
    }

    private void TryPlaceRabbit(Vector2 touchPos)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (!raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
            return;

        Pose hitPose = hits[0].pose;

        if (entryRabbitPrefab == null)
        {
            Debug.LogError("Entry Rabbit Prefab 没有绑定");
            return;
        }

        currentRabbit = Instantiate(entryRabbitPrefab, hitPose.position, Quaternion.identity);

        FaceCameraYOnly(currentRabbit.transform);

        Rigidbody rb = currentRabbit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Animator animator = currentRabbit.GetComponent<Animator>();
        if (animator != null)
        {
            animator.ResetTrigger(triggerName);
            animator.SetTrigger(triggerName);
            Debug.Log($"触发兔子入场 Trigger：{triggerName}");
        }
        else
        {
            Debug.LogWarning("生成的入场兔子没有 Animator 组件");
        }

        HidePlaneAndPointCloud();
       RabbitBedListener rabbitListener = FindObjectOfType<RabbitBedListener>();
if (rabbitListener != null && rabbitListener.spawnPoint == Vector3.zero)
{
    rabbitListener.spawnPoint = currentRabbit.transform.position;
    Debug.Log($"TapToPlace 设置出生点：{currentRabbit.transform.position}");
}

        canPlace = false;
    }

    private void FaceCameraYOnly(Transform target)
    {
        if (Camera.main == null) return;

        Vector3 direction = Camera.main.transform.position - target.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            target.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void HidePlaneAndPointCloud()
    {
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(false);
        }

        if (pointCloudManager != null)
        {
            foreach (var pointCloud in pointCloudManager.trackables)
                pointCloud.gameObject.SetActive(false);
        }

        Debug.Log("平面和点云已隐藏");
    }
}
