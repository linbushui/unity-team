using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;      // 物品名称（如"小鱼干"）
    public int price;            // 价格
    public Sprite icon;          // 图标
    public bool isOwned = false; // 是否已购买
    
    // 构造函数，方便创建新物品
    public ShopItem(string name, int price, Sprite icon)
    {
        this.itemName = name;
        this.price = price;
        this.icon = icon;
        this.isOwned = false;
    }
}