using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PersonalityDetailManager : MonoBehaviour
{
    [Header("Choose 按钮")]
    public Button[] chooseButtons;
    
    [Header("详情面板")]
    public GameObject[] detailPanels;
    
    [Header("父面板")]
    public GameObject detailsParent;
    
    [Header("Locked 按钮")]
    public Button LockedButton;
    
    [Header("动画设置")]
    public float animationDuration = 0.3f;
    
    void Start()
    {
        if (detailsParent != null)
        {
            detailsParent.SetActive(false);
            Debug.Log("Details 大面板已禁用");
        }
        
        for (int i = 0; i < chooseButtons.Length; i++)
        {
            int index = i;
            if (chooseButtons[i] != null)
            {
                chooseButtons[i].onClick.RemoveAllListeners();
                chooseButtons[i].onClick.AddListener(() => ShowDetail(index));
            }
        }
        
        // ✅ 修复：使用 gameObject.SetActive
        if (LockedButton != null)
        {
            LockedButton.gameObject.SetActive(false);
        }
    }
    
    void ShowDetail(int index)
    {
        if (detailsParent != null && !detailsParent.activeSelf)
        {
            detailsParent.SetActive(true);
        }
        
        for (int i = 0; i < detailPanels.Length; i++)
        {
            if (i != index && detailPanels[i] != null)
            {
                detailPanels[i].SetActive(false);
            }
        }
        
        if (detailPanels[index] != null)
        {
            StartCoroutine(OpenPanelAnimation(detailPanels[index]));
        }
        
        
        if (LockedButton != null)
        {
            if (index == 0)
            {
                LockedButton.gameObject.SetActive(false);
                Debug.Log("选择了 Choose1，Lock按钮已禁用");
            }
            else
            {
                LockedButton.gameObject.SetActive(true);
                Debug.Log($"选择了 Choose{index+1}，Lock按钮已激活");
            }
        }
    }
    
    IEnumerator OpenPanelAnimation(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float scale = Mathf.Lerp(0f, 1f, t);
            panel.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        
        panel.transform.localScale = Vector3.one;
    }
    
    IEnumerator ClosePanelAnimation(GameObject panel)
    {
        if (panel == null || !panel.activeSelf) yield break;
        
        float elapsedTime = 0f;
        float startScale = panel.transform.localScale.x;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float scale = Mathf.Lerp(startScale, 0f, t);
            panel.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        
        panel.transform.localScale = Vector3.zero;
        panel.SetActive(false);
        
        bool anyActive = false;
        foreach (GameObject p in detailPanels)
        {
            if (p != null && p.activeSelf)
            {
                anyActive = true;
                break;
            }
        }
        
        if (!anyActive && detailsParent != null)
        {
            detailsParent.SetActive(false);
            Debug.Log("所有详情已关闭，隐藏 Details 大面板");
        }
    }
    
    public void CloseDetail(GameObject panel)
    {
        StartCoroutine(ClosePanelAnimation(panel));
    }
}