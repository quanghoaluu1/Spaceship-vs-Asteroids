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

    public GameObject bossStraightBulletPrefab;
    public Transform firePoint1;
    public Transform firePoint2;
    public float straightBulletSpeed = 8f;
    public int bulletBurstCount = 10;
    private bool canFireStraight = true; // để chặn spam

    public Transform firePointX1;
    public Transform firePointX2;
    public float bulletSpeedX = 10f;
    public float shootInterval = 1f;
    public int numberOfShots = 5;
    public float verticalMoveSpeed = 3f;
    public float verticalMoveRange = 2f;
    private float colliderHalfHeight = 0f;

    public Transform specialGun1;
    public Transform specialGun2;
    public GameObject specialBulletPrefab;
    public float specialGunRange = 2f;
    public float specialCooldown = 3f;
    private bool canShootSpecial1 = true;
    private bool canShootSpecial2 = true;


    void Awake()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    void Start()
    {
        CalculateColliderHalfHeight();
        StartCoroutine(BossAttackLoop());
    }

    void Update()
    {
        if (player == null) return;

        if (canShootSpecial1 && Vector2.Distance(player.position, specialGun1.position) <= specialGunRange)
        {
            StartCoroutine(ShootSpecialFrom(specialGun1));
            canShootSpecial1 = false;
            StartCoroutine(ResetSpecialCooldown(1));
        }

        if (canShootSpecial2 && Vector2.Distance(player.position, specialGun2.position) <= specialGunRange)
        {
            StartCoroutine(ShootSpecialFrom(specialGun2));
            canShootSpecial2 = false;
            StartCoroutine(ResetSpecialCooldown(2));
        }
    }

    void CalculateColliderHalfHeight()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
            colliderHalfHeight = renderer.bounds.extents.y;
    }

    IEnumerator BossAttackLoop()
    {
        while (true)
        {
            // 1. Bắn laser
            yield return StartCoroutine(LaserShooterPattern());

            // Chờ 0.3s sau khi laser xong
            yield return new WaitForSeconds(0.3f);

            // 2. Bắn đạn thẳng đôi
            yield return StartCoroutine(FireStraightAndXBullets());

            // Chờ 0.3s sau khi đạn thẳng xong
            yield return new WaitForSeconds(0.3f);

            // 3. Bắn plasma
            PlasmaBall();

            // Chờ 0.5s rồi tiếp tục vòng mới
            yield return new WaitForSeconds(0.5f);

            // 🔁 Cooldown cho toàn bộ chu kỳ (thay vì để trong LaserShooterPattern)
            //yield return new WaitForSeconds(LazerShooterCooldown);
        }
    }

    public void PlasmaBall()
    {
        FireFanPlasmaBall(5);
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
        //yield return new WaitForSeconds(LazerShooterCooldown);

        //isFiring = false; // Cho phép lặp lại nếu muốn
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

    void FireFanPlasmaBall(int bulletCount)
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

    public void StartDualStraightFire()
    {
        if (canFireStraight)
        {
            StartCoroutine(FireStraightBullets());
        }
    }

    IEnumerator FireStraightBullets()
    {
        canFireStraight = false;

        int totalShots = 10;
        float shotInterval = 0.3f;

        for (int i = 0; i < totalShots; i++)
        {
            FireBulletFrom(firePoint1);
            FireBulletFrom(firePoint2);
            yield return new WaitForSeconds(shotInterval);
        }

        // Cooldown sau khi bắn xong
        //yield return new WaitForSeconds(5f);

        canFireStraight = true;
    }

    void FireBulletFrom(Transform firePoint)
    {
        GameObject bullet = Instantiate(bossStraightBulletPrefab, firePoint.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<BossBulletStraight>();
        bulletScript.SetDirection(Vector2.left);
        //bulletScript.SetSpeed(straightBulletSpeed);
    }

    public IEnumerator FireStraightAndXBullets()
    {
        canFireStraight = false;

        for (int i = 0; i < numberOfShots; i++) // số đợt
        {
            // 🔁 Bắn 3 lần trong 1 đợt
            for (int j = 0; j < 3; j++)
            {
                FireBulletFrom(firePoint1);
                FireBulletFrom(firePoint2);
                FireXBullets();

                yield return new WaitForSeconds(0.3f); // khoảng cách giữa mỗi phát
            }

            yield return StartCoroutine(MoveSmartUpOrDown()); // chỉ di chuyển sau mỗi đợt

            yield return new WaitForSeconds(shootInterval); // giữa các đợt
        }

        canFireStraight = true;
    }

    void FireXBullets()
    {
        FireSingleXBullet(firePointX1);
        FireSingleXBullet(firePointX2);
    }

    void FireSingleXBullet(Transform firePoint)
    {
        if (firePoint == null || bossStraightBulletPrefab == null)
            return;

        GameObject bullet = Instantiate(bossStraightBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeedX;
        }
    }

    IEnumerator MoveSmartUpOrDown()
    {
        Camera cam = Camera.main;
        if (cam == null) yield break;

        float camZ = transform.position.z - cam.transform.position.z;
        float minY = cam.ViewportToWorldPoint(new Vector3(0, 0, camZ)).y + colliderHalfHeight;
        float maxY = cam.ViewportToWorldPoint(new Vector3(0, 1, camZ)).y - colliderHalfHeight;

        float currentY = transform.position.y;

        // Xác định hướng di chuyển (tránh sát mép thì đổi hướng)
        int direction;
        if (Mathf.Abs(currentY - maxY) < 0.1f)
            direction = -1; // sát trên → xuống
        else if (Mathf.Abs(currentY - minY) < 0.1f)
            direction = 1; // sát dưới → lên
        else
            direction = Random.value > 0.5f ? 1 : -1;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, direction * verticalMoveRange, 0);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime * verticalMoveSpeed;
            yield return null;
        }

        transform.position = targetPosition;
    }

    IEnumerator ShootSpecialFrom(Transform firePoint)
    {
        // 🌀 Xoay họng súng về phía player (theo trục X)
        Vector3 direction = player.position - firePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        for (int i = 0; i < 5; i++) // 🔁 Bắn 5 viên
        {
            GameObject bullet = Instantiate(specialBulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 🎯 Bay theo trục X của họng súng
                rb.linearVelocity = firePoint.right * bulletSpeedX;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator ResetSpecialCooldown(int gunIndex)
    {
        yield return new WaitForSeconds(specialCooldown);
        if (gunIndex == 1) canShootSpecial1 = true;
        else if (gunIndex == 2) canShootSpecial2 = true;
    }

}
