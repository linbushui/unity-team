using UnityEngine;
using TMPro;

public class ScreenDebug : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private string log = "";

    // 只显示包含这些关键词的日志
    private string[] keywords = { "PET" };

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 检查是否包含关键词
        bool show = false;
        foreach (string kw in keywords)
        {
            if (logString.Contains(kw))
            {
                show = true;
                break;
            }
        }

        if (show)
        {
            log = logString;
            if (debugText != null)
                debugText.text = log;
        }
    }
}