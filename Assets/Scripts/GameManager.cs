using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using SvDl.Tools;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Analytics;

public class GameManager : Singleton<GameManager>
{
    [Header("References")]
    public Planet[] Planets;
    public Derelict DerelictPrefab;
    public Robot RobotWarrior;
    public Robot RobotHarvester;
    public TMP_Text TimeText;

    [Header("Game settings")]
    public Vector2 DerelictByPlanet;
    public int StartingRobotCount = 30;
    public int GameTime = 600;
    public float DefaultSpeed = 1;

    public int MenuSceneIndex = 0;

    private Planet[] _createdPlanets = new Planet[0];
    public Planet[] CurrentPlanets => _createdPlanets;

    private float _gameTimeLeft = 0f;
    public bool started { get; private set; }
    private bool _gameEnded = false;

    public AnalyticsEventTracker[] GameEndAnalytics = new AnalyticsEventTracker[0];

    private void Start()
    {
        StartGame(); // Temporary
    }

    private void Update()
    {
        if (!this.started) return;
        _gameTimeLeft = Mathf.Max(0f, _gameTimeLeft - Time.deltaTime);

        System.TimeSpan t = System.TimeSpan.FromSeconds(_gameTimeLeft);
        TimeText.text = string.Format("{0:D2} : {1:D2}", t.Minutes, t.Seconds);

        if(_gameTimeLeft <= 0f && !_gameEnded)
        {
            OnTimerEnd();
        }
    }

    /// <summary>
    /// Let's play !
    /// </summary>
    public void StartGame()
    {
        this.started = true;
        foreach(var p in Planets)
        {
            p.Initialize(DerelictPrefab, Random.Range(Mathf.FloorToInt(DerelictByPlanet.x), Mathf.CeilToInt(DerelictByPlanet.y)));
        }

        foreach (var p in Player.Players)
        {
            p.Reinitialize(StartingRobotCount);
        }

        _gameTimeLeft = Application.isEditor ? 60 : GameTime;
    }

    /// <summary>
    /// Pretty self-explanatory
    /// </summary>
    private void Clear()
    {
        for (int i = 0; i < _createdPlanets.Length; i++)
        {
            Destroy(_createdPlanets[i].gameObject);
        }

        _createdPlanets = new Planet[0];
    }

    private void OnTimerEnd()
    {
        _gameEnded = true;

        int score1 = Player.Players[0].RemainingRobots;
        int score2 = Player.Players[1].RemainingRobots;

        if(score1 > score2)
        {
            GameOverScreen.Instance.TriggerScreen(TranslationFile.Player1Won);
        }
        else if (score2 > score1)
        {
            GameOverScreen.Instance.TriggerScreen(TranslationFile.Player2Won);
        }
        else if(score1 == score2)
        {
            GameOverScreen.Instance.TriggerScreen(TranslationFile.Draw);
        }

        if(TutorialManager.AnalyticsAgree)
        {
            foreach (var a in GameEndAnalytics)
            {
                Debug.Log(a.name + " event");
                a.TriggerEvent();
            }
        }
        

        InputManager.OnKeyDown -= this._BackToMainMenu;
        InputManager.OnKeyDown += this._BackToMainMenu;
    }

    //---------------------------------------------------
    /// <summary>
    /// Back to main menu
    /// </summary>
    private void _BackToMainMenu(KeyCode key, int player)
    {
        if (key != KeyCode.B) return;
        started = false;
        Clear();
        SceneManager.LoadScene(MenuSceneIndex, LoadSceneMode.Single);
    }
}
