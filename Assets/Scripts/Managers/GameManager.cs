using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int coin = 0;
    public int score = 0;
    public float EndGameTime = 0;
    public int lives = 3;
    private bool isGameOver = false;

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
        EventManager.Instance.onEndgameController += Endgame;


    }
    //=========================================================================
    void OnDisable()
    {
        EventManager.Instance.onCoinCollect -= coinCollected;
        EventManager.Instance.onEndgameController -= Endgame;


    }
    //=========================================================================
    void Start()
    {
        EventManager.Instance.SetState(EventManager.GameState.Initial);
        EventManager.Instance.SetState(EventManager.GameState.GameContinue);
    }

    private void Update()
    {

        EndGameTime += Time.deltaTime;

        if (EndGameTime > 2)
        {
            EventManager.Instance.EndGame(false, lives);
            EndGameTime = 0;
            lives--;
        }
        if (lives <= 0 && !isGameOver)
        {
            isGameOver = true;
            EventManager.Instance.SetState(EventManager.GameState.LevelFailed);
            lives = 3;
            coin = 0;

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
            lives++;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
            EventManager.Instance.SetState(EventManager.GameState.LevelComplete);
            
        }
        else
        {
            coin = 0;
            lives=3;
            EventManager.Instance.SetState(EventManager.GameState.LevelFailed);
        }

    }
    //==========================================================================
    //=========================================================================


}
