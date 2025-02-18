using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTouch : MonoBehaviour
{
    private int playerLayer;
    const string stagPlayer = "Player";



    private void Start()
    {
        playerLayer = LayerMask.NameToLayer(stagPlayer);
    }
    //==================================================================================

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == playerLayer)
        {

            GameManager.Instance.LevelComplete(true, GameManager.Instance.coin);

        }
    
    }
    //==================================================================================


}
