using UnityEngine;

public class RabbitBedListener : MonoBehaviour
{
    [Header("睡觉模型（luna_sleep）")]
    public GameObject sleepModel;

    [Header("窝模型（Bed Model）")]
    public GameObject bedModel;

    [Header("音效设置")]
    public AudioSource audioSource; // 音频播放组件
    public AudioClip sleepSound;    // 进窝时播放的音效文件

    public Vector3 spawnPoint;
    public bool isInBed = false;

    void Start()
    {
        if (sleepModel != null)
            sleepModel.SetActive(false);
    }

    public void SetSpawnPoint(Vector3 pos)
    {
        spawnPoint = pos;
    }

    // 这个方法给 BedWarp 调用
    public void EnterBed(Transform bedTransform)
    {
        if (isInBed) return;

        // 1. 隐藏自己（idle）
        gameObject.SetActive(false);

        // 2. 显示睡觉模型，位置和角度对齐窝
        sleepModel.transform.position = bedTransform.position;
        sleepModel.transform.rotation = bedTransform.rotation;
        sleepModel.SetActive(true);

        // 3. 隐藏窝的视觉模型
        if (bedModel != null)
            bedModel.SetActive(false);

        // 4. 播放睡觉音效
        if (audioSource != null && sleepSound != null)
        {
            audioSource.PlayOneShot(sleepSound);
            Debug.Log("兔子进窝，播放音效");
        }

        isInBed = true;
        Debug.Log("兔子进窝");
    }

    public void ExitBed()
    {
        if (!isInBed) return;

        // 隐藏睡觉模型
        if (sleepModel != null)
            sleepModel.SetActive(false);

        // 显示窝的视觉模型
        if (bedModel != null)
            bedModel.SetActive(true);

        // 自己回到出生点并显示
        transform.position = spawnPoint;
        gameObject.SetActive(true);

        isInBed = false;
        Debug.Log("兔子出窝");
    }
}