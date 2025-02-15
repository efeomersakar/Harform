using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Numerics;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody rb;
    private UnityEngine.Vector3 startPosition;



    void OnEnable()
    {
        EventManager.Instance.OnPlayerStartPosition += PlayerStartPosition;

    }

    void OnDisable()
    {
        EventManager.Instance.OnPlayerStartPosition -= PlayerStartPosition;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }
    //=====================================================================
    void Update()
    {
        if (EventManager.Instance.currentState == EventManager.GameState.GameContinue)
        {
            HandleMovement();
        }

    }


    //==================================================================================

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        UnityEngine.Vector3 movement = new UnityEngine.Vector3(moveX, moveY, 0);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
    //=========================================================
    private void PlayerStartPosition()
    {
        transform.position = startPosition;
    }

}
