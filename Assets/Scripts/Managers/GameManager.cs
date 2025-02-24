using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int coin = 0;
    public float EndGameTime = 30f;
    public int lives = 3;
    private int minimumCoin = 1;
    private bool isEnemyHit = false;
    private bool isGameContinue = false;
    public static GameManager Instance

    {
        get;
        private set;

    }
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
    }
    //=========================================================================
    void OnEnable()
    {
        EventManager.Instance.onCoinCollect += coinCollected;
        EventManager.Instance.OnEnemyAttacked += EnemyHit;
        EventManager.Instance.onEndgameController += LevelComplete;
        EventManager.Instance.OnLevelFailed += LevelFailed;



    }
    //=========================================================================
    void OnDisable()
    {
        EventManager.Instance.onCoinCollect -= coinCollected;
        EventManager.Instance.OnEnemyAttacked -= EnemyHit;
        EventManager.Instance.onEndgameController -= LevelComplete;
        EventManager.Instance.OnLevelFailed -= LevelFailed;


    }
    //=========================================================================
    void Start()
    {
        EventManager.Instance.SetState(EventManager.GameState.MainMenu);
        EventManager.Instance.SetState(EventManager.GameState.Initial);
    }

    private void Update()
    {
        if (EventManager.Instance.currentState == EventManager.GameState.GameContinue)
        {
            isGameContinue = true;
            EndGameTime -= Time.deltaTime;
        }

        if (EndGameTime < 0 || isEnemyHit)
        {
            if (lives > 0)
            {
                lives--;
                EndGameTime = 30f;
                isEnemyHit = false;
                EventManager.Instance.SetPlayerState(EventManager.PlayerState.PlayerGotDamaged);
            }

            if (lives == 0 && isGameContinue)
            {
                
                EventManager.Instance.EndGame(false, lives);
                EventManager.Instance.SetPlayerState(EventManager.PlayerState.PlayerGotKilled);
                DOVirtual.DelayedCall(1f, () =>
    {
        EventManager.Instance.SetState(EventManager.GameState.LevelFailed);
    });
            isGameContinue=false;
            }

        }

    }
    //==========================================================================
    public void LevelComplete(bool isWin, int coin)
    {
        if (isWin && (level == 3 || coin >= minimumCoin))
        {
            level++;
            minimumCoin++;
            EventManager.Instance.SetState(EventManager.GameState.LevelComplete);
            DOVirtual.DelayedCall(1.2f, () =>
    {
        EventManager.Instance.SetState(EventManager.GameState.GameContinue);

    });
        }
        else
        {
            Debug.Log("Yetersiz Bakiye");
        }

    }
    //====================================================================
    private void coinCollected(Vector3 playerPosition)
    {
        coin++;
    }

    //==========================================================================
    private void EnemyHit()
    {
        isEnemyHit = true;
    }
    //=========================================================================
    private void LevelFailed()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DefeatScene"); //yeni sahne çağırma metodu
        //Debug.Log("KAÇ DEFA ÇAĞIRDI");
    }

}
