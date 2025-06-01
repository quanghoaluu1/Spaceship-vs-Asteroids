using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int life = 3;
    public int currentLife = 3;

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

    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if (SkinManager.Instance != null)
        {
            SpriteRenderer.sprite = SkinManager.Instance.GetCurrentSkin();
        }
        else
        {
            Debug.LogWarning("SkinManager is NULL — running gameplay scene directly?");
        }

        inputActions = new PlayerInputActions();


        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        shield.SetActive(false);

        pulse1Renderer = pulse1.GetComponent<SpriteRenderer>();
        pulse2Renderer = pulse2.GetComponent<SpriteRenderer>();
    }
    public void ResetLife()
    {
        currentLife = life;
        // Gọi cập nhật UI nếu cần
        heartUI.UpdateHealth(currentLife);
    }

    public void LoseLife()
    {
        currentLife--;
        heartUI.UpdateHealth(currentLife);

        if (currentLife <= 0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {

        // Tạo 1 GameObject chứa AudioSource
        GameObject audioObj = new GameObject("TempAudio");
        AudioSource audio = audioObj.AddComponent<AudioSource>();
        audio.clip = overSound;
        audio.Play();

        // Không bị destroy khi chuyển scene
        DontDestroyOnLoad(audioObj);

        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(this.gameObject);
        Debug.Log("Ngỏm củ tỏi zồi");

        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync(2);

        // Tự huỷ sau khi âm thanh phát xong
        Destroy(audioObj, overSound.length);
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        MovePlayer();
    }
    public bool IsInvincible()
    {
        return Time.time - lastHitTime < invincibleDuration;
    }

    public float IsInvincibleTime()
    {
        return invincibleDuration - (Time.time - lastHitTime);
    }

    public void TakeDamage()
    {
        lastHitTime = Time.time;
        // TODO: giảm máu, hiệu ứng, animation...
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.position += move * speed * Time.deltaTime;

        // clamp the player's position to the camera's viewport
        Vector3 clampedPosition = Camera.main.WorldToViewportPoint(transform.position);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0.05f, 0.95f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(clampedPosition);
    }

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
}

