using UnityEngine;
using UnityEngine.SceneManagement;

public class Asteroid : MonoBehaviour
{
    public Sprite[] asteroidSprites;
    public float size = 1f;
    public float maxSize = 2f;
    public float minSize = 0.5f;
    public float speed = 200f;
    public float maxLifetime = 5f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigitbody;
    public GameObject explosionPrefab;
    public float playerInvincibleTime = 5f; // thời gian miễn nhiễm của player (5 giây)

    public float spawnTime;            
    public float asteroidInvincibleTime = 0.5f; // thời gian miễn nhiễm của thiên thạch (500ms)

    public static int life = 2;     // Số mạng của player

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigitbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        spawnTime = Time.time;
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

        if (collision.gameObject.tag == "LaserShot")
        {
            if (Time.time - spawnTime < asteroidInvincibleTime)
            {
                return; // miễn nhiễm tạm thời khi mới spawn
            }

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
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsInvincible())
            {
                Debug.Log("Player đang bất tử, không mất máu trong " + playerController.IsInvincibleTime());

                Destroy(this.gameObject);
                return;
            }

            // Gây sát thương cho player
            if (life <= 0)
            {
                GameOver();
            }
            else
            {
                life--;
                playerController.TakeDamage();  // Ghi nhận thời gian bị đâm
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


        SceneManager.LoadSceneAsync(3);
    }

    public void CreateSplit()
    {
        float newSize = size / 2f;

        if (newSize < minSize)
        {
            return; // Không chia nhỏ nữa nếu kích thước nhỏ hơn minSize
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return; // Không tìm thấy player, không chia nhỏ
        }

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < 2; i++)
        {
            Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
            newAsteroid.size = newSize;
            newAsteroid.maxLifetime = maxLifetime;

            // Sao chép các giá trị quan trọng
            newAsteroid.minSize = this.minSize;
            newAsteroid.explosionPrefab = this.explosionPrefab;
            newAsteroid.asteroidSprites = this.asteroidSprites;
            newAsteroid.asteroidInvincibleTime = 0.5f; // Cho miễn nhiễm khi spawn

            // Đặt lại sprite
            SpriteRenderer sr = newAsteroid.GetComponent<SpriteRenderer>();
            sr.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
            newAsteroid.transform.localScale = Vector3.one * newSize;

            // Thêm một chút random để không bay trùng nhau hoàn toàn
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector2 finalDirection = (directionToPlayer + randomOffset).normalized;

            newAsteroid.SetTrajectory(finalDirection);
        }
    }
}
