using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    
    public int currentCoin = 100;
    public TextMeshProUGUI coinText;  // 这个先留着，可以在 Inspector 拖拽
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        // 延迟一帧再更新，确保所有东西都加载好了
        Invoke("UpdateCoinUI", 0.1f);
    }
    
    public bool SpendCoin(int amount)
    {
        if (currentCoin >= amount)
        {
            currentCoin -= amount;
            UpdateCoinUI();
            Debug.Log($"花费 {amount} 金币，剩余 {currentCoin}");
            return true;
        }
        else
        {
            Debug.Log($"金币不足！需要 {amount}，当前 {currentCoin}");
            return false;
        }
    }
    
    public void AddCoin(int amount)
    {
        currentCoin += amount;
        UpdateCoinUI();
        Debug.Log($"获得 {amount} 金币，当前 {currentCoin}");
    }
    
    void UpdateCoinUI()
    {
        // 如果 coinText 是空的，尝试从场景中查找
        if (coinText == null)
        {
            // 尝试通过名字查找
            GameObject found = GameObject.Find("CoinText");
            if (found != null)
            {
                coinText = found.GetComponent<TextMeshProUGUI>();
                if (coinText != null)
                {
                    Debug.Log("自动找到了金币文字组件");
                }
            }
            
            // 如果还是找不到，报错但不崩溃
            if (coinText == null)
            {
                Debug.LogWarning("找不到金币文字，请手动拖拽赋值");
                return;
            }
        }
        
        coinText.text = currentCoin.ToString();
        Debug.Log($"金币UI更新为：{currentCoin}");
    }
}