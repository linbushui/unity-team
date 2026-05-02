using UnityEngine;
using UnityEngine.UI;
using TMPro;  

public class DecorateModeManager : MonoBehaviour
{
    public static DecorateModeManager Instance;

    [Header("装修模式按钮")]
    public Button decorateButton;       // 开启装修
    public Button decorationPressed;    // 关闭装修

    [Header("装修模式下显示功能按钮禁用版")]
    public GameObject title1;
    public GameObject title2;

    [Header("文本消息UI")]
    public TextMeshProUGUI modeStatusText;      // 显示 "Decoration Mode"
    public TextMeshProUGUI currentEditingText;   // 显示 "Now editing: xxx"
    public Button confirmPlacementButton;        // 确认摆放按钮
    [HideInInspector]
    public GameObject currentEditingItem = null;

    [HideInInspector]
    public bool isDecorateMode = false;

    private Button title1Button;
    private Button title2Button;

    void Awake()
    {
        Instance = this;

        // 获取 title1 和 title2 上的 Button 组件
        if (title1 != null)
            title1Button = title1.GetComponent<Button>();
        if (title2 != null)
            title2Button = title2.GetComponent<Button>();

        // 绑定按钮事件
        if (decorateButton != null)
            decorateButton.onClick.AddListener(() => SetDecorateMode(true));

        if (decorationPressed != null)
            decorationPressed.onClick.AddListener(() => SetDecorateMode(false));

        // 初始状态：未装修
        SetDecorateMode(false);
    }
    


    /// <summary>
    /// 开关装修模式（true=开启，false=关闭）
    /// </summary>
    public void SetDecorateMode(bool enable)
    {
        isDecorateMode = enable;

        // 两态按钮显示切换
        if (decorateButton != null)
            decorateButton.gameObject.SetActive(!enable);

        if (decorationPressed != null)
            decorationPressed.gameObject.SetActive(enable);

        //装修模式提示文本显示
        if (modeStatusText != null)
            modeStatusText.gameObject.SetActive(enable);
        if (currentEditingText != null)
            currentEditingText.gameObject.SetActive(enable);
            currentEditingText.text = "Now editing: none"; 
        if (confirmPlacementButton != null) 
            confirmPlacementButton.gameObject.SetActive(enable);


        // 装修模式时禁用 title1 和 title2 的点击功能
        if (title1Button != null)
            title1Button.interactable = !enable;
        if (title2Button != null)
            title2Button.interactable = !enable;

        Debug.Log(isDecorateMode ? "装修模式 ✅开启（title1/title2 按钮被禁用）" : "装修模式 ❌关闭（title1/title2 按钮恢复可用）");
    }

 

    //提示文本输入
        void UpdateEditingUI()
    {
        if (!isDecorateMode) return;

        if (currentEditingItem != null)
        {
            if (currentEditingText != null)
                currentEditingText.text = $"Now editing: {currentEditingItem.name}";
            if (confirmPlacementButton != null)
                confirmPlacementButton.gameObject.SetActive(true);
        }
        else
        {
            if (currentEditingText != null)
                currentEditingText.text = "Now editing: none";
            if (confirmPlacementButton != null)
                confirmPlacementButton.gameObject.SetActive(false);
        }
    }

public void SetCurrentEditingItem(GameObject item)
{
    if (!isDecorateMode) return;
    currentEditingItem = item;

    string showName = item.name;

    // 如果物品有自定义名字，就用它的
    ItemDisplayName display = item.GetComponent<ItemDisplayName>();
    if (display != null && !string.IsNullOrEmpty(display.displayName))
        showName = display.displayName;

    if (currentEditingText != null)
        currentEditingText.text = $"Now editing: {showName}";
}

    void ConfirmCurrentItemPlacement()
    {
        if (currentEditingItem != null)
        {
            Debug.Log($"物品 {currentEditingItem.name} 位置已确认");
            // ✅ 这里未来可以保存位置
        }
        currentEditingItem = null;
        UpdateEditingUI();
    }
}