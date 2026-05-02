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
    [Header("Animator中的Trigger名称")]
    public string triggerName = "Appear"; 

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private ARPointCloudManager pointCloudManager;

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

        if (touch.phase == TouchPhase.Began && canPlace)
        {
            TryPlaceObject(touch.position);
        }
        else if (touch.phase == TouchPhase.Moved && !canPlace && isDragging)
        {
            UpdateDragging(touch.position);
        }
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
                // 放到点击位置并朝向相机
                objectToPlace.transform.position = hitPose.position;
                objectToPlace.transform.LookAt(Camera.main.transform);

                // 触发Animator中的Appear Trigger
                Animator animator = objectToPlace.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger(triggerName);
                    Debug.Log($"触发动画 Trigger：{triggerName}");
                }
                else
                {
                    Debug.LogWarning("目标没有 Animator 组件");
                }

                // 禁用物理
                var rb = objectToPlace.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }

                // 隐藏平面
                HidePlaneAndPointCloud();
                canPlace = false;
                isDragging = true;
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
    }
}
