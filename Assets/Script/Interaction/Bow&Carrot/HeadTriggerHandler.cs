using UnityEngine;
using System.Collections;

public class HeadTriggerHandler : MonoBehaviour
{
    [Header("兔子模型组")]
    public GameObject idleModel;
    public GameObject carrotModel;
    public GameObject bowModel;
    public GameObject bowIdleModel;

    [Header("头部触发器碰撞体")]
    public Collider headTrigger;   // 用于检测物品

    private void Start()
    {
        carrotModel.SetActive(false);
        bowModel.SetActive(false);
        bowIdleModel.SetActive(false);

        if (headTrigger != null)
            headTrigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[HeadTrigger]is true, item:{other.name}，Tag：{other.tag}");
        if (other.CompareTag("Carrot"))
        {
            StartCoroutine(HandleCarrot(other.gameObject));
        }
        else if (other.CompareTag("Bow"))
        {
            StartCoroutine(HandleBow(other.gameObject));
        }
    }

    private IEnumerator HandleCarrot(GameObject carrot)
    {
        carrot.SetActive(false);
        idleModel.SetActive(false);
        carrotModel.SetActive(true);

        yield return new WaitForSeconds(6f);

        carrotModel.SetActive(false);
        idleModel.SetActive(true);
    }

    private IEnumerator HandleBow(GameObject bow)
    {
        bow.SetActive(false);
        idleModel.SetActive(false);
        bowModel.SetActive(true);

        yield return new WaitForSeconds(2f);

        bowModel.SetActive(false);
        bowIdleModel.SetActive(true);
    }

    public void OnCarrotEnter(GameObject carrot)
{
    Debug.Log("[Head] active carrot on hand");
    StartCoroutine(HandleCarrot(carrot));
}

public void OnBowEnter(GameObject bow)
{
    Debug.Log("[Head] active bow on hand");
    StartCoroutine(HandleBow(bow));
}
}