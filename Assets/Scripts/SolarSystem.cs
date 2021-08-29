using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SvDl.Tools;

public class SolarSystem : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Animator backgroundAnimator;
    [NonSerialized]
    public RectTransform rectTransform;
    private bool _listeningAxis = false;
    [SerializeField]
    private UIPlanet[] _planets;
    private int _targetIndex = -1;

    public Player Owner;

    //------------------------------------------------
    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        this.rectTransform = this.GetComponent<RectTransform>();
        this.BindAxis();
        Player.OnPlayerMove += OnPlayerMove;
        this._planets[this._targetIndex].Select();
    }

    private void OnDestroy()
    {
        Player.OnPlayerMove -= OnPlayerMove;
    }

    private void OnPlayerMove(Player player, Planet planet)
    {
        if (player != Owner)
        {
            foreach(var p in _planets)
            {
                p.ToggleEnemyPresence(p.Planet == planet);
            }
        }
    }

    public void UpdateRobotCounts(Dictionary<Planet, List<Robot>> robotLists)
    {
        // Update robot counts displayed on solar map
        foreach(var p in _planets)
        {
            if(robotLists.ContainsKey(p.Planet))
            {
                p.UpdateRobotCount(robotLists[p.Planet].Count);
            }
        }

        // Call enemies solar maps to update their map
        foreach(var player in Player.Players)
        {
            if(player != Owner)
            {
                player.SolarSystem.UpdateEnemyRobotCounts(robotLists);
            }
        }
    }

    /// <summary>
    /// Called from another player to update ui elements
    /// </summary>
    public void UpdateEnemyRobotCounts(Dictionary<Planet, List<Robot>> enemyRobotLists)
    {
        foreach(var p in _planets)
        {
            p.ToggleEnemyRobotPresence(enemyRobotLists[p.Planet].Count > 0);
        }
    }

    //-----------------------------------------
    /// <summary>
    /// Bind Axis
    /// </summary>
    public void BindAxis()
    {
        this._listeningAxis = true;
        if (this._targetIndex < 0)
        {
            this._targetIndex = 0;
            this._planets[this._targetIndex].Select();
        }
        InputManager.OnKeyDown -= this._Input;
        InputManager.OnKeyDown += this._Input;
    }

    private void _Input(KeyCode key, int player)
    {
        if (player != Owner.PlayerID ||
            Owner.UIMode != UIMode.Map)
        {
            return;
        }

        switch (key)
        {
            // Clockwise
            case KeyCode.B:
            case KeyCode.RightArrow:
                this._planets[this._targetIndex].UnSelect();
                this._targetIndex++;
                if (this._targetIndex >= this._planets.Length) this._targetIndex = 0;
                this._planets[this._targetIndex].Select();
                break;

            // Anticlockwise
            case KeyCode.X:
            case KeyCode.LeftArrow:
                this._planets[this._targetIndex].UnSelect();
                this._targetIndex--;
                if (this._targetIndex < 0) this._targetIndex = this._planets.Length - 1;
                this._planets[this._targetIndex].Select();
                break;

            case KeyCode.A:
                this.UnbindAxis();
                this.GoToPlanet(this._planets[this._targetIndex].Planet);
                break;
        }
    }

    //-----------------------------------------
    /// <summary>
    /// Unbind Axis
    /// </summary>
    public void UnbindAxis()
    {
        this._listeningAxis = false;
        InputManager.OnKeyDown -= this._Input;
    }

    //-----------------------------------------
    /// <summary>
    /// Go to planet
    /// </summary>
    public void GoToPlanet(Planet planet)
    {
        backgroundAnimator.SetBool("Visible", false);
        this.StartCoroutine(this._GoToPlanet_Routine(planet));
    }

    //-----------------------------------------
    /// <summary>
    /// Go to planet
    /// </summary>
    /// <param name="planetLocalisation"></param>
    private IEnumerator _GoToPlanet_Routine(Planet planet)
    {
        Owner.SetSkybox(planet.SkyboxMaterial);

        Transform planetLocalisation = planet.transform;
        Bezier bezier = Bezier.IncreasingCurve(new Vector2(0.33f, 0.4f), new Vector2(0.66f, 1f));
        float startTime = Time.time;
        float dt = 0f;
        float duration = 0.5f;
        Vector3 startPosition = this._camera.transform.position;
        Vector3 movement = new Vector3(planetLocalisation.position.x, planetLocalisation.position.y, 0f);
        Vector2 solarPosition = rectTransform.anchoredPosition;
        Vector3 solarMovement = new Vector2(1000 * planetLocalisation.position.x * -1, 1000 * planetLocalisation.position.y * -1);

        while (dt < 1f)
        {
            dt = (Time.time - startTime) / duration;
            float dy = bezier.GetY(dt);
            this._camera.transform.position = startPosition + new Vector3(movement.x, movement.y) * dy;
            this.rectTransform.anchoredPosition = solarPosition + new Vector2(solarMovement.x, solarMovement.y) * dy;
            yield return null;
        }

        this._camera.transform.position = startPosition + movement;
        this.rectTransform.anchoredPosition = solarPosition + new Vector2(solarMovement.x, solarMovement.y);

        planet.Activate(Owner.PlayerID);
    }

    //-----------------------------------------
    /// <summary>
    /// Comback from planet
    /// </summary>
    /// <param name="planetLocalisation"></param>
    public void ComeBackFromPlanet()
    {
        backgroundAnimator.SetBool("Visible", true);
        if (this.rectTransform == null) return;
        if (this._camera == null) return;
        this.StartCoroutine(this._ComeBackFromPlanet());
    }

    //-----------------------------------------
    /// <summary>
    /// Comback from planet coroutine
    /// </summary>
    /// <param name="planetLocalisation"></param>
    private IEnumerator _ComeBackFromPlanet()
    {
        Bezier bezier = Bezier.IncreasingCurve(new Vector2(0.33f, 0.4f), new Vector2(0.66f, 1f));
        float startTime = Time.time;
        float dt = 0f;
        float duration = 0.5f;
        Vector3 startPosition = this._camera.transform.position;
        Vector3 movement = -startPosition;
        Vector2 solarPosition = this.rectTransform.anchoredPosition;
        Vector2 solarMovement = -solarPosition;

        while (dt < 1f)
        {
            dt = (Time.time - startTime) / duration;
            float dy = bezier.GetY(dt);
            this._camera.transform.position = startPosition + new Vector3(movement.x, movement.y) * dy;
            this.rectTransform.anchoredPosition = solarPosition + new Vector2(solarMovement.x, solarMovement.y);
            yield return null;
        }

        //this._camera.transform.position = Vector3.zero;

        BindAxis();
    }
}
