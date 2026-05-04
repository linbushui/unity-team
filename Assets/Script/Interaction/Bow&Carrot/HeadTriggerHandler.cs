using UnityEngine;
using System.Collections;

public class HeadTriggerHandler : MonoBehaviour
{
    [Header("普通兔子模型")]
    public GameObject idleModel;

    [Header("带蝴蝶结的兔子模型（仅蝴蝶结专用）")]
    public GameObject idleModelWithBow;

    [Header("胡萝卜动画时长")]
    public float carrotDuration = 6f;

    [Header("蝴蝶结动画时长")]
    public float bowDuration = 2f;

    private GameObject tempItemModel;
    private Animator tempAnimator;
    private string currentItemTag;

    void Start()
    {
        if (idleModelWithBow != null)
            idleModelWithBow.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carrot"))
        {
            currentItemTag = "Carrot";
            StartCoroutine(HandleCarrot(other.gameObject));
        }
        else if (other.CompareTag("Bow"))
        {
            currentItemTag = "Bow";
            StartCoroutine(HandleBow(other.gameObject));
        }
    }

    IEnumerator HandleCarrot(GameObject carrot)
    {
        // 隐藏胡萝卜
        carrot.SetActive(false);

        // 隐藏普通兔子
        idleModel.SetActive(false);

        // 生成临时模型（吃萝卜动画）
        tempItemModel = Instantiate(carrot, transform.position, Quaternion.identity);
        tempAnimator = tempItemModel.GetComponent<Animator>();

        yield return new WaitForSeconds(carrotDuration);

        // 销毁临时模型
        Destroy(tempItemModel);

        // 恢复普通兔子
        idleModel.SetActive(true);

        Debug.Log("[Carrot] 吃完萝卜，恢复待机");
    }

    IEnumerator HandleBow(GameObject bow)
    {
        // 隐藏蝴蝶结
        bow.SetActive(false);

        // 隐藏普通兔子
        idleModel.SetActive(false);

        // 生成临时模型（戴蝴蝶结动画）
        tempItemModel = Instantiate(bow, transform.position, Quaternion.identity);
        tempAnimator = tempItemModel.GetComponent<Animator>();

        yield return new WaitForSeconds(bowDuration);

        // 销毁临时模型
        Destroy(tempItemModel);

        // 显示带蝴蝶结的固定模型
        if (idleModelWithBow != null)
            idleModelWithBow.SetActive(true);

        Debug.Log("[Bow] 蝴蝶结佩戴完成，切换为带蝴蝶结模型");
    }
}