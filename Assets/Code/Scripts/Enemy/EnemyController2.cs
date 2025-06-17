using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossEnemyController : MonoBehaviour
{
    public float horizontalSpeed = 3f;
    public float verticalSpeed = 2f;
    private float travelDistance = 7f;

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool startVerticalMovement = false;
    private Vector2 verticalDirection = Vector2.up;
    private float verticalPadding = 3f;

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
}
