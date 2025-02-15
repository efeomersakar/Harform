using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTouchScript : MonoBehaviour
{
    private int playerLayer;
    const string stagPlayer = "Player";

    private void OnEnable()
    {
        EventManager.Instance.onEndgameController += GameEnd;
    }

    private void OnDisable()
    {
        EventManager.Instance.onEndgameController -= GameEnd;
    }

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer(stagPlayer);
    }
    //==================================================================================

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == playerLayer)
        {
            EventManager.Instance.EndGame(true);
            GameEnd(true);

        }

    }
    //==================================================================================
    private void GameEnd(bool iswin)
    {
        Debug.Log("Bölüm Bitti!");
    }
}
