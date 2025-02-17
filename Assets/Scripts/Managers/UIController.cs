using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;
using System.Linq;
using Palmmedia.ReportGenerator.Core;

public class UIController : MonoBehaviour
{

    // Button
    private Button playButton;
    private Button loadButton;
    private Button optionsButton;
    private Button exitButton;
    private Button MainMenuButton;
    private Button TryAgainButton;
    private Button PauseButton;
    private Button ContinueButton;


    // TEXT
    [SerializeField] private TextMeshProUGUI YouLoseText;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI LivesText;
    [SerializeField] private TextMeshProUGUI CoinText;
    [SerializeField] private TextMeshProUGUI EndGameText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    //PANEL
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject LevelCompletePanel;

    public static UIController Instance { get; private set; }

    //==================================================================================

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //==================================================================================

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (EventManager.Instance != null)
        {
            EventManager.Instance.onCoinCollect += coinTextEvent;
        }
    }

    //==================================================================================

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (EventManager.Instance != null)
        {
            EventManager.Instance.onCoinCollect -= coinTextEvent;
        }
    }

    //==================================================================================

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.SetState(EventManager.GameState.GameContinue);
        }
    }

    //==================================================================================

    void Update()
    {
        maintext();
    }

    //==================================================================================

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedUIElements());
    }

    //==================================================================================

    private IEnumerator DelayedUIElements()
    {
        yield return null;
        UIElements();
    }

    //==================================================================================

    private void UIElements()
    {
        CoinText = null;
        levelText = null;
        LivesText = null;
        TimerText = null;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            playButton = FindButton("PlayButton");
            loadButton = FindButton("LoadButton");
            optionsButton = FindButton("OptionsButton");
            exitButton = FindButton("ExitButton");

            if (playButton != null)
            {
                playButton.onClick.AddListener(PlayGame);
            }

            if (loadButton != null)
            {
                loadButton.onClick.AddListener(LoadGame);
            }

            if (optionsButton != null)
            {
                optionsButton.onClick.AddListener(OpenOptions);
            }

            if (exitButton != null)
            {
                exitButton.onClick.AddListener(ExitGame);
            }
        }

        if (SceneManager.GetActiveScene().name == "DefeatScene")
        {
            YouLoseText = FindText("YouLoseText");
            TryAgainButton = FindButton("TryAgainButton");
            MainMenuButton = FindButton("MainMenuButton");
            exitButton = FindButton("ExitButton");

            if (TryAgainButton != null)
            {
                TryAgainButton.onClick.AddListener(TryAgain);
            }

            if (MainMenuButton != null)
            {
                MainMenuButton.onClick.AddListener(GoToMainMenu);
            }

            if (exitButton != null)
            {
                exitButton.onClick.AddListener(ExitGame);
            }

            YouLoseTextAnimation();
        }

        if (SceneManager.GetActiveScene().name == "Level" + GameManager.Instance.level && EventManager.Instance.currentState == EventManager.GameState.GameContinue)
        {
            CoinText = FindText("CoinText");
            levelText = FindText("LevelText");
            LivesText = FindText("Lives");
            TimerText = FindText("Timer");
            PauseButton = FindButton("PauseButton");
            ContinueButton = FindButton("ContinueButton");
            MainMenuButton = FindButton("MainMenuButton");
            exitButton = FindButton("ExitButton");

            if (PauseButton != null)
            {
                PauseButton.onClick.AddListener(PauseButtonClicked);
            }

            SettingsPanel = GameObject.Find("SettingsPanel");
            SettingsPanel.SetActive(false);

            if (MainMenuButton != null)
            {
                MainMenuButton.onClick.AddListener(GoToMainMenu);
            }

            if (exitButton != null)
            {
                exitButton.onClick.AddListener(ExitGame);
            }
            if (ContinueButton != null)
            {
                ContinueButton.onClick.AddListener(PauseButtonClicked);
            }
            // LevelCompletePanel = GameObject.Find("LevelCompletePanel");

            // if (EventManager.Instance.currentState == EventManager.GameState.LevelComplete)
            // {
            //     LevelCompletePanel.SetActive(true);
            // }
            //  else
            // {
            //      LevelCompletePanel.SetActive(false);
            // }
            

        }

    }

    //==================================================================================

    private TextMeshProUGUI FindText(string name)
    {
        GameObject obj = GameObject.Find(name);
        return obj != null ? obj.GetComponent<TextMeshProUGUI>() : null;
    }

    private Button FindButton(string name)
    {
        GameObject obj = GameObject.Find(name);
        return obj != null ? obj.GetComponent<Button>() : null;
    }


    //==================================================================================

    private void PlayGame()
    {
        SceneManager.LoadScene("Level1");
        EventManager.Instance.SetState(EventManager.GameState.GameContinue);
    }

    //==================================================================================

    private void LoadGame()
    {
        Debug.Log("Load Button Clicked! (Şu an boş)");
    }

    //==================================================================================

    private void OpenOptions()
    {
        Debug.Log("Options Button Clicked! (Şu an boş)");
    }

    //==================================================================================

    private void ExitGame()
    {
        Application.Quit();
    }

    //==================================================================================

    private void TryAgain()
    {
        if (SceneManager.GetActiveScene().name == "DefeatScene")
        {
            GameManager.Instance.lives = 3;
            GameManager.Instance.coin = 0;
            GameManager.Instance.EndGameTime = 30f;
            SceneManager.LoadScene("Level" + GameManager.Instance.level);
            EventManager.Instance.SetState(EventManager.GameState.GameContinue);
        }
    }

    //==================================================================================

    private void GoToMainMenu()
    {
        SettingsPanel.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
        EventManager.Instance.SetState(EventManager.GameState.Initial);
    }

    //==================================================================================

    private void PauseButtonClicked()
    {
        if (EventManager.Instance.currentState == EventManager.GameState.PauseLevel)
        {

            EventManager.Instance.SetState(EventManager.GameState.GameContinue);

            SettingsPanel.gameObject.SetActive(false);

        }
        else
        {

            EventManager.Instance.SetState(EventManager.GameState.PauseLevel);
            PauseButton.gameObject.SetActive(false);
            SettingsPanel.gameObject.SetActive(true);

        }
    }

    //==================================================================================


    private void coinTextEvent(Vector3 PlayerPosition)
    {
        StartCoroutine(coinNum());
    }

    //==================================================================================

    private IEnumerator coinNum()
    {
        yield return new WaitForSeconds(0.1f);
        if (CoinText != null)
        {
            CoinText.text = "COIN: " + GameManager.Instance.coin;
        }
    }

    //==================================================================================

    private void maintext()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (levelText != null)
        {
            levelText.text = "LEVEL " + GameManager.Instance.level;
        }

        if (LivesText != null)
        {
            LivesText.text = "LIVES: " + GameManager.Instance.lives;
        }

        if (TimerText != null)
        {
            TimerText.text = "TIMER: " + Mathf.FloorToInt(GameManager.Instance.EndGameTime);
        }
    }

    //==================================================================================

    private void YouLoseTextAnimation()
    {
        if (YouLoseText != null)
        {
            DOTween.Init();
            YouLoseText.transform.DOScale(Vector3.one * 1.5f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            YouLoseText.DOColor(new Color(1f, 0f, 0f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }
    }

    //==================================================================================
}
