using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    public GameObject lazerShooterPrefab;
    public Transform player;
    public Transform[] spawnPoints; // 6 vị trí spawn
    public GameObject spawnEffectPrefab;
    public float LazerShooterCooldown = 10f;

    public GameObject spinningBulletPrefab;
    public Transform bulletFirePoint;
    public float fanBulletSpeed = 5f;
    public float fanAngle = 60f; // tổng góc xoè
    public float fireFanDelay = 2f; // thời gian đợi trước khi bắn fan
    public float plasmaBallCooldown = 6f;
    private float plasmaBallTimer = 0f;


    private bool isFiring = false;

    void Update()
    {
        // LaserShooter tự động theo Coroutine
        LaserShooters();

        // PlasmaBall: tự cooldown theo timer
        plasmaBallTimer += Time.deltaTime;
        if (plasmaBallTimer >= plasmaBallCooldown)
        {
            plasmaBallTimer = 0f;
            PlasmaBall();
        }
    }

    public void LaserShooters()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(LaserShooterPattern());
        }
    }

    public void PlasmaBall()
    {
        FireFanBullets(5);
    }


    IEnumerator LaserShooterPattern()
    {
        yield return new WaitForSeconds(2.5f);

        List<GameObject> activeShooters = new List<GameObject>();

        // --- Đợt 1 ---
        var shooters1 = new List<GameObject>();
        yield return StartCoroutine(SpawnAndShoot(0, 2, shooters1));
        activeShooters.AddRange(shooters1);
        yield return new WaitForSeconds(1f);

        // --- Đợt 2 ---
        var shooters2 = new List<GameObject>();
        yield return StartCoroutine(SpawnAndShoot(2, 2, shooters2));
        activeShooters.AddRange(shooters2);
        yield return new WaitForSeconds(1f);

        // --- Đợt 3 ---
        var shooters3 = new List<GameObject>();
        yield return StartCoroutine(SpawnAndShoot(4, 2, shooters3));
        activeShooters.AddRange(shooters3);
        yield return new WaitForSeconds(1.5f);

        // --- Xoá toàn bộ ---
        foreach (var shooter in activeShooters)
        {
            if (shooter != null) Destroy(shooter);
        }

        yield return new WaitForSeconds(0f);

        // --- Đợt 4: toàn bộ 6 cái ---
        var finalWave = new List<GameObject>();
        yield return StartCoroutine(SpawnAndShoot(0, 6, finalWave));

        yield return new WaitForSeconds(2f);

        foreach (var shooter in finalWave)
        {
            if (shooter != null) Destroy(shooter);
        }

        // --- Cooldown ---
        yield return new WaitForSeconds(LazerShooterCooldown);

        isFiring = false; // Cho phép lặp lại nếu muốn
    }

    IEnumerator SpawnAndShoot(int startIndex, int count, List<GameObject> result)
    {
        for (int i = startIndex; i < startIndex + count && i < spawnPoints.Length; i++)
        {
            Transform point = spawnPoints[i];

            if (spawnEffectPrefab != null)
                Instantiate(spawnEffectPrefab, point.position, Quaternion.identity);

            GameObject shooter = Instantiate(lazerShooterPrefab, point.position, Quaternion.identity);

            // Xoay trục X về phía player
            Vector3 dir = (player.position - point.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            shooter.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            var lazerShooter = shooter.GetComponent<LazerShooter>();
            lazerShooter.SetTarget(player);

            result.Add(shooter);

            // Bắn sau 0.5s
            StartCoroutine(FireWithDelay(lazerShooter, 0.5f));
        }

        yield return null;
    }

    IEnumerator FireWithDelay(LazerShooter shooter, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (shooter != null)
        {
            shooter.FireLaserImmediately();
        }
    }

    void FireFanBullets(int bulletCount)
    {
        float startAngle = -fanAngle / 2f;
        float angleStep = fanAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            // 🔁 Đảo hướng trục X (xoay ngược 180 độ quanh trục Z)
            float adjustedAngle = 180f + angle;
            float rad = adjustedAngle * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            GameObject bullet = Instantiate(spinningBulletPrefab, bulletFirePoint.position, Quaternion.identity);
            var bulletScript = bullet.GetComponent<SpinningBullet>();
            bulletScript.SetDirection(dir);
            bulletScript.speed = fanBulletSpeed;
        }
    }


}
