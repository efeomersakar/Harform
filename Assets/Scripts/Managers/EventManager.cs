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
    //==================================================================================
    public event rewardCollected onRewardBoxTouched;
    public event PlayerStateChange onPlayerStateChange;
    public event coinCollected onCoinCollect;
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
