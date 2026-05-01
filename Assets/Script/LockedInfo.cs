using UnityEngine;
using TMPro;
using System.Collections;

public class LockedInfo : MonoBehaviour
{
    [Header("UI 组件")]
    public TextMeshProUGUI lockedMessage;  // 显示 "not unlocked" 的文字
    
    [Header("设置")]
    public float displayDuration = 5f;     // 显示时长（秒）
    public float fadeDuration = 0.5f;      // 渐隐时长（秒）
    
    private bool isShowing = false;
    private Coroutine currentCoroutine = null;
    
    void Start()
    {
        if (lockedMessage != null)
        {
            // 初始透明且隐藏
            Color color = lockedMessage.color;
            color.a = 0f;
            lockedMessage.color = color;
            lockedMessage.gameObject.SetActive(false);
        }
    }
    
    // 显示未解锁提示（给按钮调用）
    public void ShowLockedMessage()
    {
        if (lockedMessage == null) return;
        
        // 如果已经在显示，停止当前动画
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        
        // 开始新的动画
        currentCoroutine = StartCoroutine(ShowAndFadeOut());
    }
    
    IEnumerator ShowAndFadeOut()
    {
        // 确保物体激活
        lockedMessage.gameObject.SetActive(true);
        
        // 设置完全不透明
        Color color = lockedMessage.color;
        color.a = 1f;
        lockedMessage.color = color;
        
        // 等待显示时长
        yield return new WaitForSeconds(displayDuration);
        
        // 渐隐动画
        float elapsedTime = 0f;
        float startAlpha = 1f;
        float endAlpha = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;  // 0 到 1
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            
            color = lockedMessage.color;
            color.a = newAlpha;
            lockedMessage.color = color;
            
            yield return null;  // 等待下一帧
        }
        
        // 确保最终完全透明
        color.a = 0f;
        lockedMessage.color = color;
        
        // 完全透明后隐藏物体（可选，不影响显示效果）
        lockedMessage.gameObject.SetActive(false);
        
        currentCoroutine = null;
    }
}