using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController enemyPrefab;
    private float spawnRate = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        float randomY = Random.Range(0f, 1f); // Tọa độ Y ngẫu nhiên trong viewport

        Vector3 viewportPosition = new Vector3(1f, randomY, 0f); // <-- Z = 0
        Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(viewportPosition);
        spawnPoint.z = 0f;

        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        EnemyController newEnemy = Instantiate(enemyPrefab, spawnPoint, rotation);

        newEnemy.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
    }
}
