using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    public bool canPlace = true;
    public string animationName = "Take 001";  // 动画名称

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private ARPointCloudManager pointCloudManager;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        pointCloudManager = GetComponent<ARPointCloudManager>();
    }

    void Update()
    {
        if (!canPlace) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    if (objectToPlace != null)
                    {
                        objectToPlace.transform.position = hitPose.position;
                        
                        // ========== 播放动画 ==========
                        // 方法1：用 Animator 组件
                        Animator animator = objectToPlace.GetComponent<Animator>();
                        if (animator != null)
                        {
                            animator.Play(animationName);
                            Debug.Log($"播放动画：{animationName}");
                        }
                        else
                        {
                            // 方法2：用 Animation 组件（旧版）
                            Animation animation = objectToPlace.GetComponent<Animation>();
                            if (animation != null)
                            {
                                animation.Play(animationName);
                                Debug.Log($"用 Animation 组件播放：{animationName}");
                            }
                            else
                            {
                                Debug.LogWarning("兔子模型没有 Animator 或 Animation 组件");
                            }
                        }
                        // ========== 播放动画结束 ==========
                    }

                    // 隐藏所有已生成的平面
                    if (planeManager != null)
                    {
                        foreach (var plane in planeManager.trackables)
                        {
                            plane.gameObject.SetActive(false);
                        }
                        planeManager.enabled = true;
                        planeManager.gameObject.SetActive(false);
                    }

                    // 隐藏所有已生成的点云
                    if (pointCloudManager != null)
                    {
                        foreach (var pointCloud in pointCloudManager.trackables)
                        {
                            pointCloud.gameObject.SetActive(false);
                        }
                        pointCloudManager.enabled = true;
                        pointCloudManager.gameObject.SetActive(false);
                    }

                    Debug.Log("平面和点云已隐藏");
                    canPlace = false;
                }
            }
        }
    }
}