using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public float lifeTime = 2f; // Thời gian (giây) trước khi đạn tự hủy

    void Start()
    {
        // Giả sử đạn bắn "lên trên" so với hướng xoay của nó
        rb.linearVelocity = transform.up * speed;
        // Hủy đối tượng đạn sau một khoảng thời gian nhất định để tránh làm đầy bộ nhớ
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Ví dụ: Kiểm tra xem có va chạm với kẻ thù không
        EnemyController enemy = hitInfo.GetComponent<EnemyController>();
        if (enemy != null)
        {
            // Thêm logic để gây sát thương cho kẻ thù
            // Destroy(enemy.gameObject); // Ví dụ: Hủy kẻ thù
            Destroy(gameObject); // Hủy viên đạn
        }

        // Thêm các kiểm tra va chạm khác nếu cần (ví dụ: với thiên thạch)
        // Asteroid asteroid = hitInfo.GetComponent<Asteroid>();
        // if (asteroid != null)
        // {
        //     // Thêm logic để gây sát thương hoặc phá hủy thiên thạch
        //     Destroy(gameObject); // Hủy viên đạn
        // }
    }
}