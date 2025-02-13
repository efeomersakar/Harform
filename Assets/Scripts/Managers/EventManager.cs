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
    public delegate void rewardCollected();
    public delegate void PlayerStateChange(PlayerController.PlayerState newState);
    public event rewardCollected onRewardCollected;
    public event PlayerStateChange onPlayerStateChange;
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
    public void RewardBoxTrigger()
    {
        onRewardCollected?.Invoke();
    }
    //==================================================================================

    public void PlayerStateChangeEvent(PlayerController.PlayerState newState)
    {
        onPlayerStateChange?.Invoke(newState);
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
