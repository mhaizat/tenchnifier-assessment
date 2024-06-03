using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardSceneManager : MonoBehaviour
{
    private ManagerHub managerHub;

    [SerializeField] private GameObject scoreEntryPrefab;
    [SerializeField] private GameObject notificationPopup;

    [SerializeField] private Transform contentTransform;

    [SerializeField] private Button backButton;

    private int index;

    private void Start()
    {
        managerHub = ManagerHub.Instance;

        notificationPopup.SetActive(false);

        backButton.onClick.AddListener(BackToMenu);
        
        PopulateLeaderboard();
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void PopulateLeaderboard()
    {
        if (managerHub.GetDataManager().GetGameData().playerEntryArray.Length == 0)
        {
            notificationPopup.SetActive(true);
        }

        for (int i = 0; i < managerHub.GetDataManager().GetGameData().playerEntryArray.Length; i++)
        {
            GameObject entry = Instantiate(scoreEntryPrefab, contentTransform.position, Quaternion.identity);

            DataManager.PlayerEntry playerEntry = managerHub.GetDataManager().GetGameData().playerEntryArray[i];

            entry.GetComponent<ScoreEntryBehavior>().PopulateEntry(index + 1, playerEntry.playerName, playerEntry.highscore);
            entry.transform.SetParent(contentTransform);

            RectTransform entryRectTransform = entry.GetComponent<RectTransform>();

            entryRectTransform.localScale = Vector2.one;

            index++;
        }
    }
}
