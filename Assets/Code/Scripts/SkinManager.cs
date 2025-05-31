using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;
    public Sprite[] spaceshipSkins;
    public int currentSkinIndex = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // giữ lại khi chuyển scene
    }

    public Sprite GetCurrentSkin()
    {
        if (currentSkinIndex < 0 || currentSkinIndex >= spaceshipSkins.Length)
        {
            currentSkinIndex = 0; // Gán về skin đầu tiên
        }

        return spaceshipSkins[currentSkinIndex];
    }

    public void NextSkin()
    {
        currentSkinIndex = (currentSkinIndex + 1) % spaceshipSkins.Length;
    }

    public void PreviousSkin()
    {
        currentSkinIndex = (currentSkinIndex - 1 + spaceshipSkins.Length) % spaceshipSkins.Length;
    }
}
