using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoverSceneManager : MonoBehaviour
{
    private ManagerHub managerhub;

    [SerializeField] private TMP_InputField usernameInputField;

    [SerializeField] private Button confirmButton;
    [SerializeField] private Button createNewProfileButton;
    [SerializeField] private Button[] menuSceneButton;

    [SerializeField] private GameObject signinPopup;
    [SerializeField] private GameObject loadProfilePopup;

    void Start()
    {
        managerhub = ManagerHub.Instance;

        loadProfilePopup.SetActive(false);
        signinPopup.SetActive(false);

        menuSceneButton[0].onClick.AddListener(LoadMenuScene);
        menuSceneButton[1].onClick.AddListener(LoadMenuScene);
        createNewProfileButton.onClick.AddListener(ProfileCreationPage);
        confirmButton.onClick.AddListener(SetUserProfile);

        if (managerhub.GetDataManager().GetCurrentProfile().playerName != string.Empty)
        {
            loadProfilePopup.SetActive(true);
            menuSceneButton[1].gameObject.SetActive(true);
        }
        else
        { 
            signinPopup.SetActive(true);
            menuSceneButton[1].gameObject.SetActive(false);
        }
    }

    void ProfileCreationPage()
    { 
       loadProfilePopup.SetActive(false);
       signinPopup.SetActive(true);
    }

    void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void SetUserProfile()
    {
        if (string.IsNullOrEmpty(usernameInputField.text))
        {
            return;
        }

        managerhub.GetDataManager().SetCurrentProfile(usernameInputField.text);

        SceneManager.LoadScene("MenuScene");
    }
}
