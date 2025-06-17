using System.Collections;
using UnityEngine;

public class BossGun : MonoBehaviour
{
    public GameObject EnemyBullet;

    void Start()
    {
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        // 💥 Boss chờ 1.5s trước khi bắt đầu xả đạn
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            for (int i = 0; i < 3; i++) // Bắn 3 viên
            {
                FireEnemyBullet();
                yield return new WaitForSeconds(0.5f); // Chờ 0.5s giữa mỗi viên
            }

            yield return new WaitForSeconds(2f); // Nghỉ 2s sau khi bắn đủ 3 viên
        }
    }

    void FireEnemyBullet()
    {
        GameObject playerShip = GameObject.Find("Player");

        if (playerShip != null)
        {
            GameObject bullet = Instantiate(EnemyBullet, transform.position, Quaternion.identity);

            Vector2 direction = playerShip.transform.position - bullet.transform.position;

            BossBullet bulletScript = bullet.GetComponent<BossBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
        }
    }
}
