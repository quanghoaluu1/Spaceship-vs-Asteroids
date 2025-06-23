using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public Vector2 moveDirection = Vector2.left;
    
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.fixedDeltaTime);
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

            playerController.ActivateShield(); // Bật trạng thái bất tử + khiên + nhấp nháy
            playerController.TakeDamage(20);
            Destroy(this.gameObject);
        }
    }
}
