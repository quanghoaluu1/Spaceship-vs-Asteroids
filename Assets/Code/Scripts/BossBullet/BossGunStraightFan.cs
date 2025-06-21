using UnityEngine;
using System.Collections;

public class BossGunStraightFan : MonoBehaviour
{
    public GameObject bulletPrefab;
    private float fireRate = 1.5f;
    private float angleStep = 15f; // góc lệch giữa các viên đạn
    private int bulletCount = 5;   // tổng số viên mỗi lần bắn

    void Start()
    {
        // Dùng Coroutine thay vì InvokeRepeating để có thể chờ 1.5s lúc đầu
        StartCoroutine(FireBulletFanRoutine());
    }

    IEnumerator FireBulletFanRoutine()
    {
        // 💥 Delay 1.5s trước khi bắt đầu bắn
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            FireBulletFan();
            yield return new WaitForSeconds(fireRate); // Lặp lại sau mỗi fireRate giây
        }
    }

    void FireBulletFan()
    {
        float startAngle = -angleStep * (bulletCount - 1) / 2f; // Bắt đầu từ góc âm (trái xuống)
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 dir = RotateVector(Vector2.left, angle);
            FireBullet(dir);
        }
    }

    void FireBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        BossBulletStraightFan bulletScript = bullet.GetComponent<BossBulletStraightFan>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }

    // Hàm xoay 1 vector 2D theo độ
    Vector2 RotateVector(Vector2 v, float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
