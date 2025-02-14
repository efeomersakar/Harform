using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIController : MonoBehaviour
{


    private Button playButton;
    private Button loadButton;
    private Button optionsButton;
    private Button exitButton;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI EndGameText;
    [SerializeField] private TextMeshProUGUI ScoreText;
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
    }
    //===============================================================
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventManager.Instance.onCoinCollect -= coinTextEvent;
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

    }
    //===============================================================
    private void PlayGame()
    {
        Debug.Log("Play Button Clicked!");
        SceneManager.LoadScene("Level1"); // Oyun sahnesinin adını güncelle
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
}
