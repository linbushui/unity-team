using UnityEngine;
using UnityEngine.UI;

public class IconToggle : MonoBehaviour
{
    [Header("UI Components")]
    public Image iconImage; // 拖入需要更改图标的 Image 组件

    [Header("Icons")]
    public Sprite playSprite; // 播放图标
    public Sprite muteSprite; // 静音图标

    private bool isPlaying = true; // 状态记录：true 表示播放，false 表示静音

    // 公开的方法，供按钮点击时调用
    public void OnClickToggle()
    {
        // 切换状态
        isPlaying = !isPlaying;

        // 根据状态切换 Sprite
        if (iconImage != null)
        {
            iconImage.sprite = isPlaying ? playSprite : muteSprite;
        }
    }
}
