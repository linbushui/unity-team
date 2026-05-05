using UnityEngine;

public class RabbitBedListener : MonoBehaviour
{
    [Header("睡觉模型（luna_sleep）")]
    public GameObject sleepModel;

    [Header("窝的视觉模型（拖入 Bed 模型的根物体）")]
    public GameObject bedModel;

    [Header("出生点（由 TapToPlace 设置）")]
    public Vector3 spawnPoint;

    [Header("当前是否在窝里")]
    public bool isInBed = false;

    void Start()
    {
        if (sleepModel != null)
            sleepModel.SetActive(false);
    }

    // 进窝：被窝的 BedWarp 调用
    public void EnterBed(Transform bedTransform)
    {
        if (isInBed) return;

        // 1. 隐藏自己（站立兔子）
        gameObject.SetActive(false);

        // 2. 把睡觉模型放到窝的位置，并匹配朝向
        if (sleepModel != null)
        {
            sleepModel.transform.position = bedTransform.position;
            sleepModel.transform.rotation = bedTransform.rotation;
            sleepModel.SetActive(true);
            Debug.Log($"睡觉模型已生成，位置：{sleepModel.transform.position}");
        }

        // 3. 隐藏窝的视觉模型
        if (bedModel != null)
            bedModel.SetActive(false);

        isInBed = true;
        Debug.Log("兔子进窝");
    }

    // 出窝：被窝的 BedWarp 调用
    public void ExitBed()
    {
        if (!isInBed) return;

        // 1. 隐藏睡觉模型
        if (sleepModel != null)
            sleepModel.SetActive(false);

        // 2. 显示窝的视觉模型
        if (bedModel != null)
            bedModel.SetActive(true);

        // 3. 把站立兔子放回出生点，并显示
        transform.position = spawnPoint;
        gameObject.SetActive(true);

        isInBed = false;
        Debug.Log($"兔子出窝，回到出生点：{spawnPoint}");
    }
}