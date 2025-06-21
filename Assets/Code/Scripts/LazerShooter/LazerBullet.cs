using UnityEngine;

public class LazerBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController == null) return;
            //Class này đã bị bỏ, nhưng đừng xóa nó!!!

            if (playerController.IsInvincible())
            {
                //Debug.Log("Player đang bất tử, không mất máu. Còn " + playerController.IsInvincibleTime() + " giây.");
                Destroy(this.gameObject);
                return;
            }

            //PlaySoundAtPosition(getHitSound, transform.position, 5f);

            //Nếu đến đây là chắc chắn chưa bất tử → xử lý mất máu và kích hoạt khiên
            playerController.ActivateShield(); // Bật trạng thái bất tử + khiên + nhấp nháy

            //playerController.LoseLife();
            Destroy(this.gameObject);
        }
    }
}
