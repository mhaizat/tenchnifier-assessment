using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    private ManagerHub managerHub;

    [SerializeField] private Button startButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text highscoreText;

    private void Start()
    {
        managerHub = ManagerHub.Instance;

        usernameText.text = $"Name: {managerHub.GetDataManager().GetCurrentProfile().playerName}";
        highscoreText.text = $"Highscore: {managerHub.GetDataManager().GetCurrentProfile().highscore.ToString()}";

        startButton.onClick.AddListener(StartGame);
        leaderboardButton.onClick.AddListener(LoadLeaderboardScene);
        exitButton.onClick.AddListener(ExitApplication);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void LoadLeaderboardScene()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    private void ExitApplication()
    {
        Application.Quit();
    }
}
