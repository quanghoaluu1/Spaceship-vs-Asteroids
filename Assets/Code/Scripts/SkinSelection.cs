using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SkinSelection : MonoBehaviour
{
    public Image skinPreview;
    void Start()
    {
        UpdatePreview();
    }

    public void OnNextSkin()
    {
        SkinManager.Instance.NextSkin();
        UpdatePreview();
    }

    public void OnPreviousSkin()
    {
        SkinManager.Instance.PreviousSkin();
        UpdatePreview();
    }

    void UpdatePreview()
    {
        skinPreview.sprite = SkinManager.Instance.GetCurrentSkin();
    }

    public void OnPlayGame()
    {
        SceneManager.LoadScene("GamePlayScene");
    }
}
