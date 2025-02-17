using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class EventManager : MonoBehaviour
{
    public event Action OnMainMenu;
    public event Action OnGameLoading;
    public event Action OnInitial;
    public event Action OnGameContinue;
    public event Action OnPause;
    public event Action OnPLayerStateChange;
    public event Action OnLevelFailed;
    public event Action OnLevelCompleted;
    public event Action OnEnemyAttacked;
    public event Action OnPlayerStartPosition;
    public event Action OnPlayerKilled;
    public event Action  OnPlayerGotDamaged;

    //=======================================================================
    public delegate void rewardCollected(Vector3 SpawnPosition);
    public delegate void coinCollected(Vector3 PlayerPosition);
    public delegate void EndGameController(bool isWin, int coin);
    public delegate void PlayerStateChange(PlayerState newState);

    //==================================================================================
    public event rewardCollected onRewardBoxTouched;
    public event coinCollected onCoinCollect;
    public event EndGameController onEndgameController;
    public event PlayerStateChange onPlayerStateChange;

    //==================================================================================
    public GameState currentState;
    public PlayerState PlayerCurrentState;
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

    public void coinTrigger(Vector3 PlayerPosition)
    {
        onCoinCollect?.Invoke(PlayerPosition);
    }
    //==================================================================================

    public void EndGame(bool isWin, int coin)
    {
        onEndgameController?.Invoke(isWin, coin);
    }
    //==================================================================================
    public void EnemyAttacked()
    {
        OnEnemyAttacked?.Invoke();
    }
    //==================================================================================
    public void SetState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.MainMenu:
                OnMainMenu?.Invoke();
                break;
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
            case GameState.LevelFailed:
                OnLevelFailed?.Invoke();
                SceneManager.LoadScene("DefeatScene");
                break;
            case GameState.LevelComplete:
                OnLevelCompleted?.Invoke();
                break;
        }
    }
    //==================================================================================
    public void SetPlayerState(PlayerState newPlayerState)
    {
        PlayerCurrentState = newPlayerState;
        onPlayerStateChange?.Invoke(PlayerCurrentState);


        switch (PlayerCurrentState)
        {

            case PlayerState.PlayerStartPosition:
                OnPlayerStartPosition?.Invoke();
                break;
            case PlayerState.PlayerGotKilled:
                OnPlayerKilled?.Invoke();
                break;
                case PlayerState.PlayerGotDamaged:
                OnPlayerGotDamaged?.Invoke();
                break;

        }
    }

    //==================================================================================
    public enum GameState
    {
        MainMenu,
        GameLoading,
        Initial,
        GameContinue,
        PauseLevel,
        LevelFailed,
        LevelComplete
    }
    public enum PlayerState
    {
        PlayerStartPosition,
        PlayerGotDamaged,
        PlayerGotKilled

    }
}
