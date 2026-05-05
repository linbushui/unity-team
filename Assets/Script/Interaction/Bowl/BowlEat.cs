using UnityEngine;

public class BowlEat : MonoBehaviour
{
private RabbitEatListener eatListener;
public GameObject idleModel;  



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
        // 假设进食动画持续 2.5 秒后自动结束  
        Invoke(nameof(StopEating), 2.5f);  
    }  
}  
void StopEating()  
{  
    if (eatListener != null)  
        eatListener.StopEat();  
}  
}