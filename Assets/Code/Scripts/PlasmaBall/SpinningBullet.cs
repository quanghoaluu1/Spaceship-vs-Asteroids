using UnityEngine;

public class SpinningBullet : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 180f;

    private Vector2 moveDirection;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    void Update()
    {
        // Di chuyển
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // Xoay
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Huỷ khi ra khỏi màn hình
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPoint.x < -0.1f || screenPoint.x > 1.1f || screenPoint.y < -0.1f || screenPoint.y > 1.1f)
        {
            Destroy(gameObject);
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
                Destroy(this.gameObject);
                return;
            }

            //PlaySoundAtPosition(getHitSound, transform.position, 5f);

            playerController.TakeDamage(50); // Gọi hàm mất máu, nếu có
            playerController.ActivateShield(); // Bật trạng thái bất tử + khiên + nhấp nháy

            //playerController.LoseLife();
            Destroy(this.gameObject);
        }
    }
}
