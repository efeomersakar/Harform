using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector2 inputDirection;
    private Vector2 keyboardInput;

    void OnEnable()
    {
        EventManager.Instance.OnPlayerGotDamaged += PlayerStartPosition;
        EventManager.Instance.OnPlayerKilled += PlayDeathAnimation;
    }

    void OnDisable()
    {
        EventManager.Instance.OnPlayerGotDamaged -= PlayerStartPosition;
        EventManager.Instance.OnPlayerKilled -= PlayDeathAnimation;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (EventManager.Instance.currentState == EventManager.GameState.GameContinue)
        {
            HandleKeyboardInput();
            HandleTouchInput();
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        Vector2 finalInput = inputDirection != Vector2.zero ? inputDirection : keyboardInput;
        Vector3 movement = new Vector3(finalInput.x, finalInput.y, 0);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    #region Keyboard Controls
    private void HandleKeyboardInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        keyboardInput = new Vector2(moveX, moveY);
    }
    #endregion

    #region Touch Controls
    private void HandleTouchInput()
    {
        #if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPosition = Input.mousePosition;
        #else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;
        #endif

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            if (touchPosition.x < screenWidth / 3)
                inputDirection = Vector2.left;
            else if (touchPosition.x > 2 * screenWidth / 3)
                inputDirection = Vector2.right;
            else if (touchPosition.y > 2 * screenHeight / 3)
                inputDirection = Vector2.up;
            else if (touchPosition.y < screenHeight / 3)
                inputDirection = Vector2.down;
        }
        else
        {
            inputDirection = Vector2.zero;
        }
    }
    #endregion

    private void PlayerStartPosition()
    {
        if (GameManager.Instance.lives >= 1)
        {
            transform.position = startPosition;
        }
    }

    private void PlayDeathAnimation()
    {
        Sequence deathAnimation = DOTween.Sequence();
        deathAnimation.Append(transform.DOScale(2f, 0.9f).OnStart(() => 
            GetComponent<Renderer>().material.DOColor(Color.red, 0.1f)));
        deathAnimation.Append(transform.DOScale(0f, 0.1f));
        deathAnimation.AppendCallback(() => Destroy(gameObject));
    }
}
