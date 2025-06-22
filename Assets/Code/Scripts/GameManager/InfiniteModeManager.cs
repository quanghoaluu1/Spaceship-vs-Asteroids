using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteModeManager : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public EnemyController enemyPrefab1;
    public EnemyController enemyPrefab2;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    private int bossSpawnRequirement = 50; // điểm số yêu cầu để spawn boss
    private int enemySpawnRequirement = 20; // điểm số yêu cầu để spawn enemy

    private float asteroidSpawnRate = 2f;
    private float enemySpawnRate = 3f;
    private GameObject currentBoss;
    private Coroutine spawnRoutine;
    private int currentTotalScore = 0;
    private bool isBossAlive = false;

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

    void SpawnEnemy(EnemyController prefab, Vector3 scale)
    {
        float randomY = Random.Range(0f, 1f);
        Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1f, randomY, 0f));
        spawnPoint.z = 0f;

        EnemyController enemy = Instantiate(prefab, spawnPoint, Quaternion.identity);
        enemy.transform.localScale = scale;
    }

    void SpawnBoss()
    {
        isBossAlive = true;

        // Lùi trái 6f so với bossSpawnPoint
        Vector3 spawnPosition = bossSpawnPoint != null
            ? bossSpawnPoint.position - Vector3.left * 4f
            : Vector3.left * 6f;

        currentBoss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

        BossController bossController = currentBoss.GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.OnBossDefeated += HandleBossDefeated;
        }
    }

    public void HandleBossDefeated()
    {
        isBossAlive = false;
        currentTotalScore = ScoreManager.Instance.score + 1;
        SetMode(GameMode.Asteroid);
    }
}
