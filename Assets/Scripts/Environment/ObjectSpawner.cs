using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatternSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject gatePrefab;
    public GameObject ceilingPillarPrefab;
    public GameObject floorPillarPrefab;
    public GameObject[] enemyPrefabs;
    public GameObject healthPackPrefab;

    [Header("Settings")]
    public float timeBetweenWaves = 3f;
    public float objectSpacing = 1.5f; // Distance between objects in a wave
    
    [Tooltip("Set this to the Y position of your Floor")]
    public float minY = -4.5f;         
    [Tooltip("Set this to the Y position of your Ceiling")]
    public float maxY = 4.5f;          

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Pick a random pattern
            int patternType = Random.Range(0, 3);

            switch (patternType)
            {
                case 0: yield return StartCoroutine(SpawnCorridorWave()); break;
                case 1: yield return StartCoroutine(SpawnSlalomWave()); break;
                case 2: yield return StartCoroutine(SpawnEnemyAmbush()); break;
            }

            yield return new WaitForSeconds(timeBetweenWaves / (GameManager.GlobalSpeed / 5f));
        }
    }

    // PATTERN 1: A series of gates that stay at the same height
    IEnumerator SpawnCorridorWave()
    {
        // Buffer the range by 2 so the hole doesn't spawn partially inside the floor/ceiling
        float targetY = Random.Range(minY + 2f, maxY - 2f);
        int count = Random.Range(3, 6);

        for (int i = 0; i < count; i++)
        {
            Instantiate(gatePrefab, new Vector3(transform.position.x, targetY, 0), Quaternion.identity);
            yield return new WaitForSeconds(objectSpacing / GameManager.GlobalSpeed);
        }
    }

    // PATTERN 2: Alternating Top and Bottom pillars (slalom style)
    IEnumerator SpawnSlalomWave()
    {
        int count = Random.Range(4, 8);
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = (i % 2 == 0) ? ceilingPillarPrefab : floorPillarPrefab;
            
            // USE THE MIN AND MAX Y VARIABLES HERE
            float yPos = (i % 2 == 0) ? maxY : minY;

            Instantiate(prefab, new Vector3(transform.position.x, yPos, 0), Quaternion.identity);
            yield return new WaitForSeconds(objectSpacing / GameManager.GlobalSpeed);
        }
    }

    // PATTERN 3: Enemies mixed with a Health Pack
    IEnumerator SpawnEnemyAmbush()
    {
        int count = Random.Range(3, 5);
        for (int i = 0; i < count; i++)
        {
            // Buffer by 1 so enemies don't spawn half-inside the walls
            float y = Random.Range(minY + 1f, maxY - 1f);
            if (enemyPrefabs != null && enemyPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                GameObject randomEnemy = enemyPrefabs[randomIndex];

                Instantiate(randomEnemy, new Vector3(transform.position.x, y, 0), Quaternion.identity);
            }
            
            // 20% chance to sneak a health pack in the middle
            if (Random.value < 0.2f) 
            {
                float healthY = Random.Range(minY + 1f, maxY - 1f);
                Instantiate(healthPackPrefab, new Vector3(transform.position.x + 2f, healthY, 0), Quaternion.identity);
            }

            yield return new WaitForSeconds(objectSpacing / GameManager.GlobalSpeed);
        }
    }
}