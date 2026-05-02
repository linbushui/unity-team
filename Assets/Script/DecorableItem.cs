using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(Collider))]
public class DecoratableItem : MonoBehaviour
{
    private bool isSelected = false;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (!DecorateModeManager.Instance.isDecorateMode) return; // 非装修模式不响应

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 选中/拖动逻辑
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == transform)
                    {
                        isSelected = true;
                        DecorateModeManager.Instance.SetCurrentEditingItem(gameObject);
                        Debug.Log(gameObject.name + " 被选中进行拖动");
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && isSelected)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    transform.position = hitPose.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isSelected = false;
            }
        }
    }
}
