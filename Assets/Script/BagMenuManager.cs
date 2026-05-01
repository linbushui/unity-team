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

    // 各种模型
public GameObject bedPrefab;
public GameObject bowlPrefab;
public GameObject bowPrefab;
public GameObject foodPrefab;
    
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

    public void OnStartPlacement(string itemType)
{
    // 1. 确定预制体
    GameObject prefabToSpawn = null;
    switch (itemType)
    {
        case "Bed":   prefabToSpawn = bedPrefab; break;
        case "Bowl":  prefabToSpawn = bowlPrefab; break;
        case "Bow":   prefabToSpawn = bowPrefab; break;
        case "Food":  prefabToSpawn = foodPrefab; break;
        default:
            Debug.LogWarning($"未知物品类型：{itemType}");
            return;
    }

    if (prefabToSpawn == null)
    {
        Debug.LogError($"预制体未赋值：{itemType}");
        return;
    }

    // 2. 生成物品（贴在平面高度）
    Vector3 spawnPos = Camera.main.transform.position
                     + Camera.main.transform.forward * 1.8f;
    spawnPos.y = 0;

    GameObject newItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

    // 3. ✅ 确保物品一定可以被 grab
    //    前提：预制体上必须已有 XR Grab Interactable + Collider

    // 4. ✅ 开启平面视觉（桌面的网格）
    ShowARPlanes(true);

    // 5. 关闭背包界面
    CloseBagMenu();

    Debug.Log($"✅ 生成可拖拽物品：{itemType}，平面视觉已开启");
}

  
}