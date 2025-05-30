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
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Game Over: Player bị thiên thạch đâm!");

            GameOver();

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
    }

    public void CreateSplit()
    {
        float newSize = size / 2f;

        for (int i = 0; i < 2; i++)
        {
            Asteroid newAsteroid = Instantiate(this, transform.position, Quaternion.identity);
            newAsteroid.size = newSize;
            newAsteroid.maxLifetime = maxLifetime;

            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            newAsteroid.SetTrajectory(randomDirection * speed);
        }
    }
}
