using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject CoinPrefab;
    [SerializeField] private int poolSize = 1;

    private Queue<GameObject> coinQueue;

    public static ObjectPool Instance { get; private set; }

    //==================================================================================
    void Awake()
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

        coinQueue = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {

            GameObject coin = Instantiate(CoinPrefab);
            coin.SetActive(false);
            coinQueue.Enqueue(coin);
        }
    }

    private void OnEnable()
    {

        EventManager.Instance.onRewardBoxTouched += SpawnCoin;

    }

    private void OnDisable()
    {
       
            EventManager.Instance.onRewardBoxTouched -= SpawnCoin;
        
    }

    private void SpawnCoin(Vector3 spawnPosition)
    {
        GameObject coin;

        if (coinQueue.Count > 0)
        {
            coin = coinQueue.Dequeue();
        }
        else
        {
            coin = Instantiate(CoinPrefab);
        }

        coin.transform.position = spawnPosition;
        coin.SetActive(true);

        coin.transform.DOJump(spawnPosition + Vector3.up * 1.3f, 1f, 0, 0.3f)
            .OnComplete(() =>
            {
                Vector3 PositionAfterJump = coin.transform.position;
                Vector3 secondPosition = PositionAfterJump + new Vector3(1.6f, 0f, 0f);
                coin.transform.DOMove(secondPosition, 0.3f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                 {
                     Vector3 finalPosition = secondPosition + new Vector3(0f, -3.5f, 0f);
                     coin.transform.DOMove(finalPosition, 0.4f)
                     .SetEase(Ease.OutQuad);
                 });

            });
    }

 public void ReturnCoinToPool(GameObject coin)
    {
        coin.SetActive(false);
        coinQueue.Enqueue(coin);
    }
}
