using UnityEngine;

public class RabbitEatListener : MonoBehaviour
{
[Header("进食模型（eating model）")]
public GameObject eatModel;



[Header("当前是否在进食状态")]  
public bool isEating = false;  
private Vector3 originalPosition;  
private Quaternion originalRotation;  
void Start()  
{  
    if (eatModel != null)  
        eatModel.SetActive(false);  
}  
public void RecordOriginalTransform()  
{  
    originalPosition = transform.position;  
    originalRotation = transform.rotation;  
}  
// 开始进食，传入食盆的 Transform  
public void StartEat(Transform bowlTransform)  
{  
    if (isEating) return;  
    RecordOriginalTransform();  
    gameObject.SetActive(false);  
    if (eatModel != null)  
    {  
        // 让进食模型出现在食盆旁边（你可以自己调偏移）  
        Vector3 offset = new Vector3(0.025f, 0f, 0.07f);  // <-- 你自己调这个值  
        eatModel.transform.position = bowlTransform.position + offset;  
        eatModel.transform.rotation = bowlTransform.rotation;  
        eatModel.SetActive(true);  
    }  
    isEating = true;  
    Debug.Log("[Eat] 兔子开始进食");  
}  
public void StopEat()  
{  
    if (!isEating) return;  
    if (eatModel != null)  
        eatModel.SetActive(false);  
    transform.position = originalPosition;  
    transform.rotation = originalRotation;  
    gameObject.SetActive(true);  
    isEating = false;  
    Debug.Log("[Eat] 兔子结束进食，回到原来位置");  
}  
}