using UnityEngine;

public class BowlEat : MonoBehaviour
{
        [Header("待机模型（eating model）")]
    public GameObject idleModel; 
    private RabbitEatListener eatListener;

    void Start()
    {
        eatListener = FindObjectOfType<RabbitEatListener>();
        if (eatListener == null)
            Debug.LogError("场景中没有找到 RabbitEatListener，请确保它挂在 idle 兔子身上");
        
        if (idleModel != null)
        eatListener = idleModel.GetComponent<RabbitEatListener>();
    }

    void OnMouseDown()
    {
        // 装修模式不触发进食
        if (DecorateModeManager.Instance != null && DecorateModeManager.Instance.isDecorateMode)
        {
            Debug.Log("[Eat] 装修模式中，不触发进食");
            return;
        }

        if (eatListener == null) return;

        if (!eatListener.isEating)
        {
            eatListener.StartEat(transform);
            // 进食动画持续3秒后自动结束
            Invoke(nameof(StopEating), 3f);
        }
    }

    void StopEating()
    {
        if (eatListener != null)
            eatListener.StopEat();
    }
}