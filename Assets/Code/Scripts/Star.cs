using UnityEngine;

public class Star : MonoBehaviour
{
    public Sprite[] starSprites;
    public float size = 1f;
    public float maxSize = 1.5f;
    public float minSize = 0.5f;
    public float speed = 10f;
    public float maxLifetime = 5f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigitbody;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigitbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _spriteRenderer.sprite = starSprites[Random.Range(0, starSprites.Length)];

        _spriteRenderer.transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        this.transform.localScale = Vector3.one * size;

        _rigitbody.mass = size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigitbody.linearDamping = 0f;
        _rigitbody.linearVelocity = direction.normalized * 2f;
        Destroy(gameObject, maxLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
