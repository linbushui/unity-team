using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    [Header("放置的物体")]
    public GameObject objectToPlace;
    [Header("是否允许放置")]
    public bool canPlace = true;
    [Header("放置时播放的动画名")]
    public string animationName = "Take 001";

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private ARPointCloudManager pointCloudManager;

    // 拖拽控制
    private bool isDragging = false;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        pointCloudManager = GetComponent<ARPointCloudManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        // ============ 第一次点击：放置物体 ============
        if (touch.phase == TouchPhase.Began && canPlace)
        {
            TryPlaceObject(touch.position);
        }
        // ============ 拖拽逻辑 ============
        else if (touch.phase == TouchPhase.Moved && !canPlace && isDragging)
        {
            UpdateDragging(touch.position);
        }
        // ============ 松手：结束拖动 ============
        else if (touch.phase == TouchPhase.Ended && isDragging)
        {
            isDragging = false;
            Debug.Log("拖拽结束");
        }
    }

    private void TryPlaceObject(Vector2 touchPos)
    {
        var hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (objectToPlace != null)
            {
                // 把物体放在点击位置
                objectToPlace.transform.position = hitPose.position;

                // 播放动画
                PlaySpawnAnimation(objectToPlace);

                // 禁用物理（避免弹开）
                var rb = objectToPlace.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // 平面视觉可隐藏但仍检测
                HidePlaneAndPointCloud();

                canPlace = false;
                isDragging = true; // 放下后自动可拖拽一次
                Debug.Log("初次放置完成，可拖拽调整位置");
            }
        }
    }

    private void UpdateDragging(Vector2 touchPos)
    {
        if (objectToPlace == null) return;

        var hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            objectToPlace.transform.position = hitPose.position;
        }
    }

    private void PlaySpawnAnimation(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(animationName);
            Debug.Log($"播放 Animator 动画：{animationName}");
            return;
        }

        Animation animation = obj.GetComponent<Animation>();
        if (animation != null)
        {
            animation.Play(animationName);
            Debug.Log($"播放旧版 Animation 动画：{animationName}");
        }
        else
        {
            Debug.LogWarning("目标没有 Animator 或 Animation 组件");
        }
    }

    private void HidePlaneAndPointCloud()
    {
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(false);
            planeManager.enabled = true;
        }

        if (pointCloudManager != null)
        {
            foreach (var pointCloud in pointCloudManager.trackables)
                pointCloud.gameObject.SetActive(false);
            pointCloudManager.enabled = true;
        }

        Debug.Log("平面和点云已隐藏但仍在检测");
    }
}
