using UnityEngine;
using System.Collections;

public class BowlFeeder : MonoBehaviour
{
    [Header("兔子引用")]
    public Transform rabbit;
    public Animator rabbitAnimator;

    [Header("食盆位置（就是自身位置）")]
    public Transform eatPosition;

    [Header("动画时长")]
    public float eatDuration = 1.5f;

    private Vector3 rabbitOriginalPos;
    private Quaternion rabbitOriginalRot;

    void Start()
    {
        if (rabbit == null)
            rabbit = FindObjectOfType<RabbitTag>()?.transform; // 需要下面那个脚本

        if (eatPosition == null)
            eatPosition = transform;
    }

    void OnMouseDown()
    {
        if (rabbit == null) return;

        // 记录原位置
        rabbitOriginalPos = rabbit.position;
        rabbitOriginalRot = rabbit.rotation;

        // 闪现到食盆旁（偏移一点，不要重合）
        Vector3 targetPos = eatPosition.position + new Vector3(0.3f, 0, 0.3f);
        rabbit.position = targetPos;

        // 播放吃动画
        if (rabbitAnimator != null)
            rabbitAnimator.SetTrigger("Eat");

        // 延迟后返回原位
        StartCoroutine(BackToOriginal());
    }

    IEnumerator BackToOriginal()
    {
        yield return new WaitForSeconds(eatDuration);
        rabbit.position = rabbitOriginalPos;
        rabbit.rotation = rabbitOriginalRot;

        if (rabbitAnimator != null)
            rabbitAnimator.SetTrigger("Idle");
    }
}