using System.Collections.Generic;
using UnityEngine;

public class DinoSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    public MiniGame3Dino miniGame3Dino;
    public GameObject spawnPoint;

    public SpawnableObject[] objects;

    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    [Header("----- DEBUG ------")]
    public List<DinoObstacle> obstacles;
    public float currentTime = 0f;
    public float timeStep = 0f;

    private void OnEnable()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
        timeStep = 1.0f / miniGame3Dino.gameSpeed;

        Spawn();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        float spawnChance = Random.value;

        foreach (var obj in objects)
        {
            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab, spawnPoint.transform);
                DinoObstacle dinoObstacle = obstacle.GetComponent<DinoObstacle>();
                dinoObstacle.spawner = this;
                obstacles.Add(obstacle.GetComponent<DinoObstacle>());
                break;
            }

            spawnChance -= obj.spawnChance;
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void Update()
    {
        float speed = miniGame3Dino.gameSpeed / transform.localScale.x;
        currentTime += Time.deltaTime;
        if (currentTime >= timeStep)
        {
            currentTime = 0f;
            foreach (DinoObstacle obstacle in obstacles)
            {
                obstacle.UpdateObstacle(speed);
            }
        }
    }

    public void DestroyObstacle(DinoObstacle obstacle)
    {
        miniGame3Dino.IncrementScore();
        obstacles.Remove(obstacle);
        Destroy(obstacle.gameObject);
    }
}

