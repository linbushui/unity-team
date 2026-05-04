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

    // 旋转相关
    private bool isRotating = false;
    private float lastFingerAngle = 0f;
    [SerializeField] private float rotationSpeed = 0.3f;

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (!DecorateModeManager.Instance.isDecorateMode) return;

        if (Input.touchCount == 2)
        {
            // 双指旋转逻辑
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float currentAngle = Mathf.Atan2(
                touch1.position.y - touch0.position.y,
                touch1.position.x - touch0.position.x
            ) * Mathf.Rad2Deg;

            if (!isRotating)
            {
                lastFingerAngle = currentAngle;
                isRotating = true;
                isSelected = false; // 旋转时取消单指拖动
            }
            else
            {
                float delta = currentAngle - lastFingerAngle;
                transform.Rotate(Vector3.up, -delta * rotationSpeed, Space.World);
                lastFingerAngle = currentAngle;
            }
            return;
        }
        else
        {
            isRotating = false;
        }

        // 单指拖动逻辑（只在没有双指旋转时执行）
        if (Input.touchCount == 1 && !isRotating)
        {
            Touch touch = Input.GetTouch(0);

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
        else if (Input.touchCount != 2)
        {
            isSelected = false;
        }
    }
}