using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarSpawn : MonoBehaviour
{
    public Star starPrefab;
    public float trajectoryVariance = 15f;
    public float spawnRate = 5f;
    public float spawnDistance = 15f;
    public int spawnAmount = 1;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
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
