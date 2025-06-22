using UnityEngine;

public class Star : MonoBehaviour
{
    public Sprite[] starSprites;
    public float size = 1f;
    public float speed = 10f;
    public float maxLifetime = 5f;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigitbody;
    public AudioClip getStarSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigitbody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
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
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(3);
            }

            if (_audioSource != null && getStarSound != null)
            {
                PlaySoundAtPosition(getStarSound, transform.position, 1.5f);
            }

            Destroy(gameObject);
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
