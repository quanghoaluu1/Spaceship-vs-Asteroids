using UnityEngine;

public class BossBulletStraight : MonoBehaviour
{
    private Vector2 direction = Vector2.left; // Mặc định bắn trái
    private float speed = 8f;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Huỷ nếu ra khỏi màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 max = Camera.main.ViewportToWorldPoint(Vector2.one);
        if (transform.position.x < min.x || transform.position.x > max.x ||
            transform.position.y < min.y || transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player == null) return;

            if (!player.IsInvincible())
            {
                player.ActivateShield();
            }

            Destroy(gameObject);
        }
    }
}
