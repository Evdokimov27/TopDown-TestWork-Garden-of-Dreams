using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // Массив префабов монстров
    public Transform player; // Ссылка на игрока
    public int maxMonsters = 5; // Максимальное количество монстров
    public Transform[] spawnPoints; // Массив точек спавна
    public bool nearPlayer = false; // true - не могут спавнится рядом с игроком
    public float minSpawnDistance = 10f; // Минимальное расстояние от игрока для спавна монстров


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Vector3 randomSpawnPosition = randomSpawnPoint.position;

            if (nearPlayer)
            {
                while (Vector3.Distance(randomSpawnPosition, player.position) < minSpawnDistance)
                {
                    randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    randomSpawnPosition = randomSpawnPoint.position;
                }
            }
            GameObject randomMonsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            Instantiate(randomMonsterPrefab, randomSpawnPosition, Quaternion.identity);
        }
    }
}
