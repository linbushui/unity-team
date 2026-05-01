using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BagMenuManager : MonoBehaviour
{
    public static BagMenuManager Instance;

    [Header("UI 组件")]
    public GameObject bagBTM;
    public GameObject bagBTM1;
    public GameObject itemMenu;
    public GameObject furnitureContent;
    public GameObject foodContent;
    public GameObject clothesContent;
    public GameObject title1, title2, title3;
    public FitHeightToContent heightFitter; 

    private bool isMenuOpen = false;

    [Header("背包数据")]
    public List<ShopItem> backpackItems = new List<ShopItem>();

    [Header("家具预制体")]
    public GameObject bedPrefab;
    public GameObject bowlPrefab;
    public GameObject bowPrefab;
    public GameObject foodPrefab;

    [Header("AR 系统组件")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public ARPointCloudManager pointCloudManager;

    // 当前正在放置的物品
    private GameObject currentPlacingItem;
    private bool isPlacing = false;

    void Awake()
    {
        Instance = this;
        if (bagBTM != null) bagBTM.SetActive(true);
        if (bagBTM1 != null) bagBTM1.SetActive(false);
        if (itemMenu != null) itemMenu.SetActive(false);
    }

    void Update()
    {
        // 拖拽逻辑
        if (!isPlacing || currentPlacingItem == null || raycastManager == null) return;
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            var hits = new List<ARRaycastHit>();
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                currentPlacingItem.transform.position = hitPose.position;
            }
        }

        if (touch.phase == TouchPhase.Ended)
        {
            ConfirmPlacement();
        }
    }

    // =======================================================
    // 背包 UI 控制
    // =======================================================
    public void BagMenuSwitch()
    {
        if (isMenuOpen) CloseBagMenu();
        else OpenBagMenu();
    }

    public void OpenBagMenu()
    { 
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
            title1.SetActive(true); title2.SetActive(false); title3.SetActive(false);
            furnitureContent.SetActive(true);
            active = furnitureContent;
        }
        else if (category == "food" && foodContent != null)
        {
            title1.SetActive(false); title2.SetActive(true); title3.SetActive(false);
            foodContent.SetActive(true);
            active = foodContent;
        }
        else if (category == "outfit" && clothesContent != null)
        {
            title1.SetActive(false); title2.SetActive(false); title3.SetActive(true);
            clothesContent.SetActive(true);
            active = clothesContent;
        }
        
        if (heightFitter != null && active != null)
        {
            heightFitter.targetContent = active.GetComponent<RectTransform>();
        }
    }

    // =======================================================
    // 物品管理逻辑
    // =======================================================
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

    // =======================================================
    // 核心：生成物品 + 拖拽放置
    // =======================================================
    public void OnStartPlacement(string itemType)
    {
        GameObject prefabToSpawn = null;
        switch (itemType)
        {
            case "Bed": prefabToSpawn = bedPrefab; break;
            case "Bowl": prefabToSpawn = bowlPrefab; break;
            case "Bow": prefabToSpawn = bowPrefab; break;
            case "Food": prefabToSpawn = foodPrefab; break;
            default: return;
        }

        if (prefabToSpawn == null) return;

        // 生成物品在相机前方
        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 0.8f;
        currentPlacingItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        // 禁用物理影响
        var rb = currentPlacingItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // 显示平面辅助
        ShowARPlanes(true);

        // 进入拖拽模式
        isPlacing = true;

        CloseBagMenu();
        Debug.Log($"生成家具：{itemType}");
    }

    private void ConfirmPlacement()
    {
        ShowARPlanes(false);
        currentPlacingItem = null;
        isPlacing = false;
        Debug.Log("家具放置完成，退出拖拽模式");
    }

    // =======================================================
    // 控制平面显示
    // =======================================================
    private void ShowARPlanes(bool show)
    {
        if (planeManager != null)
        {
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(show);
        }

        if (pointCloudManager != null)
        {
            foreach (var point in pointCloudManager.trackables)
                point.gameObject.SetActive(show);
        }
    }
}
