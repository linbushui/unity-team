using UnityEngine;

public class ModelSwitcher : MonoBehaviour
{
    public GameObject enterModel;   // 带帽子 / 入场模型
    public GameObject idleModel;    // 普通待机模型

    void Start()
    {
        // 一开始展示入场模型
        enterModel.SetActive(true);
        idleModel.SetActive(false);
    }

    // 动画事件会调用这方法
    public void SwitchToIdleModel()
    {
        enterModel.SetActive(false);
        idleModel.SetActive(true);

        // 顺便切动画状态
        GetComponent<Animator>().Play("Idle");
    }
}