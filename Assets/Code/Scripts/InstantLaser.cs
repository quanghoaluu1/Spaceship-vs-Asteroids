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
        curve.AddKey(0f, 0.05f);
        curve.AddKey(0.5f, 0.15f);
        curve.AddKey(1f, 0.05f);
        lineRenderer.widthCurve = curve;

        lineRenderer.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FireLaser();
        }
        else if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireLaser();
        }

        if (isLaserActive)
        {
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0f)
            {
                isLaserActive = false;
                lineRenderer.enabled = false;
            }
        }
    }

    private void FireLaser()
    {
        Debug.Log("Laser bắn ra");
        isLaserActive = true;
        laserTimer = laserDuration;
        lineRenderer.enabled = true;

        if (laserSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(laserSound);
        }

        UpdateLaser(); // chỉ gọi 1 lần duy nhất khi bắn
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
            Debug.Log("Laser va chạm với: " + hit.collider.name);
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.CompareTag("Asteroid"))
            {
                Asteroid asteroid = hit.collider.GetComponent<Asteroid>();
                if (asteroid != null && Time.time - asteroid.spawnTime >= asteroid.asteroidInvincibleTime)
                {
                    asteroid.TakeLaserDamage();
                }
                else
                {
                    Debug.Log("Asteroid vừa spawn, chưa nhận sát thương từ laser.");
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPos + direction * maxDistance);
        }
    }
}
