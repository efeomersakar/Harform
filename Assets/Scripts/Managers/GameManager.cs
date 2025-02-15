using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int coin = 0;
    public int score = 0;
    public float EndGameTime = 0;
    public int lives = 3;
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
        EventManager.Instance.onEnemyAttacking += EnemyAttack;

    }
    //=========================================================================
    void OnDisable()
    {
        EventManager.Instance.onCoinCollect -= coinCollected;
        EventManager.Instance.onEndgameController -= Endgame;
        EventManager.Instance.onEnemyAttacking -= EnemyAttack;


    }
    //=========================================================================

    private void Update()
    {
        EndGameTime += Time.deltaTime;
        if (EndGameTime > 2)
        {
            EventManager.Instance.EndGame(false, 0);
            EndGameTime = 0;
            lives--;
        }
        //====================================================================
        if (lives <= 0)
        {
            EventManager.Instance.SetState(EventManager.GameState.End);
        }
    }
    //=========================================================================
    private void coinCollected(Vector3 playerPosition)
    {
        coin++;
    }
    //==========================================================================
    public void Endgame(bool isWin, int score)
    {
        StartCoroutine(EndgameRoutine(isWin, score));
    }
    //==========================================================================
    private IEnumerator EndgameRoutine(bool isWin, int score)
    {
        yield return new WaitForSeconds(5f);

        if (isWin)
        {
            EventManager.Instance.SetState(EventManager.GameState.GameLoading);

            yield return new WaitForSeconds(3f);
            level++;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
            yield return new WaitForSeconds(1f);
            EventManager.Instance.SetState(EventManager.GameState.GameContinue);
            this.score = 100 + (coin * 10);
        }
        else
        {
            EventManager.Instance.SetState(EventManager.GameState.End);
            coin = 0;
        }
    }

    //=========================================================================
    private void EnemyAttack()
    {
        lives--;
    }


}
