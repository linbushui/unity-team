using UnityEngine;

public class RabbitBedListener : MonoBehaviour
{
    [Header("睡觉模型（luna_sleep）")]
    public GameObject sleepModel;

    [Header("窝模型（Bed Model）")]
    public GameObject bedModel;

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

    void OnMouseDown()
    {
        // 如果点击的是窝，不在这个脚本处理，由窝自己的 BedWarp 处理
        // 但这个脚本是为了让 idle 响应窝的点击，所以我们会通过另一个方式触发
    }

    // 这个方法给 BedWarp 调用
    public void EnterBed(Transform bedTransform)
{
    if (isInBed) return;

    // 隐藏自己（idle）
    gameObject.SetActive(false);

    // 显示睡觉模型，位置和角度对齐窝
    sleepModel.transform.position = bedTransform.position;
    sleepModel.transform.rotation = bedTransform.rotation;
    sleepModel.SetActive(true);

    // 隐藏窝的视觉模型
    if (bedModel != null)
        bedModel.SetActive(false);

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