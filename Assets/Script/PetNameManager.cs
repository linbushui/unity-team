using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetNameManager : MonoBehaviour
{
    public static PetNameManager Instance;
    
    // 输入框（用户打字的地方）
    public TMP_InputField nameInputField;
    
    // 确认按钮
    public Button confirmButton;
    
    // ========== 新增：游戏主界面显示名字的文字 ==========
    public TextMeshProUGUI gameUIText;  // 直接把游戏界面的文字拖进来
    
    public string defaultName = "Luna";
    public int maxNameLength = 10;
    
    private string currentPetName;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        if (nameInputField != null)
        {
            nameInputField.characterLimit = maxNameLength;
        }
        
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnConfirmName);
        }
        
        // 加载已保存的名字
        LoadPetName();
    }
    
    public void OnConfirmName()
    {
        if (nameInputField != null && !string.IsNullOrEmpty(nameInputField.text))
        {
            currentPetName = nameInputField.text;
        }
        else
        {
            currentPetName = defaultName;
        }
        
        currentPetName = currentPetName.Trim();
        
        if (string.IsNullOrEmpty(currentPetName))
        {
            currentPetName = defaultName;
        }
        
        // 保存
        PlayerPrefs.SetString("PetName", currentPetName);
        PlayerPrefs.Save();
        
        // ========== 关键：直接更新游戏界面的文字 ==========
        if (gameUIText != null)
        {
            gameUIText.text = currentPetName;
        }
        
        Debug.Log($"宠物名字已保存并显示：{currentPetName}");

    }
    
    public void LoadPetName()
    {
        currentPetName = PlayerPrefs.GetString("PetName", defaultName);
        
        // 如果输入框还在，也更新输入框显示
        if (nameInputField != null)
        {
            nameInputField.text = currentPetName;
        }
        
        // 如果游戏界面文字还在，也更新它
        if (gameUIText != null)
        {
            gameUIText.text = currentPetName;
        }
    }
    
    public string GetPetName()
    {
        return PlayerPrefs.GetString("PetName", defaultName);
    }
}