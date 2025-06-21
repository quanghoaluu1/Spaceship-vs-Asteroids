using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth = 100;
    public float speed = 10f;
    private Vector2 moveInput;
    public float invincibleDuration = 5f;
    private float lastHitTime = -999f;

    public GameObject shield;
    private bool isInvincible = false;
    private SpriteRenderer SpriteRenderer;
    public GameObject pulse1;
    public GameObject pulse2;
    public HeartUIController heartUI;
    public AudioClip overSound;

    private SpriteRenderer pulse1Renderer;
    private SpriteRenderer pulse2Renderer;
    private PlayerInputActions inputActions;
    private float baseSpeed;
    private Coroutine speedBoostRoutine;
    public GameObject bombEffectPrefab;

    public static PlayerController Instance { get; private set; }

    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject); // Đã có instance hợp lệ, không tạo thêm
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
        if (PlayerPrefs.GetInt("HasSaved", 0) == 1)
        {
            Debug.Log("Saved health: " + PlayerPrefs.GetInt("SavedHealth", 3));
            currentHealth = PlayerPrefs.GetInt("SavedHealth", 3);
        }
        else
        {
            Debug.Log("No saved health data found, initializing with maxHealth.");
            currentHealth = maxHealth; // Nếu không có dữ liệu lưu, khởi tạo với maxHealth
        }
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if (SkinManager.Instance != null)
            SpriteRenderer.sprite = SkinManager.Instance.GetCurrentSkin();

        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        shield.SetActive(false);
        pulse1Renderer = pulse1.GetComponent<SpriteRenderer>();
        pulse2Renderer = pulse2.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
        heartUI.SetMaxHealth(maxHealth);    // Cho thanh máu dạng slider
        heartUI.UpdateHealth(currentHealth);
    }

    void OnEnable() => inputActions.Enable();

    void OnDisable()
    {
        if (inputActions != null)
            inputActions.Disable();
    }

    void Update() => MovePlayer();

    void MovePlayer()
    {
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.position += move * speed * Time.deltaTime;

        Vector3 clampedPosition = Camera.main.WorldToViewportPoint(transform.position);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0.05f, 0.95f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(clampedPosition);
    }

    public void TakeDamage(int amount)
    {
        //if (IsInvincible()) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        heartUI.UpdateHealth(currentHealth);  // Slider

        StartCoroutine(CameraShake.Instance.Shake(0.5f, 0.3f));
        lastHitTime = Time.time;

        if (currentHealth <= 0) GameOver();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        heartUI.UpdateHealth(currentHealth);
    }

    public bool IsInvincible() => Time.time - lastHitTime < invincibleDuration;

    public void ActivateShield()
    {
        if (isInvincible) return;
        lastHitTime = Time.time;
        StartCoroutine(ShieldRoutine());
    }

    private IEnumerator ShieldRoutine()
    {
        isInvincible = true;
        shield.SetActive(true);
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < invincibleDuration)
        {
            visible = !visible;
            SpriteRenderer.enabled = visible;
            pulse1Renderer.enabled = visible;
            pulse2Renderer.enabled = visible;
            elapsed += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }

        pulse1Renderer.enabled = true;
        pulse2Renderer.enabled = true;
        SpriteRenderer.enabled = true;
        shield.SetActive(false);
        isInvincible = false;
    }

    public void GameOver()
    {
        GameObject audioObj = new GameObject("TempAudio");
        AudioSource audio = audioObj.AddComponent<AudioSource>();
        audio.clip = overSound;
        audio.Play();
        DontDestroyOnLoad(audioObj);

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(this.gameObject);

        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync(2);
        Destroy(audioObj, overSound.length);
    }

    // Buff tăng tốc
    public void ApplySpeedBoost(float duration, float multiplier)
    {
        if (speedBoostRoutine != null)
            StopCoroutine(speedBoostRoutine);
        speedBoostRoutine = StartCoroutine(SpeedBoost(duration, multiplier));
    }

    private IEnumerator SpeedBoost(float duration, float multiplier)
    {
        float originalSpeed = speed;
        speed = originalSpeed * multiplier;
        Debug.Log($"⚡ Speed Boost Activated: {speed}");

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        Debug.Log($"⚡ Speed Boost Ended: {speed}");
    }

    //Bom nổ

    public void ApplyEffect()
    {
        TriggerBomb();

        if (bombEffectPrefab != null)
        {
            Vector3 explosionPosition = transform.position;
            explosionPosition.z = 0f;

            GameObject effect = Instantiate(bombEffectPrefab, explosionPosition, Quaternion.identity);
            Destroy(effect, 0.5f); 
        }
    }
    public void TriggerBomb()
    {
        StartCoroutine(ExplodeAllAsteroids());
    }

    private IEnumerator ExplodeAllAsteroids()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject asteroidObj in asteroids)
        {
            if (asteroidObj == null || !asteroidObj.activeInHierarchy) continue;

            Asteroid asteroid = asteroidObj.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.DestroyInstantly();
                yield return new WaitForSeconds(0.05f); 
            }
        }
    }

    // Kích hoạt khiên
    public void ActivateShieldNoBlink()
    {
        if (isInvincible) return;

        isInvincible = true;
        shield.SetActive(true);
        StartCoroutine(DisableShieldAfterDelay(invincibleDuration));
    }

    private IEnumerator DisableShieldAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        shield.SetActive(false);
        isInvincible = false;
    }




}
