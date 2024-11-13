using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject[] enemies;
    private GameObject enemyToSpawn;
    private const float lowerTimeLimit = 7f;
    private const float upperTimeLimit = 13f;
    private const float offsetFromPlayerX = 8f;

    //start at upper limit to give player time to get oriented
    private float timeUntilSpawn = upperTimeLimit;
    private float timer = 0;

    public bool spawning { get; set; } = false;
    private bool spawnStartHelper = true;
    public bool overrideSpawner { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.tutorialRunning && spawnStartHelper)
        {
            spawning = true;
            spawnStartHelper = false;
        }

        if (enemyToSpawn == null && !overrideSpawner && !GameManager.instance.tutorialRunning) spawning = true;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (!spawning) return;

        timer += Time.deltaTime;

        if (timer < timeUntilSpawn) return;

        int enemyIndex = Random.Range(0, enemies.Length);
        enemyToSpawn = Instantiate(enemies[enemyIndex]);
        if (enemyToSpawn.GetComponent<EnemyMove>().movingType)
        {
            enemyToSpawn.transform.position = new Vector2(playerController.transform.position.x + offsetFromPlayerX, enemyToSpawn.GetComponent<EnemyMove>().spawnY);
        }
        else
        {
            enemyToSpawn.transform.position = playerController.transform.position;
        }

        timeUntilSpawn = Random.Range(lowerTimeLimit, upperTimeLimit);

        timer = 0f;
        spawning = false;
    }
}
