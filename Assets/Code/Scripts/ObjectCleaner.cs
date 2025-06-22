using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu đối tượng va chạm là một thiên thạch
        if (other.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnemySpaceship"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Star"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            Destroy(other.gameObject);
        }
    }
}
