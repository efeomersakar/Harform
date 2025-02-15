using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventManager : MonoBehaviour
{
    public event Action OnGameLoading;
    public event Action OnInitial;
    public event Action OnGameContinue;
    public event Action OnPause;
    public event Action OnEnd;
    //=======================================================================
    public delegate void rewardCollected(Vector3 SpawnPosition);
    public delegate void PlayerStateChange(PlayerController.PlayerState newState);
    public delegate void coinCollected(Vector3 PlayerPosition);
    public delegate void EndGameController(bool isWin, int score);
    public delegate void EnemyAttack();


    //==================================================================================
    public event rewardCollected onRewardBoxTouched;
    public event PlayerStateChange onPlayerStateChange;

    public event coinCollected onCoinCollect;
    public event EndGameController onEndgameController;
    public event EnemyAttack onEnemyAttacking;

    //==================================================================================
    public GameState currentState;
    public static EventManager Instance
    {
        get;
        private set;
    }
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

    }

    //==================================================================================
    public void RewardBoxTrigger(Vector3 spawnPosition)
    {
        onRewardBoxTouched?.Invoke(spawnPosition);
    }
    //==================================================================================

    public void PlayerStateChangeEvent(PlayerController.PlayerState newState)
    {
        onPlayerStateChange?.Invoke(newState);
    }
    //==================================================================================

    public void coinTrigger(Vector3 PlayerPosition)
    {
        onCoinCollect?.Invoke(PlayerPosition);
    }
    //==================================================================================

    public void EndGame(bool isWin, int score)
    {
        onEndgameController?.Invoke(isWin, score);
    }
    //==================================================================================
    public void EnemyAttacked()
    {
        onEnemyAttacking?.Invoke();
    }

    //==================================================================================
    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.GameLoading:
                OnGameLoading?.Invoke();
                break;
            case GameState.Initial:
                OnInitial?.Invoke();
                break;
            case GameState.GameContinue:
                OnGameContinue?.Invoke();
                break;
            case GameState.PauseLevel:
                OnPause?.Invoke();
                break;
            case GameState.End:
                OnEnd?.Invoke();
                break;
        }
    }
    //==================================================================================
    public enum GameState
    {
        GameLoading,
        Initial,
        GameContinue,
        PauseLevel,
        End
    }

}
