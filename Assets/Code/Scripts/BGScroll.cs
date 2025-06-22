using UnityEngine;

public class BGScroll : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float scrollSpeed = 0.1f; 
    private MeshRenderer meshRenderer;
    private float xSxroll;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
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
    }

}
