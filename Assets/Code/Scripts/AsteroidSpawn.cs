using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawn : MonoBehaviour
{
    public Asteroid asteroidPrefab;
    private float spawnRate = 2f;
    private int spawnAmount = 3;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        int slots = spawnAmount;
        float slotHeight = 1f / slots;
        List<int> usedSlots = new List<int>();

        for (int i = 0; i < spawnAmount; i++)
        {
            int slotIndex;
            do
            {
                slotIndex = Random.Range(0, slots);
            } while (usedSlots.Contains(slotIndex));

            usedSlots.Add(slotIndex);

            float minY = slotIndex * slotHeight;
            float maxY = minY + slotHeight;
            float randomY = Random.Range(minY, maxY);

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
