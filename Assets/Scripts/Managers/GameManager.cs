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
    private bool isEnemyHit = false;
    int PlayerLayer;
    const string stagPlayer = "Player";
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
        EventManager.Instance.onEndgameController += Endgame;


    }
    //=========================================================================
    void OnDisable()
    {
        EventManager.Instance.onCoinCollect -= coinCollected;
        EventManager.Instance.OnEnemyAttacked -= EnemyHit;
        EventManager.Instance.onEndgameController -= Endgame;


    }
    //=========================================================================
    void Start()
    {
        PlayerLayer = LayerMask.NameToLayer(stagPlayer);


        EventManager.Instance.SetState(EventManager.GameState.Initial);
        EventManager.Instance.SetState(EventManager.GameState.GameContinue);
    }

    private void Update()
    {
        if (EventManager.Instance.currentState == EventManager.GameState.GameContinue)
        {
            EndGameTime -= Time.deltaTime;
        }

        if (EndGameTime < 0 || isEnemyHit)
        {
            lives--;
            EndGameTime = 30f;
            isEnemyHit = false;
            EventManager.Instance.SetPlayerState(EventManager.PlayerState.PlayerStartPosition);

            if (lives <= 0)
            {
                EventManager.Instance.EndGame(false, lives);
                EventManager.Instance.SetPlayerState(EventManager.PlayerState.PlayerGotDamage);
                DOVirtual.DelayedCall(2f, () =>
    {
        EventManager.Instance.SetState(EventManager.GameState.LevelFailed);
    });

            }
        }

    }
    //====================================================================
    private void coinCollected(Vector3 playerPosition)
    {
        coin++;
    }
    //==========================================================================
    public void Endgame(bool isWin, int lives)
    {
        if (isWin)
        {
            level++;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
            EventManager.Instance.SetState(EventManager.GameState.LevelComplete);
        }

    }
    //==========================================================================
    private void EnemyHit()
    {
        isEnemyHit = true;
    }
    //=========================================================================

}
