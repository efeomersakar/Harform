using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int level = 0;
    public int coin = 0;
    public int score = 0;
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
        EventManager.Instance.onRewardBoxTouched += RewardCollect;
    }
    //=========================================================================
    void OnDisable()
    {
        EventManager.Instance.onRewardBoxTouched -= RewardCollect;

    }

 

    //=========================================================================

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

       private void RewardCollect(Vector3 spawnPosition)
    {
        Debug.Log("Ödül Alındı");
    }
}
