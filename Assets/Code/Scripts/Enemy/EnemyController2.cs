using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour
{
    public float horizontalSpeed = 3f;
    public float verticalSpeed = 2f;
    private float travelDistance = 6f;

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool startVerticalMovement = false;
    private Vector2 verticalDirection = Vector2.up;
    private float verticalPadding = 3f;

    public delegate void BossDefeated();
    public event BossDefeated OnBossDefeated;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!startVerticalMovement)
        {
            Vector2 newPos = rb.position + Vector2.left * horizontalSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            if (Vector2.Distance(initialPosition, rb.position) >= travelDistance)
            {
                startVerticalMovement = true;
            }
        }
        else
        {
            Vector2 currentPos = rb.position;
            Vector2 newPos = currentPos + verticalDirection * verticalSpeed * Time.fixedDeltaTime;

            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            // Nếu sắp vượt khỏi rìa, thì đổi hướng trước khi áp dụng vị trí mới
            if (newPos.y >= max.y - verticalPadding || newPos.y <= min.y + verticalPadding)
            {
                verticalDirection *= -1;
            }

            rb.MovePosition(currentPos + verticalDirection * verticalSpeed * Time.fixedDeltaTime);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController == null) return;


            if (playerController.IsInvincible())
            {
                Debug.Log("Player đang bất tử, không mất máu. Còn " + playerController.IsInvincibleTime() + " giây.");
                return;
            }

            playerController.ActivateShield(); // Bật trạng thái bất tử + khiên + nhấp nháy

            playerController.LoseLife();

        }

        if (collision.gameObject.CompareTag("Asteroid"))
        {
            // Xóa thiên thạch
            Destroy(collision.gameObject);
        }
    }
    public void Die()
    {
        // Hiệu ứng chết, v.v...
        OnBossDefeated?.Invoke();
        Destroy(gameObject);
    }
}
