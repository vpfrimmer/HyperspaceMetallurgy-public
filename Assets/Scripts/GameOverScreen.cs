using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : Singleton<GameOverScreen>
{
    public TMP_Text EndText;

    private Animator _animator;
    private bool _isDisplayed = false;
    public bool IsDisplayed
    {
        get => _isDisplayed;
        set
        {
            if (value == _isDisplayed) return;
                
            if(value)
            {
                _animator.SetTrigger("Trigger");
            }

            _isDisplayed = value;
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        InputManager.OnKeyDown -= OnKeyPressed;
    }

    public void TriggerScreen(string message)
    {
        if (IsDisplayed) return;

        message += System.Environment.NewLine + System.Environment.NewLine + "Press Escape to go back to title screen";

        EndText.text = message;
        IsDisplayed = true;
        InputManager.OnKeyDown += OnKeyPressed;
    }

    private void OnKeyPressed(KeyCode key, int playerId)
    {
        if(key == KeyCode.Escape)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
