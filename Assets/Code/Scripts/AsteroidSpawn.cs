using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawn : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    public float trajectoryVariance = 15f;
    public float spawnRate = 2f;
    public float spawnDistance = 15f;
    public int spawnAmount = 3;

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

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);

            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.maxLifetime = 10f;

            asteroid.SetTrajectory(Vector2.left);
        }
    }
}
