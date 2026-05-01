using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BagMenuManager : MonoBehaviour
{
    public static BagMenuManager Instance;

    // 背包按钮双形态
    public GameObject bagBTM;
    public GameObject bagBTM1;
    
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

    // 记录菜单是否打开
    private bool isMenuOpen = false;
    
    // 背包物品列表
    public List<ShopItem> backpackItems = new List<ShopItem>();
    
    void Awake()
    {
        Instance = this;
        
        if (bagBTM != null) bagBTM.SetActive(true);
        if (bagBTM1 != null) bagBTM1.SetActive(false);
        if (itemMenu != null) itemMenu.SetActive(false);
    }

    // 背包总开关
    public void BagMenuSwitch()
    {
        if (isMenuOpen) CloseBagMenu();
        else OpenBagMenu();
    }
    
    public void OpenBagMenu()
    { 
        // 确保商店菜单关闭
        if (ShopMenuManager.Instance != null)
            ShopMenuManager.Instance.CloseShopMenu();

        if (itemMenu != null) itemMenu.SetActive(true);
        if (bagBTM != null) bagBTM.SetActive(false);
        if (bagBTM1 != null) bagBTM1.SetActive(true);
        
        isMenuOpen = true;
        ShowitemMenu("Furniture");
        Debug.Log("背包菜单已打开");
    }
    
    public void CloseBagMenu()
    {
        if (itemMenu != null) itemMenu.SetActive(false);
        if (bagBTM != null) bagBTM.SetActive(true);
        if (bagBTM1 != null) bagBTM1.SetActive(false);
        isMenuOpen = false;
        Debug.Log("背包菜单已关闭");
    }
    
    public void ShowitemMenu(string category)
    {
        if (!isMenuOpen) OpenBagMenu();
        if (itemMenu != null) itemMenu.SetActive(true);
        
        if (furnitureContent != null) furnitureContent.SetActive(false);
        if (foodContent != null) foodContent.SetActive(false);
        if (clothesContent != null) clothesContent.SetActive(false);
        
        GameObject active = null;
        
        if (category == "Furniture" && furnitureContent != null)
        {
            title1.SetActive(true);
            title2.SetActive(false);
            title3.SetActive(false);
            furnitureContent.SetActive(true);
            active = furnitureContent;
        }
        else if (category == "food" && foodContent != null)
        {
            title1.SetActive(false);
            title2.SetActive(true);
            title3.SetActive(false);
            foodContent.SetActive(true);
            active = foodContent;
        }
        else if (category == "outfit" && clothesContent != null)
        {
            title1.SetActive(false);
            title2.SetActive(false);
            title3.SetActive(true);
            clothesContent.SetActive(true);
            active = clothesContent;
        }
        
        if (heightFitter != null && active != null)
        {
            heightFitter.targetContent = active.GetComponent<RectTransform>();
        }
    }
    
    // 添加物品到背包
    public void AddItem(ShopItem item)
    {
        backpackItems.Add(item);
        Debug.Log($"{item.itemName} 已添加到背包");
        RefreshBackpackUI();
    }

    public void RefreshBackpackUI()
    {
        Debug.Log("背包 UI 需要刷新");
    }

  
}