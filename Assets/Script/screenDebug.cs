using UnityEngine;
using TMPro;

public class ScreenDebug : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private string log = "";

    // 只显示包含这些关键词的日志
    private string[] keywords = { "Test", "Head", "Drag"};

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
            // ✅ 追加日志，而不是替换
            log += "\n" + logString;

            // 可选：限制最多显示20行，避免文字太多
            string[] lines = log.Split('\n');
            if (lines.Length > 20)
            {
                log = "";
                for (int i = lines.Length - 20; i < lines.Length; i++)
                    log += lines[i] + "\n";
            }

            if (debugText != null)
                debugText.text = log;
        }
    }
}