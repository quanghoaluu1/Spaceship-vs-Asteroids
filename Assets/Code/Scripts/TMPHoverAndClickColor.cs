using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TMPHoverAndClickColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tmpText;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;
    public Color clickColor = Color.yellow;
    public float transitionDuration = 0.2f;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }

        // Set màu mặc định lúc bắt đầu
        if (tmpText != null)
            tmpText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tmpText != null)
            tmpText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tmpText != null)
            tmpText.color = normalColor; // ✅ trả về màu ban đầu
    }

    void OnClick()
    {
        if (tmpText != null)
            tmpText.color = clickColor;
    }
}
