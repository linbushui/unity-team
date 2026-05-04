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

    // 记录原始位置（可在 Start 或 进窝前调用）
    public void RecordOriginalTransform()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    // 开始进食（传进食位置，通常是食盆的位置）
    public void StartEat(Transform bowlTransform)
    {
        if (isEating) return;

        // 记录原本位置（吃之前）
        RecordOriginalTransform();

        // 隐藏普通模型
        gameObject.SetActive(false);

        // 把进食模型放到食盆位置，并激活
        if (eatModel != null)
        {
            Vector3 offset = new Vector3(0.025f, 0f, 0.07f);
            eatModel.transform.position = bowlTransform.position + offset;
            eatModel.transform.rotation = bowlTransform.rotation;
            eatModel.SetActive(true);
        }

        isEating = true;
        Debug.Log("[Eat] 兔子开始进食");
    }

    // 结束进食，回到原来位置
    public void StopEat()
    {
        if (!isEating) return;

        // 隐藏进食模型
        if (eatModel != null)
            eatModel.SetActive(false);

        // 恢复普通模型位置，并显示
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        gameObject.SetActive(true);

        isEating = false;
        Debug.Log("[Eat] 兔子结束进食，回到原来位置");
    }
}