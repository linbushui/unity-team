using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopMenuManager : MonoBehaviour
{
    public static ShopMenuManager Instance;

    // 商店按钮双形态
    public GameObject shopBTM;
    public GameObject shopBTM1;
    
    // 菜单背景和分支菜单
    public GameObject itemMenu;
    public GameObject furnitureContent;
    public GameObject foodContent;
    public GameObject clothesContent;
  
    // 三个深色功能按钮
    public GameObject title1;
    public GameObject title2;
    public GameObject title3;
    
    // 菜单滚动高度
    public FitHeightToContent heightFitter; 

    private bool isMenuOpen = false;
    
    // ========== 只需要这三个容器（你手动摆好格子的地方）==========
    public Transform furnitureShopContainer;  // 家具物品的父容器
    public Transform foodShopContainer;       // 食物物品的父容器
    public Transform clothesShopContainer;    // 服装物品的父容器
    
    [Header("提示信息")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;
    
    void Awake()
    {
        Instance = this;
        
        if (shopBTM != null) shopBTM.SetActive(true);
        if (shopBTM1 != null) shopBTM1.SetActive(false);
        if (itemMenu != null) itemMenu.SetActive(false);
    }
    
    void Start()
    {
        // 初始化所有格子的购买状态
        InitShopItems(furnitureShopContainer);
        InitShopItems(foodShopContainer);
        InitShopItems(clothesShopContainer);
    }
    
    // 读取手动摆放的格子，记录购买状态
   void InitShopItems(Transform shopContainer)
{
    if (shopContainer == null) return;
    
    for (int i = 0; i < shopContainer.childCount; i++)
    {
        Transform item = shopContainer.GetChild(i);
        
        ShopItemData itemData = item.GetComponent<ShopItemData>();
        if (itemData == null)
        {
            itemData = item.gameObject.AddComponent<ShopItemData>();
        }
        itemData.isOwned = false;
        
        // 先找到 prize in shop（Image）
        Transform priceImage = item.Find("prize in shop");
        if (priceImage == null)
        {
            Debug.LogError($"找不到 'prize in shop'，物品：{item.name}");
            continue;
        }
        
        // 再找到它里面的 Text 子物体
        TextMeshProUGUI priceTextComponent = priceImage.GetComponentInChildren<TextMeshProUGUI>();
        if (priceTextComponent == null)
        {
            Debug.LogError($"在 'prize in shop' 里找不到 Text 组件，物品：{item.name}");
            continue;
        }
        
        // 获取价格
        string priceStr = priceTextComponent.text.Trim();
        int price;
        if (int.TryParse(priceStr, out price))
        {
            itemData.originalPrice = price;
            Debug.Log($"价格解析成功：{price}，物品：{item.name}");
        }
        else
        {
            Debug.LogError($"价格解析失败！文字是：'{priceStr}'，物品：{item.name}");
        }
        
        // 绑定购买事件
        Button btn = item.GetComponent<Button>();
        if (btn != null)
        {
            GameObject itemObj = item.gameObject;
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => BuyItem(itemObj));
        }
    }

}
    
    // 购买物品
   public void BuyItem(GameObject itemObject)
{
    ShopItemData itemData = itemObject.GetComponent<ShopItemData>();
    if (itemData == null || itemData.isOwned)
    {
        ShowMessage("你已经拥有这个物品了！");
        return;
    }
    // ========== 添加调试日志 ==========
    Debug.Log($"准备购买，价格：{itemData.originalPrice}");
    Debug.Log($"CoinManager.Instance 是否为 null：{CoinManager.Instance == null}");
    // ========== 调试日志结束 ==========
    
    // ========== 检查金币是否足够 ==========
    if (CoinManager.Instance == null)
    {
        Debug.LogError("CoinManager 不存在！");
        return;
    }
    
    if (CoinManager.Instance.SpendCoin(itemData.originalPrice))
    {
        // 购买成功
        itemData.isOwned = true;
        
        // 变灰
        Button btn = itemObject.GetComponent<Button>();
        if (btn != null) btn.interactable = false;
        
        // 修改文字
        Transform priceImage = itemObject.transform.Find("prize in shop");
        if (priceImage != null)
        {
            TextMeshProUGUI priceTextComponent = priceImage.GetComponentInChildren<TextMeshProUGUI>();
            if (priceTextComponent != null) priceTextComponent.text = "已拥有";
            }
        
        // 移动到末尾
        itemObject.transform.SetAsLastSibling();
        
        // 添加到背包
        AddToBackpack(itemObject);
        
        ShowMessage($"成功购买 {itemObject.name}！");
    }
    else
    {
        // 购买失败
        ShowMessage($"宠物币不够！需要 {itemData.originalPrice}");
    }
}

void AddToBackpack(GameObject itemObject)
{
    if (BagMenuManager.Instance == null) return;
    
    ShopItemData itemData = itemObject.GetComponent<ShopItemData>();
    if (itemData == null)
    {
        Debug.LogWarning("找不到 ShopItemData 组件");
        return;
    }
    
    Sprite iconSprite = null;
    Image iconImage = itemObject.transform.Find("Image")?.GetComponent<Image>();
    if (iconImage != null)
    {
        iconSprite = iconImage.sprite;
    }
    
    string itemName = itemObject.name;
    int price = itemData.originalPrice;
    ShopItem shopItem = new ShopItem(itemName, price, iconSprite);
    shopItem.isOwned = true;
    
    BagMenuManager.Instance.AddItem(shopItem);
}
    
    void ShowMessage(string message)
    {
        if (messageText != null) messageText.text = message;
        if (messagePanel != null) messagePanel.SetActive(true);
        CancelInvoke("HideMessage");
        Invoke("HideMessage", messageDuration);
    }
    
    void HideMessage()
    {
        if (messagePanel != null) messagePanel.SetActive(false);
    }
    
    // ========== 原有的菜单控制方法 ==========
    
    public void ShopMenuSwitch()
    {
        if (isMenuOpen) CloseShopMenu();
        else OpenShopMenu();
    }
    
    public void OpenShopMenu()
    {
        if (BagMenuManager.Instance != null) BagMenuManager.Instance.CloseBagMenu();
        if (itemMenu != null) itemMenu.SetActive(true);
        if (shopBTM != null) shopBTM.SetActive(false);
        if (shopBTM1 != null) shopBTM1.SetActive(true);
        
        isMenuOpen = true;
        ShowitemMenu("Furniture");
    }
    
    public void CloseShopMenu()
    {
        if (itemMenu != null) itemMenu.SetActive(false);
        if (shopBTM != null) shopBTM.SetActive(true);
        if (shopBTM1 != null) shopBTM1.SetActive(false);
        isMenuOpen = false;
    }
    
    public void ShowitemMenu(string category)
    {
        if (!isMenuOpen) OpenShopMenu();
        if (itemMenu != null) itemMenu.SetActive(true);
        
        if (furnitureContent != null) furnitureContent.SetActive(false);
        if (foodContent != null) foodContent.SetActive(false);
        if (clothesContent != null) clothesContent.SetActive(false);
        
        if (category == "Furniture" && furnitureContent != null)
        {
            title1.SetActive(true);
            title2.SetActive(false);
            title3.SetActive(false);
            furnitureContent.SetActive(true);
        }
        else if (category == "food" && foodContent != null)
        {
            title1.SetActive(false);
            title2.SetActive(true);
            title3.SetActive(false);
            foodContent.SetActive(true);
        }
        else if (category == "outfit" && clothesContent != null)
        {
            title1.SetActive(false);
            title2.SetActive(false);
            title3.SetActive(true);
            clothesContent.SetActive(true);
        }
        
        if (heightFitter != null)
        {
            GameObject active = null;
            if (category == "Furniture") active = furnitureContent;
            else if (category == "food") active = foodContent;
            else if (category == "outfit") active = clothesContent;
            
            if (active != null)
            {
                heightFitter.targetContent = active.GetComponent<RectTransform>();
            }
        }
    }
}

// 用于记录每个物品的购买状态
public class ShopItemData : MonoBehaviour
{
    public bool isOwned = false;
    public int originalPrice = 0;
}