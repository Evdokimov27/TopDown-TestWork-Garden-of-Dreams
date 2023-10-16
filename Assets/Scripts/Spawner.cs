using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // ������ �������� ��������
    public Transform player; // ������ �� ������
    public int maxMonsters = 5; // ������������ ���������� ��������
    public Transform[] spawnPoints; // ������ ����� ������
    public bool nearPlayer = false; // true - �� ����� ��������� ����� � �������
    public float minSpawnDistance = 10f; // ����������� ���������� �� ������ ��� ������ ��������


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
