using UnityEngine;

public class FoodTrigger : MonoBehaviour
{
    private Animator rabbitAnimator;

    void Start()
    {
        rabbitAnimator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);

            if (rabbitAnimator != null)
            {
                rabbitAnimator.SetBool("IsEating", true);
                Invoke(nameof(StopEating), 1.2f); // 动画长度自己调
            }
        }
    }

    void StopEating()
    {
        if (rabbitAnimator != null)
            rabbitAnimator.SetBool("IsEating", false);
    }
}