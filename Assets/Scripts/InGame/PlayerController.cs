using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector2 touchStartPos;
    private bool isTouchActive = false;
    private Vector2 inputDirection;
    private Vector2 keyboardInput;
    public BoxCollider PlayerBox;
    float safeAreaLimit;

    void OnEnable()
    {
        EventManager.Instance.OnPlayerGotDamaged += PlayerStartPosition;
        EventManager.Instance.OnPlayerKilled += PlayDeathAnimation;
        EventManager.Instance.OnInitial += GameInitial;

    }

    void OnDisable()
    {
        EventManager.Instance.OnPlayerGotDamaged -= PlayerStartPosition;
        EventManager.Instance.OnPlayerKilled -= PlayDeathAnimation;
        EventManager.Instance.OnInitial -= GameInitial;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        safeAreaLimit = Screen.safeArea.y + Screen.safeArea.height * 0.75f;
    }

    void Update()
    {
        if (EventManager.Instance.currentState == EventManager.GameState.GameContinue)
        {
            HandleKeyboardInput();
            HandleTouchInput();
            HandleMovement();
            PlayerBox.enabled = true;
        }
        if (GameManager.Instance.lives == 0)
        {
            PlayerBox.enabled = false;
        }
    }

    private void GameInitial()
    {
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
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.y > safeAreaLimit) return;
            touchStartPos = Input.mousePosition;
            isInputActive = true;
        }
        else if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.y > safeAreaLimit) return;
            touchPosition = Input.mousePosition;
            isInputActive = true;
        }
#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.y > safeAreaLimit) return;

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position; 
                isInputActive = true;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchPosition = touch.position;
                isInputActive = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                inputDirection = Vector2.zero;
                isInputActive = false;
                return;
            }
        }
#endif

        if (isInputActive)
        {
            Vector2 delta = (touchPosition - touchStartPos).normalized;
            inputDirection = delta;
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