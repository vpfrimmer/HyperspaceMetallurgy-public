using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text NextButtonText;
    public Button NextButton;
    public Button PreviousButton;
    public Toggle AnalyticsToggle;
    public int GameSceneIndex = 1;

    public GameObject[] Screens = new GameObject[0];

    [Required]
    public BlackScreen BlackScreen;

    private int _currentScreenIndex = 0;

    public static bool AnalyticsAgree = false;

    private void Awake()
    {
        InputManager.OnKeyDown += OnKeyDownCallback;
    }

    private void Start()
    {
        PreviousButton.interactable = false;

        Display(0);

        BlackScreen.SetHidden();
        BlackScreen.onVisible += StartGame;
    }

    private void OnDestroy()
    {
        InputManager.OnKeyDown -= OnKeyDownCallback;
    }

    private void StartGame()
    {
        SceneManager.LoadScene(GameSceneIndex, LoadSceneMode.Single);
    }

    private void Display(int screenIndex)
    {
        foreach(var o in Screens)
        {
            o.SetActive(false);
        }

        Screens[screenIndex].SetActive(true);
        _currentScreenIndex = screenIndex;
    }

    private void OnKeyDownCallback(KeyCode key, int playerId)
    {
        if(key == KeyCode.A && NextButton.interactable)
        {
            OnNextButtonClicked();
        }
        else if(key == KeyCode.B && PreviousButton.interactable)
        {
            OnPreviousButtonClicked();
        }
    }

    public void OnAnalyticsToggleChange(bool value)
    {
        AnalyticsAgree = value;
        PlayerPrefs.SetInt("AnalyticsAgree", value ? 1 : 0);
    }

    public void OnNextButtonClicked()
    {
        int newIndex = _currentScreenIndex + 1;

        PreviousButton.interactable = true;

        if (newIndex == Screens.Length - 1)
        {
            NextButtonText.text = "Play !";
        }

        if(newIndex >= Screens.Length)
        {
            BlackScreen.Show();
        }
        else
        {
            Display(newIndex);
        }
    }

    public void OnPreviousButtonClicked()
    {
        int newIndex = _currentScreenIndex - 1;
        NextButtonText.text = "Next";

        if(newIndex == 0)
        {
            PreviousButton.interactable = false;
        }

        Display(newIndex);
    }
}
