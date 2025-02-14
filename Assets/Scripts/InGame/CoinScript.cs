using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
int PlayerLayer;
    const string stagPlayer = "Player";
    private void Start()
    {
        PlayerLayer = LayerMask.NameToLayer(stagPlayer);
    }
    //==================================================================================

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(PlayerLayer))
        {
            Vector3 playerPosition = other.transform.position;
            EventManager.Instance.coinTrigger(playerPosition);
            ObjectPool.Instance.ReturnCoinToPool(this.gameObject);

        }
    }
}

