using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] asteroidSprites;
    public float size = 1f;
    public float maxSize = 2f;
    public float minSize = 0.5f;
    private float speed = 200f;
    public float maxLifetime = 5f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigitbody;
    public GameObject explosionPrefab;
    public float playerInvincibleTime = 5f;
    public float spawnTime;
    public float asteroidInvincibleTime = 0.1f;

    public AudioClip getHitSound;
    private AudioSource audioSource;
    private static int life = 3;
    public HeartUIController heartUI;

    public int maxHealth = 3;
    private int currentHealth;

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
        transform.localScale = Vector3.one * size;
        _rigitbody.mass = size;
        audioSource = GetComponent<AudioSource>();

        if (size >= 1f)
        {
            currentHealth = maxHealth;
            //Debug.Log($"Khởi tạo asteroid lớn (size: {size}), máu = {currentHealth}");
        }
        else
        {
            currentHealth = 1;
            //Debug.Log($"Khởi tạo asteroid nhỏ (size: {size}), máu = {currentHealth}");
        }
    }

    public void SetTrajectory(Vector2 direction)
    {
        if (ScoreManager.Instance.score >= 80)
        {
            speed = 300f;
        }
        else if (ScoreManager.Instance.score >= 40)
        {
            speed = 250f;
        }

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
                Debug.Log("Asteroid vừa spawn, chưa nhận sát thương.");
                return;
            }

            Debug.Log("Asteroid trúng đạn laser, bị phá hủy ngay.");
            ScoreManager.Instance?.AddScore(size >= 1f ? 2 : 1);
            Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController == null) return;

            if (playerController.IsInvincible())
            {
                Debug.Log("Player đang bất tử, không mất máu.");
                Destroy(this.gameObject);
                return;
            }

            Debug.Log("Player bị thiên thạch đâm!");
            PlaySoundAtPosition(getHitSound, transform.position, 5f);
            playerController.ActivateShield();

            int damage = size >= 1f ? 10 : 5;

            Debug.Log($"Thiên thạch kích thước {size}, trừ {damage} máu.");
            playerController.TakeDamage(damage);
            Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void TakeLaserDamage()
    {
        currentHealth--;
        Debug.Log($"Laser bắn asteroid (size: {size}). Máu còn: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Asteroid bị phá hủy do hết máu.");
            if (size / 2f >= minSize)
            {
                if (ScoreManager.Instance.score >= 20 && ScoreManager.Instance.score < 40)
                {
                    Debug.Log("Asteroid tách làm 2 (giai đoạn 20–39 điểm).");
                    CreateSplit();
                }
                else if (ScoreManager.Instance.score >= 40)
                {
                    Debug.Log("Asteroid tách làm 4 (giai đoạn >= 40 điểm).");
                    CreateSplit();
                    CreateSplit();
                }
            }

            ScoreManager.Instance?.AddScore(1);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void CreateSplit()
    {
        float newSize = size / 2f;
        if (newSize < minSize) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < 2; i++)
        {
            Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
            newAsteroid.size = newSize;
            newAsteroid.maxLifetime = maxLifetime;
            newAsteroid.minSize = this.minSize;
            newAsteroid.explosionPrefab = this.explosionPrefab;
            newAsteroid.asteroidSprites = this.asteroidSprites;
            newAsteroid.asteroidInvincibleTime = 0.1f;

            SpriteRenderer sr = newAsteroid.GetComponent<SpriteRenderer>();
            sr.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
            newAsteroid.transform.localScale = Vector3.one * newSize;

            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector2 finalDirection = (directionToPlayer + randomOffset).normalized;
            newAsteroid.SetTrajectory(finalDirection);
        }

        Debug.Log("Asteroid mới đã được sinh ra sau khi chia tách.");
    }

    private void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.spatialBlend = 0f;
        aSource.Play();

        Destroy(tempGO, clip.length);
    }
}
