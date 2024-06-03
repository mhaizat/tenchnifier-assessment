using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager _instance;
    public static GameSceneManager Instance { get { return _instance; } }

    private ManagerHub managerHub;

    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text endGameScoreText;
    [SerializeField] private TMP_Text powerUpCountText;
    [SerializeField] private TMP_Text countdownTimeText;

    [SerializeField] private Button itemBoostButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button[] itemBoosterButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button reshuffleButton;

    [SerializeField] private GameObject itemSelectionPopup;
    [SerializeField] private GameObject pausePopup;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject endGamePopup;
    [SerializeField] private GameObject countdownPopup;

    [SerializeField] private Canvas canvas;

    //! REMINDER: value for test purpose only
    public float timer;
    private float multiplierTimer;
    float countdownTime = 3.0f;
    
    private bool hasStarted;
    private bool isScoreMultiplier;

    private int score = 0;
    private int powerUpIndex;
    private int powerUpCount = 5;


    void Awake()
    {
        Time.timeScale = 1.0f;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        managerHub = ManagerHub.Instance;

        if (managerHub)
        {
            usernameText.text = $"Name: {managerHub.GetDataManager().GetCurrentProfile().playerName}";
            highscoreText.text = $"Highscore: {managerHub.GetDataManager().GetCurrentProfile().highscore.ToString()}";
        }

        powerUpCountText.text = powerUpCount.ToString();

        itemBoostButton.onClick.AddListener(UseItemBoost);
        pauseButton.onClick.AddListener(PauseGame);
        quitButton.onClick.AddListener(ReturnToMenu);
        menuButton.onClick.AddListener(ReturnToMenu);
        restartButton.onClick.AddListener(RestartGame);
        resumeButton.onClick.AddListener(ResumeGame);
        reshuffleButton.onClick.AddListener(ShuffleCards);
        itemBoosterButton[0].onClick.AddListener(() => SelectPowerUp(0));
        itemBoosterButton[1].onClick.AddListener(() => SelectPowerUp(1));

        itemSelectionPopup.SetActive(true);
        pausePopup.SetActive(false);
        endGamePopup.SetActive(false);
        pausePopup.SetActive(false);
        countdownPopup.SetActive(false);
    }

    public void ShuffleCards()
    {
        ManagerHub.ReshuffleCards();
    }

    void StartGame()
    { 
        hasStarted = true;
    }
    void PauseGame()
    {
        pausePopup.SetActive(true);
        Time.timeScale = 0.0f;
    }

    void EndGame()
    {
        Time.timeScale = 0.0f;
        endGamePopup.SetActive(true);
        endGameScoreText.text = $"Your score: {score.ToString()}";

        managerHub.GetDataManager().UpdateHighscoreScoreDatabase(managerHub.GetDataManager().GetCurrentProfile().playerName, score);
    }

    void ResumeGame()
    {
        pausePopup.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void ReturnToMenu()
    {
        //! REMINDER: check if anything else needs changing
        SceneManager.LoadScene("MenuScene");
    }

    void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    void SelectPowerUp(int index)
    {
        countdownPopup.SetActive(true);

        StartCoroutine(CountdownTimer());
        itemSelectionPopup.SetActive(false);

        powerUpIndex = index;
    }

    void UseItemBoost()
    {
        powerUpCount--;
        powerUpCountText.text = powerUpCount.ToString();

        if (powerUpCount <= 0)
        {
            itemBoostButton.interactable = false;
        }

        if (powerUpIndex == 0)
        {
            timer += 5.0f;
            return;
        }

        isScoreMultiplier = true;
        itemBoostButton.interactable = false;
    }

    void Update()
    {
        if (hasStarted)
        {
            timer -= Time.deltaTime;

            timerText.text = Mathf.Round(timer).ToString();

            currentScoreText.text = $"Score: {score}";

            if (timer <= 0)
            {
                hasStarted = false;
                EndGame();
            }

            if (isScoreMultiplier)
            {
                multiplierTimer += Time.deltaTime;

                if (multiplierTimer >= 5.0f)
                {
                    multiplierTimer = 0f;
                    itemBoostButton.interactable = true;
                    isScoreMultiplier = false;
                }
            }
        }
    }

    public IEnumerator CountdownTimer()
    {
        float startTime = Time.time;
        float endTime = startTime + countdownTime;
        int countdownValue = Mathf.CeilToInt(countdownTime);

        while (countdownValue > 0)
        {
            countdownTimeText.text = countdownValue.ToString();
            yield return new WaitForSeconds(1f);
            countdownValue--;
        }

        countdownTimeText.text = "GO !";
        yield return new WaitForSeconds(1f);

        countdownTimeText.text = "";
        countdownPopup.SetActive(false);
        StartGame();
    }

    public Canvas GetCanvas() { return canvas; }
    public int GetScore() { return score; }

    public void SetScore(int _score)
    {
        if (isScoreMultiplier && _score >= 0)
        {
            score += (_score * 2);
        }

        score += _score;
    }
}
