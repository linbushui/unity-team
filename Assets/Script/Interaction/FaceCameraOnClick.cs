using UnityEngine;
using System.Collections;

public class FaceCameraOnClick : MonoBehaviour
{
    [Header("Face Model Prefab")]
    public GameObject facePrefab;

    [Header("Face Duration (seconds)")]
    public float faceDuration = 16f;

    [Header("Face Animation Duration (seconds)")]
    public float faceAnimationDuration = 7f;

    [Header("Distance from Camera")]
    public float distanceFromCamera = 0.8f;

    [Header("Height Offset")]
    public float yOffset = -0.2f;

    private GameObject currentFaceInstance;
    private Animator faceAnimator;
    private FacePetDetector petDetector;

    void OnMouseDown()
    {
        if (DecorateModeManager.Instance != null && DecorateModeManager.Instance.isDecorateMode)
        {
            Debug.Log("[PET] Decor mode, skip face");
            return;
        }

        if (currentFaceInstance != null)
        {
            Debug.Log("[PET] Face model already exists");
            return;
        }

        Debug.Log("[PET] Rabbit clicked, start face sequence");
        StartFaceSequence();
    }

    void StartFaceSequence()
    {
        Debug.Log("[PET] Hide normal rabbit");
        gameObject.SetActive(false);

        Vector3 camPos = Camera.main.transform.position;
        Vector3 targetPos = camPos + Camera.main.transform.forward * distanceFromCamera;
        targetPos.y += yOffset;

        Debug.Log($"[PET] Spawn face model at {targetPos}");
        currentFaceInstance = Instantiate(facePrefab, targetPos, Quaternion.identity);
        currentFaceInstance.transform.LookAt(Camera.main.transform);
        currentFaceInstance.transform.Rotate(30, 0, 0);

        faceAnimator = currentFaceInstance.GetComponent<Animator>();
        if (faceAnimator == null)
        {
            Debug.LogError("[PET] Face model has no Animator component!");
            EndFaceSequence();
            return;
        }

        // 添加抚摸检测组件到贴脸模型上
        petDetector = currentFaceInstance.AddComponent<FacePetDetector>();
        petDetector.Init(faceAnimator);

        Debug.Log($"[PET] Wait {faceAnimationDuration}s before enabling petting");
        Invoke(nameof(EnablePetting), faceAnimationDuration);
        Invoke(nameof(EndFaceSequence), faceDuration);
    }

    void EnablePetting()
    {
        if (petDetector != null)
            petDetector.EnablePetting(true);
        Debug.Log("[PET] Petting enabled, you can now pet");
    }

    void EndFaceSequence()
    {
        Debug.Log("[PET] Face time end, destroy face model");
        if (currentFaceInstance != null)
            Destroy(currentFaceInstance);

        gameObject.SetActive(true);
        currentFaceInstance = null;
        faceAnimator = null;
        petDetector = null;
        Debug.Log("[PET] Face end, normal rabbit back");
    }
}