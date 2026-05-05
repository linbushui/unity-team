using UnityEngine;
using System.Collections;

public class RabbitStateManager : MonoBehaviour
{
    [Header("入场动画模型（顺序播放）")]
    public GameObject blinkModel;
    public GameObject hatoffModel;

    [Header("状态模型（按需切换）")]
    public GameObject idleModel;
    public GameObject carrotModel;
    public GameObject bowModel;
    public GameObject bowIdleModel;

    [Header("入场动画时长（秒）")]
    public float blinkDuration = 2f;
    public float hatoffDuration = 4f;
    public float carrotDuration = 6f;
    public float bowDuration = 2f;

    private Collider touchCollider;   // 用于触摸交互的 Collider

    void Start()
    {
        // 初始状态：只显示 blink
        SetAllModelsOff();
        blinkModel.SetActive(true);

        // 确保触摸碰撞体在工作（放在 idleModel 或 rabbit 根）
        touchCollider = GetComponent<Collider>();
        if (touchCollider == null)
            Debug.LogError("[RabbitState] 没有 Collider，无法触摸！");

        StartCoroutine(PlayEntrySequence());
    }

    IEnumerator PlayEntrySequence()
    {
        yield return new WaitForSeconds(blinkDuration);
        blinkModel.SetActive(false);
        hatoffModel.SetActive(true);

        yield return new WaitForSeconds(hatoffDuration);
        hatoffModel.SetActive(false);
        idleModel.SetActive(true);
    }

    void SetAllModelsOff()
    {
        blinkModel.SetActive(false);
        hatoffModel.SetActive(false);
        idleModel.SetActive(false);
        carrotModel.SetActive(false);
        bowModel.SetActive(false);
        bowIdleModel.SetActive(false);
    }

    // 手动触发胡萝卜（拖拽结束调用）
    public void OnCarrotEnter(GameObject carrot)
    {
        if (carrot == null || carrotModel == null) return;
        StartCoroutine(HandleCarrot(carrot));
    }

    IEnumerator HandleCarrot(GameObject carrot)
    {
        carrot.SetActive(false);
        SetAllModelsOff();
        carrotModel.SetActive(true);

        yield return new WaitForSeconds(carrotDuration);

        SetAllModelsOff();
        idleModel.SetActive(true);
    }

    public void OnBowEnter(GameObject bow)
    {
        if (bow == null || bowModel == null) return;
        StartCoroutine(HandleBow(bow));
    }

    IEnumerator HandleBow(GameObject bow)
    {
        bow.SetActive(false);
        SetAllModelsOff();
        bowModel.SetActive(true);

        yield return new WaitForSeconds(bowDuration);

        SetAllModelsOff();
        bowIdleModel.SetActive(true);
    }
}