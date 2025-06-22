using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarSpawn : MonoBehaviour
{
    public Star starPrefab;
    private float spawnRate = 5f;
    private int spawnAmount = 1;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        if (ScoreManager.Instance.score >= 80)
        {
            spawnAmount = 2; // Increase spawn rate if score >= 80
        }
        else if (ScoreManager.Instance.score >= 40)
        {
            spawnRate = 4f; // Increase spawn amount if score >= 40
        }

        for (int i = 0; i < spawnAmount; i++)
        {
            float randomY = Random.Range(0f, 1f);

            Vector3 viewportPosition = new Vector3(1f, randomY, Camera.main.nearClipPlane);
            Vector3 spawnPoint = Camera.main.ViewportToWorldPoint(viewportPosition);
            spawnPoint.z = 0f;

            Quaternion rotation = Quaternion.identity;

            Star star = Instantiate(starPrefab, spawnPoint, rotation);

            star.size = 0.2f;
            star.maxLifetime = 10f;

            star.SetTrajectory(Vector2.left);
        }
    }
}
