using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power-Up Prefabs")]
    public GameObject[] powerUpPrefabs;

    [Header("Spawn Timing")]
    public float spawnInterval = 8f; // thời gian giữa mỗi lần spawn
    public float spawnChance = 0.5f; // tỉ lệ xuất hiện (50%)

    [Header("Padding")]
    public float verticalPadding = 0.5f; // để Power-Up không dính sát mép trên/dưới

    private float minY;
    private float maxY;
    private float cameraRightEdge;

    void Start()
    {
        // Tính toán rìa phải màn hình
        cameraRightEdge = Camera.main.ViewportToWorldPoint(Vector3.right).x + 1f;

        // Tính toán minY / maxY theo màn hình
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));

        minY = bottomLeft.y + verticalPadding;
        maxY = topLeft.y - verticalPadding;

        // Lặp lại việc spawn
        InvokeRepeating(nameof(TrySpawnPowerUp), 2f, spawnInterval);
    }

    void TrySpawnPowerUp()
    {
        if (Random.value <= spawnChance)
        {
            SpawnRandomPowerUp();
        }
    }

    void SpawnRandomPowerUp()
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0) return;

        int index = Random.Range(0, powerUpPrefabs.Length);
        GameObject prefab = powerUpPrefabs[index];

        float randomY = Random.Range(0f, 1f);
        Vector3 viewportPos = new Vector3(1f, randomY, Camera.main.nearClipPlane);
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(viewportPos);
        spawnPos.z = 0f;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

}
