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

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI LivesText;
    [SerializeField] private TextMeshProUGUI EndGameText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    //===============================================================

    public static UIController Instance
    {
        get;
        private set;
    }
    //===============================================================
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.Instance.onCoinCollect += coinTextEvent;
        EventManager.Instance.onEndgameController += LevelTextEvent;
        EventManager.Instance.onEndgameController += LivesTextEvent;

    }
    //===============================================================
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.Instance.onCoinCollect -= coinTextEvent;
        EventManager.Instance.onEndgameController -= LevelTextEvent;
        EventManager.Instance.onEndgameController -= LivesTextEvent;


    }
    //===============================================================
    private void Start()
    {

        EventManager.Instance.SetState(EventManager.GameState.GameContinue);

    }
    //==================================================================================

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UIElements();
    }
    //===============================================================

    private void UIElements()
    {

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();
            loadButton = GameObject.Find("LoadButton")?.GetComponent<Button>();
            optionsButton = GameObject.Find("OptionsButton")?.GetComponent<Button>();
            exitButton = GameObject.Find("ExitButton")?.GetComponent<Button>();

            if (playButton) playButton.onClick.AddListener(PlayGame);
            if (loadButton) loadButton.onClick.AddListener(LoadGame);
            if (optionsButton) optionsButton.onClick.AddListener(OpenOptions);
            if (exitButton) exitButton.onClick.AddListener(ExitGame);
        }
        else if (SceneManager.GetActiveScene().name == "DefeatScene")
        {
            YouLoseText = GameObject.Find("YouLoseText")?.GetComponent<TextMeshProUGUI>();
            TryAgainButton = GameObject.Find("TryAgainButton")?.GetComponent<Button>();
            MainMenuButton = GameObject.Find("MainMenuButton")?.GetComponent<Button>();
            exitButton = GameObject.Find("ExitButton")?.GetComponent<Button>();

            if (TryAgainButton) TryAgainButton.onClick.AddListener(TryAgain);
            if (MainMenuButton) MainMenuButton.onClick.AddListener(GoToMainMenu);
            if (exitButton) exitButton.onClick.AddListener(ExitGame);
            YouLoseTextAnimation();
        }
    }
    //===============================================================
    private void PlayGame()
    {
        Debug.Log("Play Button Clicked!");
        SceneManager.LoadScene("Level1");
    }
    //===============================================================
    private void LoadGame()
    {
        Debug.Log("Load Button Clicked! (Şu an boş)");
    }
    //===============================================================
    private void OpenOptions()
    {
        Debug.Log("Options Button Clicked! (Şu an boş)");
    }
    //===============================================================
    private void ExitGame()
    {
        Debug.Log("Exit Button Clicked!");
        Application.Quit();
    }
    // Fonksiyonlar
    private void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    //==================================================================================


    //TEXT'LER
    //===============================================================
    private void coinTextEvent(Vector3 PlayerPosition)
    {
        StartCoroutine(coinNum());
    }
    //==================================================================================

    IEnumerator coinNum()
    {
        yield return new WaitForSeconds(0.1f);
        coinText.text = "COIN: " + GameManager.Instance.coin;
    }
    //==================================================================================
    private void LevelTextEvent(bool iswin, int score)
    {
        levelText.text = "LEVEL " + GameManager.Instance.level;
    }
    //==================================================================================
    private void LivesTextEvent(bool iswin, int score)
    {
        LivesText.text = "LIVES: " + GameManager.Instance.lives;
    }
    //==================================================================================
    private void YouLoseTextAnimation()
    {
        if (YouLoseText != null)
        {
           
            YouLoseText.transform.DOScale(Vector3.one * 1.5f, 0.5f) 
                .SetLoops(-1, LoopType.Yoyo) //sonsuz döngü ekliyor
                .SetEase(Ease.InOutSine); 

            YouLoseText.DOColor(new Color(1f, 0f, 0f), 0.5f)
                .SetLoops(-1, LoopType.Yoyo) 
                .SetEase(Ease.Linear);
        }
    }

}
