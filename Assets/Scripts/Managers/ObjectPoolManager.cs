using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField] private List<GameObject> level1Prefabs;
    [SerializeField] private List<GameObject> level2Prefabs;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private List<GameObject> activePrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializePool(int level)
    {
        ClearPool(); // Önce eski prefabları temizle

        activePrefabs = (level == 1) ? level1Prefabs : level2Prefabs;

        foreach (var prefab in activePrefabs)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < 10; i++) // 10 adet nesne oluştur
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            poolDictionary[prefab.name] = objectQueue;
        }
    }

    public GameObject GetFromPool(string prefabName)
    {
        if (poolDictionary.ContainsKey(prefabName) && poolDictionary[prefabName].Count > 0)
        {
            GameObject obj = poolDictionary[prefabName].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(activePrefabs.Find(p => p.name == prefabName));
        return newObj;
    }

    public void ReturnToPool(string prefabName, GameObject obj)
    {
        obj.SetActive(false);
        if (!poolDictionary.ContainsKey(prefabName))
        {
            poolDictionary[prefabName] = new Queue<GameObject>();
        }
        poolDictionary[prefabName].Enqueue(obj);
    }

    private void ClearPool()
    {
        foreach (var queue in poolDictionary.Values)
        {
            while (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                Destroy(obj);
            }
        }
        poolDictionary.Clear();
    }
}
