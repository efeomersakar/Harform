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

// coin.transform.position = spawnPosition;
// coin.SetActive(true);

// coin.transform.DOJump(spawnPosition + Vector3.up * 1.3f, 1f, 0, 0.3f)
//     .OnComplete(() =>
//     {
//         Vector3 PositionAfterJump = coin.transform.position;
//         Vector3 secondPosition = PositionAfterJump + new Vector3(1.6f, 0f, 0f);
//         coin.transform.DOMove(secondPosition, 0.3f)
//         .SetEase(Ease.OutQuad)
//         .OnComplete(() =>
//          {
//              Vector3 finalPosition = secondPosition + new Vector3(0f, -3.5f, 0f);
//              coin.transform.DOMove(finalPosition, 0.4f)
//              .SetEase(Ease.OutQuad);
//          });

//     });