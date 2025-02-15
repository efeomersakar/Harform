using System;
using System.Collections;
using System.Collections.Generic;
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
            EventManager.Instance.EndGame(false);
            EndGameTime = 0;
            lives--;
            
        }
        if (lives <= 0 && !isGameOver)
        {
            isGameOver = true;
            EventManager.Instance.SetState(EventManager.GameState.End);
            Debug.Log("END");
        }

    }
    //====================================================================

    //=========================================================================
    private void coinCollected(Vector3 playerPosition)
    {
        coin++;
    }
    //==========================================================================
    public void Endgame(bool isWin)
    {
        StartCoroutine(EndgameRoutine(isWin));
    }
    //==========================================================================
    private IEnumerator EndgameRoutine(bool isWin)
    {
        yield return new WaitForSeconds(5f);

        if (isWin)
        {
            yield return new WaitForSeconds(3f);
            level++;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
            yield return new WaitForSeconds(1f);
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
