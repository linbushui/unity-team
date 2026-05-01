using UnityEngine;

public class StarterManager : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPage = 0;
    public GameObject gameMainCanvas;
    public GameObject rabbitModel;        // 兔子模型
    public string animationName = "Take 001";  // 要播放的动画名称
    
    void Start()
    {
        // 确保游戏主界面是隐藏的
        if (gameMainCanvas != null)
        {
            gameMainCanvas.SetActive(false);
        }
        
        //开始时兔子不出现
        if (rabbitModel != null)
        {
            rabbitModel.SetActive(false);
        }

        // 只显示第一页
        ShowPage(0);
    }
    
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }
    
    public void StartGame()
    {
        // 隐藏所有导览页
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
        
        // 显示游戏主界面
        if (gameMainCanvas != null)
        {
            gameMainCanvas.SetActive(true);
        }
        
        // 兔子出现
if (rabbitModel != null)
{
    rabbitModel.SetActive(true);
    
    // 获取模型 Animator
    Animator animator = rabbitModel.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play(animationName);  // 播放动画
                Debug.Log($"播放动画：{animationName}");
            }
    else
    {
        Debug.LogWarning("兔子模型没有 Animation 组件");
    }
}
    }
    
    void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
    }
}