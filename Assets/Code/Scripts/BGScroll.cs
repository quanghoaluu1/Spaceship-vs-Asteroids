using UnityEngine;
using UnityEngine.UIElements;

public class BGScroll : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float scrollSpeed = 0.1f; 
    private MeshRenderer meshRenderer;
    private float xSxroll;
    public Material[] backgroundMaterials;
    private int currentIndex = 0;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetBackground(0);
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }


    void Scroll()
    {
        xSxroll = Time.time * scrollSpeed;
        Vector2 offset = new Vector2(xSxroll, 0f);
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        UpdateBackgroundByScore(ScoreManager.Instance.score);
    }

    void UpdateBackgroundByScore(int score)
    {
        int newIndex = 0;
        if (score >= 40) newIndex = 2;
        else if (score >= 20) newIndex = 1;

        if (newIndex != currentIndex)
        {
            SetBackground(newIndex);
        }
    }

    void SetBackground(int index)
    {
        if (index >= 0 && index < backgroundMaterials.Length)
        {
            meshRenderer.material = backgroundMaterials[index];
            currentIndex = index;
        }
    }

}
