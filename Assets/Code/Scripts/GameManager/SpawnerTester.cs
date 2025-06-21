using UnityEngine;

public class SpawnerTester : MonoBehaviour
{
    public GameObject bossPrefab;              // Prefab của boss 3
    public Transform bossSpawnPoint;           // Điểm spawn
    public GameObject spawnEffectPrefab;       // Hiệu ứng khi spawn (tùy chọn)

    private GameObject currentBoss;

    void Update()
    {
        // Nhấn phím B để triệu hồi boss
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null)
        {
            Debug.LogWarning("Thiếu prefab hoặc điểm spawn");
            return;
        }

        // Hiệu ứng spawn nếu có
        if (spawnEffectPrefab != null)
        {
            Instantiate(spawnEffectPrefab, bossSpawnPoint.position, Quaternion.identity);
        }

        // Triệu hồi boss và phóng to
        currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        currentBoss.transform.localScale *= 1.3f;

        // (Optional) Có thể add animation hoặc effect nữa tại đây
        Debug.Log("Boss đã được triệu hồi!");
    }
}
