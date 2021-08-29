using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SvDl.Tools;
using Sirenix.OdinInspector;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Required]
    private BlackScreen BlackScreen = null;

    /// Players text confirmation animators
    [SerializeField, Required]
    private Animator Player1Validator = null;
    [SerializeField, Required]
    private Animator Player2Validator = null;

    /// Player readiness flags
    private bool _player1OK = false;
    private bool _player2OK = false;
    private bool _playStarted = false;

    [SerializeField]
    private int GameSceneIndex = 1;
    [SerializeField]
    private int TutorialSceneIndex = 2;

    
    private void Start()
    {
        // There shouldn't be any game manager here. Destroy it if we find one.
        GameManager gm = GameManager.Instance;
        if (gm) Destroy(gm);

        // Reinitializing input callback
        InputManager.Dispose();
        InputManager.OnKeyDown += OnKeyDown;

        BlackScreen.SetHidden();
        BlackScreen.onVisible += StartGame;
    }

    private void OnKeyDown(KeyCode kc, int player)
    {
        if (player == 0 && kc == KeyCode.A)
        {
            Player1Validator.SetTrigger("Validate");
            _player1OK = true;
        }
        if (player == 1 && kc == KeyCode.A)
        {
            Player2Validator.SetTrigger("Validate");
            _player2OK = true;
        }
        if(_player1OK && _player2OK && !_playStarted)
        {
            _playStarted = true;
            StartCoroutine(DifferedPlay());
        }
    }
        
    private IEnumerator DifferedPlay()
    {
        InputManager.OnKeyDown -= OnKeyDown;
        yield return new WaitForSeconds(0.5f);
        ShowBlackscreen();
    }

    public void ShowBlackscreen()
    {
        BlackScreen.Show();
    }

    // Launch the game scene and calls some analytics beforehand
    private void StartGame()
    {        
        GameCounter.Instance.OnNewGame();
        SceneManager.LoadScene(TutorialSceneIndex, LoadSceneMode.Single);
    }
}