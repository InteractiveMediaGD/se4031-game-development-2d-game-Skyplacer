using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Item Prefabs")]
    public GameObject enemyPrefab;
    public GameObject healthPackPrefab;

    [Header("Obstacle Variations")]
    public GameObject gatePrefab;           // Top + Bottom + ScoreZone
    public GameObject ceilingPillarPrefab;  // Top + ScoreZone
    public GameObject floorPillarPrefab;    // Bottom + ScoreZone

    [Header("Spawn Settings")]
    public float spawnRate = 2f; 
    public float minY = -4.5f;   // Position for Floor Pillars
    public float maxY = 4.5f;    // Position for Ceiling Pillars
    
    private float nextSpawnTime;

    void Update()
    {
        // Requirement 6: Speed scaling
        float currentSpawnRate = spawnRate / (GameManager.GlobalSpeed / 5f);

        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + currentSpawnRate;
        }
    }

    void SpawnObject()
    {
        float chance = Random.value; 

        // 1. Handle Health Packs (15% chance)
        if (chance < 0.15f) 
        {
            SpawnAtRandomHeight(healthPackPrefab);
        }
        // 2. Handle Enemies (30% chance)
        else if (chance < 0.45f) 
        {
            SpawnAtRandomHeight(enemyPrefab);
        }
        // 3. Handle Obstacles (55% chance)
        else 
        {
            SpawnObstacleVariation();
        }
    }

    void SpawnAtRandomHeight(GameObject prefab)
    {
        float randomY = Random.Range(minY + 1f, maxY - 1f); // Keep items away from very edges
        Instantiate(prefab, new Vector3(transform.position.x, randomY, 0), Quaternion.identity);
    }

    void SpawnObstacleVariation()
    {
        float obstacleChance = Random.value;
        GameObject selectedPrefab;
        float spawnY;

        if (obstacleChance < 0.4f) // 40% chance for a Gate
        {
            selectedPrefab = gatePrefab;
            // Keep the "hole" in the reachable middle area (Requirement 2)
            spawnY = Random.Range(-2f, 2f); 
        }
        else if (obstacleChance < 0.7f) // 30% chance for Ceiling Pillar
        {
            selectedPrefab = ceilingPillarPrefab;
            spawnY = maxY; // Pin to the top
        }
        else // 30% chance for Floor Pillar
        {
            selectedPrefab = floorPillarPrefab;
            spawnY = minY; // Pin to the bottom
        }

        Instantiate(selectedPrefab, new Vector3(transform.position.x, spawnY, 0), Quaternion.identity);
    }
}