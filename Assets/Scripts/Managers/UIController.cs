using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    //Button
    private Button playButton;
    private Button loadButton;
    private Button optionsButton;
    private Button exitButton;
    private Button MainMenuButton;
    private Button TryAgainButton;

    //TEXT
    [SerializeField] private TextMeshProUGUI YouLoseText;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI LivesText;
    [SerializeField] private TextMeshProUGUI CoinText;
    [SerializeField] private TextMeshProUGUI EndGameText;
    [SerializeField] private TextMeshProUGUI ScoreText;


    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (EventManager.Instance != null)
        {
            EventManager.Instance.onCoinCollect += coinTextEvent;
            EventManager.Instance.onEndgameController += LivesandLevelText;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (EventManager.Instance != null)
        {
            EventManager.Instance.onCoinCollect -= coinTextEvent;
            EventManager.Instance.onEndgameController -= LivesandLevelText;
        }
    }

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.SetState(EventManager.GameState.GameContinue);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIElements();
    }

    private void UIElements()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();
            loadButton = GameObject.Find("LoadButton")?.GetComponent<Button>();
            optionsButton = GameObject.Find("OptionsButton")?.GetComponent<Button>();
            exitButton = GameObject.Find("ExitButton")?.GetComponent<Button>();

            if (playButton != null) playButton.onClick.AddListener(PlayGame);
            if (loadButton != null) loadButton.onClick.AddListener(LoadGame);
            if (optionsButton != null) optionsButton.onClick.AddListener(OpenOptions);
            if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
        }

        if (SceneManager.GetActiveScene().name == "DefeatScene")
        {
            YouLoseText = GameObject.Find("YouLoseText")?.GetComponent<TextMeshProUGUI>();
            TryAgainButton = GameObject.Find("TryAgainButton")?.GetComponent<Button>();
            MainMenuButton = GameObject.Find("MainMenuButton")?.GetComponent<Button>();
            exitButton = GameObject.Find("ExitButton")?.GetComponent<Button>();

            if (TryAgainButton != null) TryAgainButton.onClick.AddListener(TryAgain);
            if (MainMenuButton != null) MainMenuButton.onClick.AddListener(GoToMainMenu);
            if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
            YouLoseTextAnimation();
        }

        if (SceneManager.GetActiveScene().name == "Level" + GameManager.Instance.level)
        {
            CoinText = GameObject.Find("CoinText")?.GetComponent<TextMeshProUGUI>();
            levelText = GameObject.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
            LivesText = GameObject.Find("Lives")?.GetComponent<TextMeshProUGUI>();
        }
        else
        {

            playButton = null;
            loadButton = null;
            optionsButton = null;
            exitButton = null;
            YouLoseText = null;
            TryAgainButton = null;
            MainMenuButton = null;
            CoinText = null;
            levelText = null;
            LivesText = null;
        }
    }

    private void PlayGame()
    {
        Debug.Log("Play Button Clicked!");
        SceneManager.LoadScene("Level1");
    }

    private void LoadGame()
    {
        Debug.Log("Load Button Clicked! (Şu an boş)");
    }

    private void OpenOptions()
    {
        Debug.Log("Options Button Clicked! (Şu an boş)");
    }

    private void ExitGame()
    {
        Debug.Log("Exit Button Clicked!");
        Application.Quit();
    }

    private void TryAgain()
    {
        if (SceneManager.GetActiveScene().name == "DefeatScene")
        {
            GameManager.Instance.lives = 3;
            GameManager.Instance.coin = 0;
            SceneManager.LoadScene("Level" + GameManager.Instance.level);
            EventManager.Instance.SetState(EventManager.GameState.GameContinue);

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void coinTextEvent(Vector3 PlayerPosition)
    {
        StartCoroutine(coinNum());
    }

    IEnumerator coinNum()
    {
        yield return new WaitForSeconds(0.1f);
        if (CoinText != null)
        {
            CoinText.text = "COIN: " + GameManager.Instance.coin;
        }
    }

    private void LivesandLevelText(bool iswin, int lives)
    {
        if (levelText != null)
        {
            levelText.text = "LEVEL " + GameManager.Instance.level;
            LivesText.text = "LIVES: " + GameManager.Instance.lives;
        }
    }

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
}