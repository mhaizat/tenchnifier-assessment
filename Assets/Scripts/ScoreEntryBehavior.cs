using TMPro;
using UnityEngine;

public class ScoreEntryBehavior : MonoBehaviour
{
    [SerializeField] private TMP_Text entryNumberText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text highscoreText;

    public void PopulateEntry(int entry, string name, int highscore)
    {
        entryNumberText.text = $"{entry.ToString()}.";
        nameText.text = name;
        highscoreText.text = highscore.ToString();
    }
}
