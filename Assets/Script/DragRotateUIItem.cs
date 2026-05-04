using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DragSpawnUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("拖拽生成的 3D 预制体")]
    public GameObject prefab3D;

    private GameObject currentObject;
    private Camera arCamera;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // 旋转控制相关
    [SerializeField] private float rotationSpeed = 0.3f;
    private float lastFingerAngle = 0f;
    private bool rotating = false;

    void Start()
    {
        arCamera = Camera.main;
        raycastManager = FindFirstObjectByType<ARRaycastManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab3D == null) return;

        Vector3 startPos = arCamera.transform.position + arCamera.transform.forward * 0.6f;
        currentObject = GameObject.Instantiate(prefab3D, startPos, Quaternion.identity);
        currentObject.transform.LookAt(Camera.main.transform);

        // 禁用重力弹跳
        var rb = currentObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentObject == null || raycastManager == null) return;

        // 支持双指旋转
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float currentAngle = Mathf.Atan2(
                touch1.position.y - touch0.position.y,
                touch1.position.x - touch0.position.x
            ) * Mathf.Rad2Deg;

            if (!rotating)
            {
                lastFingerAngle = currentAngle;
                rotating = true;
            }
            else
            {
                float delta = currentAngle - lastFingerAngle;
                currentObject.transform.Rotate(Vector3.up, -delta * rotationSpeed, Space.World);
                lastFingerAngle = currentAngle;
            }
            return;
        }
        else
        {
            rotating = false;
        }

        // 单指拖拽放置
        if (raycastManager.Raycast(eventData.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            currentObject.transform.position = hitPose.position;
        }
        else
        {
            currentObject.transform.position = arCamera.transform.position + arCamera.transform.forward * 0.6f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentObject = null;
        rotating = false;
    }
}