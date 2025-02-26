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
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    #region Keyboard Controls
    private void HandleKeyboardInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        keyboardInput = new Vector3(moveX, moveY, 0);
    }
    #endregion

    #region Touch Controls
    private void HandleTouchInput()
    {
        Vector2 touchPosition = Vector2.zero;
        bool isInputActive = false;

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            touchPosition = Input.mousePosition;
            isInputActive = true;
        }
#else
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        touchPosition = touch.position;
        isInputActive = touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved;

        if (touch.phase == TouchPhase.Ended)
        {
            inputDirection = Vector2.zero;
            Debug.Log("kullanıcı dokunmayı nıraktı");
            return;
        }
 
    }
#endif

        if (isInputActive)
        {
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
