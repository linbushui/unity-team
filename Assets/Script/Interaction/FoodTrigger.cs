using UnityEngine;

public class FoodTrigger : MonoBehaviour
{
    [Header("兔子引用")]
    public Animator rabbitAnimator;

    void Start()
    {
        if (rabbitAnimator == null)
            rabbitAnimator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            // 胡萝卜消失
            Destroy(other.gameObject);

            // 播放吃动画
            if (rabbitAnimator != null)
            {
                rabbitAnimator.SetTrigger("Eat");
                Debug.Log("兔子吃到食物啦！");
            }
        }
    }
}