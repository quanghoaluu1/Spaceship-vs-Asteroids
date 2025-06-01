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

    public AudioClip getHitSound;
    private AudioSource audioSource;
    public static int life = 3;     // Số mạng của player

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
        audioSource = GetComponent<AudioSource>();
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigitbody.linearDamping = 0f;
        _rigitbody.AddForce(direction * speed);
        Destroy(gameObject, maxLifetime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Va chạm xảy ra với: " + collision.gameObject.name);
        if (collision.gameObject.tag == "LaserShot")
        {
            if (Time.time - spawnTime < asteroidInvincibleTime)
            {
                return; // miễn nhiễm tạm thời khi mới spawn
            }

            Debug.Log("Thử cộng điểm");
            ScoreManager.Instance?.AddScore(1); // Dùng ?. để tránh lỗi null

            if (ScoreManager.Instance != null)
            {
                Debug.Log("Cộng điểm!");
                ScoreManager.Instance.AddScore(1);
            }
            else
            {
                Debug.LogWarning("ScoreManager.Instance == null");
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

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController == null) return;


            if (playerController.IsInvincible())
            {
                Debug.Log("Player đang bất tử, không mất máu. Còn " + playerController.IsInvincibleTime() + " giây.");
                Destroy(this.gameObject);
                return;
            }
            PlaySoundAtPosition(getHitSound, transform.position, 5f);

            // ❗ Nếu đến đây là chắc chắn chưa bất tử → xử lý mất máu và kích hoạt khiên
            life--;

            playerController.ActivateShield(); // Bật trạng thái bất tử + khiên + nhấp nháy
            Debug.Log("Player bị thiên thạch đâm! Còn " + life + " máu.");

            if (life <= 0)
            {
                GameOver();
            }

            Destroy(this.gameObject);
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

    private void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.spatialBlend = 0f; // 0 = 2D sound
        aSource.Play();

        Destroy(tempGO, clip.length);
    }
}
