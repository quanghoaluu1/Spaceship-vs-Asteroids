using UnityEngine;

public class EnemyGunStraight : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 1.5f;
    public Vector2 bulletDirection = Vector2.left;

    void Start()
    {
        InvokeRepeating(nameof(FireBullet), 1f, fireRate);
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Set hướng cho viên đạn
        EnemyBulletStraight bulletScript = bullet.GetComponent<EnemyBulletStraight>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(bulletDirection);
        }
    }
}
