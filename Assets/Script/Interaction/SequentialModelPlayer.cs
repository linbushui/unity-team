using UnityEngine;
using System.Collections;

public class SequentialModelPlayer : MonoBehaviour
{
    [Header("三个模型（按顺序）")]
    public GameObject blinkModel;
    public GameObject hatoffModel;
    public GameObject idleModel;

    [Header("每个阶段的时长（秒）")]
    public float blinkDuration = 2f;
    public float hatoffDuration = 4f;

    void Start()
    {
        // 初始状态
        blinkModel.SetActive(true);
        hatoffModel.SetActive(false);
        idleModel.SetActive(false);

        // 开始顺序播放
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // 1️⃣ 第一阶段：blink
        yield return new WaitForSeconds(blinkDuration);
        blinkModel.SetActive(false);

        // 2️⃣ 第二阶段：hatoff
        hatoffModel.SetActive(true);
        yield return new WaitForSeconds(hatoffDuration);
        hatoffModel.SetActive(false);

        // 3️⃣ 第三阶段：idle
        idleModel.SetActive(true);
    }
}