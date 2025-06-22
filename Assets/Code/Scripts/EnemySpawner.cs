using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController enemyPrefab;
    public EnemyController enemyPrefab2;
    private float spawnRate = 2f;

    void Start()
    {
        Invoke(nameof(StartSpawningEnemy1), 0f); // sau 0s
        Invoke(nameof(StartSpawningEnemy2), 10f); // sau 30s
        Invoke(nameof(StartSpawningBoth), 20f);   // sau 60s
    }

    void StartSpawningEnemy1()
    {
        InvokeRepeating(nameof(SpawnEnemy1), 0f, 2f); // spawn mỗi 2s
    }

    void StartSpawningEnemy2()
    {
        InvokeRepeating(nameof(SpawnEnemy2), 0f, 3f); // spawn mỗi 3s
    }

    void StartSpawningBoth()
    {
        InvokeRepeating(nameof(SpawnBoth), 0f, 5f); // spawn cả hai mỗi 5s
    }

    void SpawnEnemy1()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();
        EnemyController enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        enemy.transform.localScale = new Vector3(0.6f, 0.6f, 1f); // nhỏ hơn
    }

    void SpawnEnemy2()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();
        EnemyController enemy = Instantiate(enemyPrefab2, spawnPoint, Quaternion.identity);
        enemy.transform.localScale = new Vector3(0.45f, 0.45f, 1f); // nhỏ hơn
    }

    void SpawnBoth()
    {
        Vector3 spawn1 = GetRandomSpawnPoint();
        Vector3 spawn2 = GetRandomSpawnPoint();

        EnemyController enemy1 = Instantiate(enemyPrefab, spawn1, Quaternion.identity);
        enemy1.transform.localScale = new Vector3(0.6f, 0.6f, 1f); // máy bay 1

        EnemyController enemy2 = Instantiate(enemyPrefab2, spawn2, Quaternion.identity);
        enemy2.transform.localScale = new Vector3(0.45f, 0.45f, 1f); // máy bay 2 nhỏ hơn
    }

    Vector3 GetRandomSpawnPoint()
    {
        float randomY = Random.Range(0f, 1f);
        Vector3 viewportPos = new Vector3(1f, randomY, 0f);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0f;
        return worldPos;
    }

}
