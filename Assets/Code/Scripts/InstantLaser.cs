using UnityEngine;
using UnityEngine.InputSystem;

public class InstantLaser : MonoBehaviour
{
    public Transform firePoint;
    public float maxDistance = 100f;
    public LineRenderer lineRenderer;
    public LayerMask hitLayers;
    public float laserDuration = 0.1f;

    private bool isLaserActive = false;
    private float laserTimer = 0f;

    public AudioClip laserSound;          
    private AudioSource audioSource;

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        Shader shader = Shader.Find("Legacy Shaders/Particles/Additive");
        if (shader == null)
        {
            Debug.LogError("Không tìm thấy shader Legacy Shaders/Particles/Additive");
            return;
        }
        lineRenderer.material = new Material(shader);

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.cyan, 0f),
                new GradientColorKey(Color.white, 0.5f),
                new GradientColorKey(Color.magenta, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );
        lineRenderer.colorGradient = gradient;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0.05f);  // nhỏ đầu
        curve.AddKey(0.5f, 0.15f); // dày giữa
        curve.AddKey(1f, 0.05f);   // nhỏ cuối
        lineRenderer.widthCurve = curve;

        lineRenderer.enabled = false;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            isLaserActive = true;
            laserTimer = laserDuration;
            lineRenderer.enabled = true;

            if (laserSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(laserSound);
            }
        }

        if (isLaserActive)
        {
            UpdateLaser();
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0f)
            {
                isLaserActive = false;
                lineRenderer.enabled = false;
            }
        }
    }

    private void UpdateLaser()
    {
        Vector2 startPos = firePoint.position;
        Vector2 direction = firePoint.up;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDistance, hitLayers);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Asteroid"))
            {
                Asteroid asteroid = hit.collider.GetComponent<Asteroid>();
                if (asteroid != null)
                {
                    // Kiểm tra xem có đang miễn nhiễm không
                    if (Time.time - asteroid.spawnTime >= asteroid.asteroidInvincibleTime)
                    {
                        // Gây sát thương hoặc phá hủy
                        if (asteroid.size / 2f >= asteroid.minSize)
                        {
                            if (ScoreManager.Instance.score >= 20 && ScoreManager.Instance.score < 40)
                            {
                                asteroid.CreateSplit();
                            }
                            else if (ScoreManager.Instance.score >= 40)
                            {
                                asteroid.CreateSplit();
                                asteroid.CreateSplit();
                            }
                        }

                        if (ScoreManager.Instance != null)
                        {
                            ScoreManager.Instance.AddScore(1);
                        }
                        else
                        {
                            Debug.LogWarning("ScoreManager.Instance == null");
                        }

                        Instantiate(asteroid.explosionPrefab, asteroid.transform.position, Quaternion.identity);
                        Destroy(asteroid.gameObject);
                    }
                }
            }

            if (hit.collider.CompareTag("EnemySpaceship"))
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    //enemy.TakeDamage(1);
                    Destroy(enemy.gameObject);
                }
            }
            if (hit.collider.CompareTag("Boss"))
            {
                BossController boss = hit.collider.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.Die();
                    Destroy(boss.gameObject);
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPos + direction * maxDistance);
        }
    }
}
