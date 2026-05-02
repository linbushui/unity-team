using UnityEngine;

public class AppMultiAudioController : MonoBehaviour
{
    [Header("音频源组件")]
    public AudioSource bgmSource;
    public AudioSource narrationSource;

    [Header("音频文件配置")]
    public AudioClip bgmClip;
    public AudioClip narration1;
    public AudioClip narration2;
    public AudioClip narration3;

    private void Start()
    {
        // 初始化将 BGM 赋值给对应的 Audio Source
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
        }
    }

    /// <summary>
    /// 第一页：开始播放 BGM 和第一段旁白
    /// </summary>
    public void StartBGMAndNarration1()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }

        if (narrationSource != null && narration1 != null)
        {
            narrationSource.clip = narration1;
            narrationSource.Play();
        }
    }

    /// <summary>
    /// 第二页：结束第一段旁白，播放第二段旁白
    /// </summary>
    public void PlayNarration2()
    {
        if (narrationSource != null)
        {
            narrationSource.Stop();
            if (narration2 != null)
            {
                narrationSource.clip = narration2;
                narrationSource.Play();
            }
        }
    }

    /// <summary>
    /// 第三页：结束第二段旁白，播放第三段旁白
    /// </summary>
    public void PlayNarration3()
    {
        if (narrationSource != null)
        {
            narrationSource.Stop();
            if (narration3 != null)
            {
                narrationSource.clip = narration3;
                narrationSource.Play();
            }
        }
    }

    /// <summary>
    /// 第四页：结束 BGM 并结束第三段旁白
    /// </summary>
    public void StopAllAudio()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        if (narrationSource != null && narrationSource.isPlaying)
        {
            narrationSource.Stop();
        }
    }
}