using UnityEngine;

public class RandomPositionEnemies : MonoBehaviour
{
    public int numberOfEnemies = 4;

    public GameObject enemyPrefab;
    public GameObject enemy2Prefab;

    void Start()
    {
        GameManager.Instance.SetSeed();
        Debug.Log("SEED:" + GameManager.Instance.seed);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = GetRandomPosition();

            if (i % 2 == 0)
                InstantiateEnemy(enemyPrefab, randomPosition);
            else
                InstantiateEnemy(enemy2Prefab, randomPosition);
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-10f, -27f);
        float y = Random.Range(-25f, -28f);
        float z = 10f;
        return new Vector3(x, y, z);
    }

    void InstantiateEnemy(GameObject enemyPrefab, Vector3 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.name = enemyPrefab.name;
    }
}
