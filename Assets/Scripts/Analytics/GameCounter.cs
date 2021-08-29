using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameCounter : SingletonPersistent<GameCounter>
{
    public int GameCount { get; private set; } = 0;

    public AnalyticsEventTracker OnNewGameAnalytic;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnGameLevelLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnGameLevelLoaded;
    }

    public void OnNewGame()
    {
        GameCount++;
    }

    private void OnGameLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1 && TutorialManager.AnalyticsAgree)
        {
            Debug.Log("OnNewGame event");
            OnNewGameAnalytic.TriggerEvent();
        }
    }
}
