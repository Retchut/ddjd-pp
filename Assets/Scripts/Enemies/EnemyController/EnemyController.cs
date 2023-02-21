using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float spawnRate = 3f;
    [SerializeField]
    private float xBound = 15f;
    [SerializeField]
    private float ySpawnPos = 0f;
    [SerializeField]
    private float zBound = 15f;
    [SerializeField]
    private GameObject[] enemyPrefabs;

    private List<GameObject> spawnedEnemies;

    // Initializes list of spawned enemies and starts the spawning process
    void Start()
    {
        spawnedEnemies = new List<GameObject>();
        StartCoroutine(Spawner());
    }

    // Periodically spawns an enemy
    IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            Vector3 spawnPos = new Vector3(Random.Range(-xBound, xBound), ySpawnPos, Random.Range(-zBound, zBound));
            GameObject enemyToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject spawned = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
            spawned.transform.parent = gameObject.transform;
            spawned.GetComponent<Drone>().SetEnemyController(GetComponent<EnemyController>());
            spawnedEnemies.Add(spawned);
        }
    }

    // Removes an enemy from the game
    public void RemoveEnemy(GameObject toRemove)
    {
        spawnedEnemies.Remove(toRemove);
        Destroy(toRemove);
    }
}
