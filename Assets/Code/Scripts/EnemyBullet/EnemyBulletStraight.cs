using UnityEngine;

public class EnemyBulletStraight : MonoBehaviour
{
    private Vector2 direction;
    public float speed = 6f;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Huỷ nếu ra ngoài màn hình
        Vector2 min = Camera.main.ViewportToWorldPoint(Vector2.zero);
        Vector2 max = Camera.main.ViewportToWorldPoint(Vector2.one);
        if (transform.position.x < min.x || transform.position.x > max.x ||
            transform.position.y < min.y || transform.position.y > max.y)
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
                //Debug.Log("Player đang bất tử, không mất máu. Còn " + playerController.IsInvincibleTime() + " giây.");
                Destroy(this.gameObject);
                return;
            }

            //PlaySoundAtPosition(getHitSound, transform.position, 5f);

            //Nếu đến đây là chắc chắn chưa bất tử → xử lý mất máu và kích hoạt khiên
            playerController.ActivateShield(); // Bật trạng thái bất tử + khiên + nhấp nháy

            playerController.TakeDamage(20);
            Destroy(this.gameObject);
        }
    }
}
