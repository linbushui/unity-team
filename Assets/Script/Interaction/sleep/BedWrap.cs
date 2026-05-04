using UnityEngine;

public class BedWarp : MonoBehaviour
{
    public GameObject bedModel;          // 窝的视觉模型
    private RabbitBedListener rabbitListener;

    void Start()
    {
        rabbitListener = FindObjectOfType<RabbitBedListener>();
        if (rabbitListener == null)
            Debug.LogError("场景中没有找到 RabbitBedListener，请确保它挂在 idle 兔子身上");
    }

    void OnMouseDown()
    {
        // 如果处于装修模式，不触发贴脸动画
        if (DecorateModeManager.Instance != null && DecorateModeManager.Instance.isDecorateMode)
        {
            Debug.Log("装修模式中，不播放贴脸动画");
            return;
        }
        
        if (rabbitListener == null) return;

        // 根据当前是否在窝里，决定进窝还是出窝
        if (!rabbitListener.isInBed)
        {
            rabbitListener.EnterBed(transform);
        }
        else
        {
            rabbitListener.ExitBed();
        }
    }
}