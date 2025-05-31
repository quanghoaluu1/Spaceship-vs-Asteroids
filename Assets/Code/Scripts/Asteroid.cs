using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] asteroidSprites;
    public float size = 1f;
    public float maxSize = 1.5f;
    public float minSize = 0.5f;
    public float speed = 200f;
    public float maxLifetime = 5f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigitbody;
    public GameObject explosionPrefab;

    public static int life = 2;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigitbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];

        _spriteRenderer.transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        this.transform.localScale = Vector3.one * size;
        _rigitbody.mass = size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigitbody.linearDamping = 0f;
        _rigitbody.AddForce(direction * speed);
        Destroy(gameObject, maxLifetime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "LaserShot")
        {
            //if (size / 2f >= minSize)
            //{
            //    CreateSplit();
            //    CreateSplit();
            //}

            Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            if (life <= 0)
            {
                GameOver();
            }
            else
            {
                life--;
                Debug.Log("Player bị thiên thạch đâm!");
                Destroy(this.gameObject);
            }
        }
    }

    public void TakeDamageFromLaser()
    {
        if (size / 2f >= minSize)
        {
            CreateSplit();
            CreateSplit();
        }

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(this.gameObject);
    }


    private void GameOver()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(this.gameObject);
        Debug.Log("Ngỏm củ tỏi zồi");
        Time.timeScale = 0f;
    }

    public void CreateSplit()
    {
        float newSize = size / 2f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < 2; i++)
        {
            Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
            newAsteroid.size = newSize;
            newAsteroid.maxLifetime = maxLifetime;

            // Thêm một chút random để không bay trùng nhau hoàn toàn
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector2 finalDirection = (directionToPlayer + randomOffset).normalized;

            newAsteroid.SetTrajectory(finalDirection);
        }
    }
}
