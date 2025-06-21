using UnityEngine;

public class SpawnerTester : MonoBehaviour
{
    public GameObject boss3Prefab;           // Prefab của Boss 3
    public Transform bossSpawnPoint;         // Vị trí spawn (gán trong Inspector)
    public float moveDistance = 6f;          // Khoảng cách boss di chuyển vào màn hình
    public float moveDuration = 2f;          // Thời gian boss di chuyển vào màn hình
    public Vector3 bossScaleMultiplier = new Vector3(1.2f, 1.2f, 1.2f); // Scale tăng lên

    private GameObject spawnedBoss;

    void Start()
    {
        SpawnBoss3();
    }

    void SpawnBoss3()
    {
        if (boss3Prefab == null || bossSpawnPoint == null)
        {
            Debug.LogWarning("Thiếu prefab hoặc BossSpawnPoint");
            return;
        }

        // Spawn boss tại vị trí ngoài màn hình (bên phải một chút)
        Vector3 spawnPosition = bossSpawnPoint.position + new Vector3(moveDistance, 0f, 0f);
        spawnedBoss = Instantiate(boss3Prefab, spawnPosition, bossSpawnPoint.rotation);

        // Tăng scale
        spawnedBoss.transform.localScale = Vector3.Scale(spawnedBoss.transform.localScale, bossScaleMultiplier);

        // Di chuyển vào vị trí BossSpawnPoint trong moveDuration giây
        StartCoroutine(MoveToPosition(spawnedBoss.transform, bossSpawnPoint.position, moveDuration));
    }

    System.Collections.IEnumerator MoveToPosition(Transform objTransform, Vector3 targetPosition, float duration)
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
}
