using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 moveInput;
    public float invincibleDuration = 5f;
    private float lastHitTime = -999f;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();

       
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
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
}
