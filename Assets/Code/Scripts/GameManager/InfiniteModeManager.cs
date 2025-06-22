using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteModeManager : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public EnemyController enemyPrefab1;
    public EnemyController enemyPrefab2;
    public GameObject bossPrefab1;
    public GameObject bossPrefab2;
    private int bossSpawnCount = 0;
    public Transform bossSpawnPoint;

    private int bossSpawnRequirement = 50; // điểm số yêu cầu để spawn boss
    private int enemySpawnRequirement = 20; // điểm số yêu cầu để spawn enemy

    private float asteroidSpawnRate = 2f;
    private float enemySpawnRate = 3f;
    private GameObject currentBoss;
    private Coroutine spawnRoutine;
    private int currentTotalScore = 0;
    private bool isBossAlive = false;

    public float bossMoveDistance = 6f;
    public float bossMoveDuration = 2f;
    public Transform boss3SpawnPoint;
    public Vector3 bossScaleMultiplier = new Vector3(2.5f, 2.5f, 1f);

    private enum GameMode
    {
        Asteroid,
        Enemy,
        Boss
    }

    private GameMode currentMode = GameMode.Asteroid;

    void Start()
    {
        SetMode(GameMode.Asteroid);
    }

    void Update()
    {
        int score = ScoreManager.Instance.score;
        //int score = 50; // Giả sử score là 50 để test

        if (isBossAlive) return; // chờ boss chết mới chuyển

        int scoreDelta = score - currentTotalScore;

        if (scoreDelta >= bossSpawnRequirement)
        {
            if (currentMode != GameMode.Boss)
                SetMode(GameMode.Boss);
        }
        else if (scoreDelta >= enemySpawnRequirement)
        {
            if (currentMode != GameMode.Enemy)
                SetMode(GameMode.Enemy);
        }
        else
        {
            if (currentMode != GameMode.Asteroid)
                SetMode(GameMode.Asteroid);
        }
    }

    void SetMode(GameMode newMode)
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        currentMode = newMode;

        switch (newMode)
        {
            case GameMode.Asteroid:
                spawnRoutine = StartCoroutine(SpawnAsteroids());
                break;
            case GameMode.Enemy:
                spawnRoutine = StartCoroutine(SpawnEnemies());
                break;
            case GameMode.Boss:
                SpawnBoss();
                break;
        }
    }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(asteroidSpawnRate);
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float minY = 0.05f;
            float maxY = 0.95f;
            List<float> usedY = new List<float>();

            // Hàm tạo Y không trùng
            float GenerateUniqueY()
            {
                float y;
                int tries = 0;
                do
                {
                    y = Random.Range(minY, maxY);
                    tries++;
                } while (usedY.Exists(v => Mathf.Abs(v - y) < 0.15f) && tries < 10); // tránh trùng vị trí gần nhau
                usedY.Add(y);
                return y;
            }

            // Máy bay loại 1 - enemyPrefab1
            {
                float y = GenerateUniqueY();
                Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1f, y, 0f));
                spawnPoint.z = 0f;

                EnemyController enemy = Instantiate(enemyPrefab1, spawnPoint, Quaternion.identity);
                enemy.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            }

            // Máy bay loại 2 - enemyPrefab2
            {
                float y = GenerateUniqueY();
                Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1f, y, 0f));
                spawnPoint.z = 0f;

                EnemyController enemy = Instantiate(enemyPrefab2, spawnPoint, Quaternion.identity);
                enemy.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
            }

            // Nếu score > 50 → spawn thêm 1 enemyPrefab2
            if (ScoreManager.Instance.score > 50)
            {
                float y = GenerateUniqueY();
                Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1f, y, 0f));
                spawnPoint.z = 0f;

                EnemyController enemy = Instantiate(enemyPrefab2, spawnPoint, Quaternion.identity);
                enemy.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
            }

            // Nếu score > 100 → spawn thêm 1 enemyPrefab1
            if (ScoreManager.Instance.score > 100)
            {
                float y = GenerateUniqueY();
                Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1f, y, 0f));
                spawnPoint.z = 0f;

                EnemyController enemy = Instantiate(enemyPrefab1, spawnPoint, Quaternion.identity);
                enemy.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            }

            yield return new WaitForSeconds(enemySpawnRate);
        }
    }

    void SpawnAsteroid()
    {
        float randomY = Random.Range(0f, 1f);
        Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1f, randomY, 0f));
        spawnPoint.z = 0f;

        Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, Quaternion.identity);
        asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
        asteroid.maxLifetime = 10f;
        asteroid.SetTrajectory(Vector2.left);
    }

    void SpawnBoss()
    {
        isBossAlive = true;

        // 🌟 Luân phiên chọn boss
        GameObject bossToSpawn = (bossSpawnCount % 2 == 0) ? bossPrefab1 : bossPrefab2;

        // === Nếu là Boss3 ===
        if (bossToSpawn == bossPrefab2 && boss3SpawnPoint != null)
        {
            Vector3 spawnFrom = boss3SpawnPoint.position + Vector3.right * bossMoveDistance;
            currentBoss = Instantiate(bossToSpawn, spawnFrom, boss3SpawnPoint.rotation);

            // Scale lớn hơn (nếu muốn)
            currentBoss.transform.localScale = Vector3.Scale(currentBoss.transform.localScale, bossScaleMultiplier);

            // Di chuyển vào vị trí
            StartCoroutine(MoveToPosition(currentBoss.transform, boss3SpawnPoint.position, bossMoveDuration));
        }
        // === Các boss khác ===
        else if (bossSpawnPoint != null)
        {
            Vector3 spawnFrom = bossSpawnPoint.position;
            currentBoss = Instantiate(bossToSpawn, spawnFrom, bossSpawnPoint.rotation);
        }

        // Kết nối sự kiện bị tiêu diệt
        BossController bossController = currentBoss.GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.OnBossDefeated += HandleBossDefeated;
        }

        // Nếu là Boss3 thì nghe Boss3Defeated
        Boss3 boss3 = currentBoss.GetComponent<Boss3>();
        if (boss3 != null)
        {
            boss3.OnBoss3Defeated += HandleBossDefeated;
        }

        bossSpawnCount++;
    }

    IEnumerator MoveToPosition(Transform objTransform, Vector3 targetPosition, float duration)
        {
            Vector3 startPos = objTransform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                objTransform.position = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            objTransform.position = targetPosition;
        }

    public void HandleBossDefeated()
    {
        isBossAlive = false;
        currentTotalScore = ScoreManager.Instance.score + 1;
        SetMode(GameMode.Asteroid);
    }

    // InfiniteModeManager.cs
    public void ResetLogic()
    {
        currentTotalScore = 0;
        isBossAlive = false;
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        SetMode(GameMode.Asteroid);
    }

}
