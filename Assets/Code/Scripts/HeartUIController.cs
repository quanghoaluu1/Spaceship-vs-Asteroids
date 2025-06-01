using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour
{
    public Image[] hearts; // kéo 3 Image vào đây trong Inspector
    public Sprite heartRed;
    public Sprite heartBlack;

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = heartRed;
            else
                hearts[i].sprite = heartBlack;
        }
    }
}
