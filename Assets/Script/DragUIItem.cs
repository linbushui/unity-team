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

    void Start()
    {
        // 找到 AR 相机
        arCamera = Camera.main;
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab3D == null) return;

        // 生成物体：初始在相机前方
        Vector3 startPos = arCamera.transform.position + arCamera.transform.forward * 0.6f;
        currentObject = GameObject.Instantiate(prefab3D, startPos, Quaternion.identity);

        // 禁止物理弹跳
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

        if (raycastManager.Raycast(eventData.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            currentObject.transform.position = hitPose.position;
        }
        else
        {
            // 没检测到平面，就让模型停在相机前方（可视）
            currentObject.transform.position = arCamera.transform.position + arCamera.transform.forward * 0.6f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentObject == null) return;
        Debug.Log("放置完成：" + currentObject.name);
        currentObject = null; // 清空引用
    }
}
