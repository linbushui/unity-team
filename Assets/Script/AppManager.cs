using UnityEngine;
using UnityEngine.UI; // 如果需要切换图标，需要引用这个

public class AppManager : MonoBehaviour
{
    [Header("页面引用")]
    public GameObject page1;
    public GameObject page2;

    [Header("音频引用")]
    public AudioSource bgmSource;

    // --- 1. 点击 Get Started 执行的方法 ---
    public void OnGetStartedClick()
    {
        // 切换页面
        page1.SetActive(false);
        page2.SetActive(true);

        // 播放背景音乐
        if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    // --- 2. 点击声音图标执行的方法 (切换 播放/暂停) ---
    public void ToggleMusic()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause(); // 暂停播放
            Debug.Log("音乐已暂停");
        }
        else
        {
            bgmSource.UnPause(); // 从暂停处恢复播放
            // 如果你之前是用 Stop 的，这里要改用 Play
            Debug.Log("音乐继续播放");
        }
    }
}