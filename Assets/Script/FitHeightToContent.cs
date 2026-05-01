using UnityEngine;
using UnityEngine.UI;

public class FitHeightToContent : MonoBehaviour
{
    public RectTransform targetContent; // 当前激活的 Content（furnitureContent 等）
    
    void Update()
    {
        if (targetContent != null)
        {
            // 把 menuWrapper 的高度设置成 targetContent 的高度
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetContent.rect.height);
        }
    }
}