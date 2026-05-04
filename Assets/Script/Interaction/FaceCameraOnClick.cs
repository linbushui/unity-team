using UnityEngine;

public class FaceCameraOnClick : MonoBehaviour
{
    public GameObject facePrefab;
    public float faceDuration = 16f;
    public float distanceFromCamera = 0.8f;
    public float yOffset = -0.2f;

    private GameObject currentFaceInstance;

    void OnMouseDown()
    {
        // 避免重复生成
        if (currentFaceInstance != null) return;

        // 隐藏原模型
        gameObject.SetActive(false);

        // 生成贴脸模型
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 targetPos = camPos + camForward * distanceFromCamera;
        targetPos.y += yOffset;

        currentFaceInstance = Instantiate(facePrefab, targetPos, Quaternion.identity);
        currentFaceInstance.transform.LookAt(Camera.main.transform);
        currentFaceInstance.transform.Rotate(30, 0, 0);

        // 定时恢复
        Invoke(nameof(EndFaceSequence), faceDuration);
    }

    void EndFaceSequence()
    {
        if (currentFaceInstance != null)
            Destroy(currentFaceInstance);

        currentFaceInstance = null;

        // 如果原兔子没有被其他逻辑激活，则重新激活
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
}