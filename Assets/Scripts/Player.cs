using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum UIMode
{
    Map,
    Planet,
    Undefined
}

public class Player : MonoBehaviour
{
    public static Player[] Players = new Player[2];

    public int PlayerID = 0; // Player 1 : 0, Player 2 : 1

    [HideInInspector]
    public Camera Camera;

    [Header("Player objects references")]
    public ShipController Ship;
    public TractorBeam TractorBeam;
    public TMP_Text ScoreText;
    public SolarSystem SolarSystem;

    [Header("UI objects references")]
    public GameObject[] MapModeObjects;
    public GameObject[] PlanetModeObjects;
    public NotificationsGUICtrl Notifications;

    private Planet _currentPlanet = null;
    public Planet CurrentPlanet => _currentPlanet;

    private GameManager _gameManager;
    private int _remainingRobots = 0;
    private UIMode _uiMode = UIMode.Undefined;

    private Skybox _cameraSkybox;

    private Dictionary<Planet, List<Robot>> _robotsInPlanet = new Dictionary<Planet, List<Robot>>();

    public UIMode UIMode
    {
        get => _uiMode;
        set
        {
            if (_uiMode == value) return;

            AudioManager.Instance.PlayAudio(SoundType.SwitchMode);

            foreach (var o in MapModeObjects) o.SetActive(value == UIMode.Map);
            foreach (var o in PlanetModeObjects) o.SetActive(value == UIMode.Planet);

            // Going back to map mode
            if(value == UIMode.Map)
            {
                SolarSystem.ComeBackFromPlanet();
            }

            _uiMode = value;
        }
    }

    public int RemainingRobots
    {
        get => _remainingRobots;
        set
        {
            _remainingRobots = value;

            ScoreText.text = "Robots : " + value.ToString();
        }
    }

    // Values used by analytics
    public int RobotsLost { get; private set; } = 0;
    public int RobotsPickedUp { get; private set; } = 0;
    public int PlanetSwitchCount { get; private set; } = 0;

    public delegate void PlayerMovementEvent(Player player, Planet planet);
    public static event PlayerMovementEvent OnPlayerMove;

    private void Awake()
    {
        Camera = GetComponentInChildren<Camera>();
        _cameraSkybox = Camera.GetComponent<Skybox>();
        Ship = GetComponentInChildren<ShipController>();
        SolarSystem.Owner = this;

        Players[PlayerID] = this;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;

        InputManager.OnKeyDown += OnKeyPressed;
    }

    private void OnDestroy()
    {
        InputManager.OnKeyDown -= OnKeyPressed;
    }

    public void Reinitialize(int robotCount)
    {
        _robotsInPlanet.Clear();
        foreach (var planet in GameManager.Instance.Planets)
        {
            _robotsInPlanet.Add(planet, new List<Robot>());
        }

        RemainingRobots = robotCount;

        //// Looking away
        //TeleportCameraTo(new Vector2(-100f, 0f));

        // Hiding ship
        Ship.gameObject.SetActive(false);

        UIMode = UIMode.Map;
    }

    public void TeleportCameraTo(Vector3 position)
    {
        position.z -= 10;
        Camera.transform.position = position;
    }

    /// <summary>
    /// Effectively move the player to another planet
    /// </summary>
    public void SetCurrentPlanet(Planet planet)
    {
        if(_currentPlanet != planet)
        {
            PlanetSwitchCount++;
        }

        _currentPlanet = planet;

        //// Moving player camera
        //TeleportCameraTo(planet.transform.position);
        // Moving player ship
        Ship.gameObject.SetActive(true);
        Ship.transform.position = planet.ShipPositions[PlayerID].position;

        UIMode = UIMode.Planet;

        OnPlayerMove(this, planet);
    }

    public void SetSkybox(Material skyboxMaterial)
    {
        _cameraSkybox.material = skyboxMaterial;
    }

    private void ToggleTractorBeam()
    {
        if (UIMode != UIMode.Planet) return;

        TractorBeam.IsActive = !TractorBeam.IsActive;
    }

    public void AddPoint()
    {
        RemainingRobots++;
        SolarSystem.UpdateRobotCounts(_robotsInPlanet);
    }

    public void RemovePoint()
    {
        RemainingRobots--;
    }

    #region Robotos spawn

    [Button("Spawn Warrior")]
    public void SpawnWarrior()
    {
        SpawnRobot(_gameManager.RobotWarrior);
    }

    [Button("Spawn Harvester")]
    public void SpawnHarvester()
    {
        SpawnRobot(_gameManager.RobotHarvester);
    }

    private void SpawnRobot(Robot robot)
    {
        bool isRobotInTractorBeam = false;
        Collider2D hitColliders = Physics2D.OverlapArea(Ship.transform.position - Ship.transform.right * 0.5f, 
            TractorBeam.transform.position + TractorBeam.transform.right * 0.5f,
            1 << LayerMask.NameToLayer("Robot"));
        if (hitColliders != null)
        {
               isRobotInTractorBeam = true;
        }

        if (!isRobotInTractorBeam && RemainingRobots > 0 &&
           UIMode == UIMode.Planet)
        {
            Robot newRobot = Instantiate(robot, transform);
            newRobot.transform.position = Ship.GetSpawnRobotPosition();
            newRobot.transform.rotation = Ship.GetSpawnRobotRotation();
            newRobot.Initialize(this, CurrentPlanet);
            newRobot.Speed *= _gameManager.DefaultSpeed;
            //newRobot.MainSprite.color = PlayerColor;

            _robotsInPlanet[CurrentPlanet].Add(newRobot);

            SolarSystem.UpdateRobotCounts(_robotsInPlanet);

            AudioManager.Instance.PlayAudio(SoundType.RobotSpawn);

            RemovePoint();
        }
    }

    public void OnRobotDeath(Robot rip)
    {
        Planet p = rip.Planet;
        if(_robotsInPlanet[p].Contains(rip))
        {
            _robotsInPlanet[p].Remove(rip);
            SolarSystem.UpdateRobotCounts(_robotsInPlanet);

            Notifications.NotifyLostRobot(p);
            RobotsLost++;
        }
    }

    public void OnRobotRetrieve(Robot retrievedRoboto)
    {
        AddPoint();

        Planet p = retrievedRoboto.Planet;
        if (_robotsInPlanet[p].Contains(retrievedRoboto))
        {
            _robotsInPlanet[p].Remove(retrievedRoboto);
            SolarSystem.UpdateRobotCounts(_robotsInPlanet);
        }

        if (retrievedRoboto.GetComponent<Harvester>())
        {
            Harvester h = retrievedRoboto.GetComponent<Harvester>();

            if (!h.IsFree)
            {
                AddPoint();
                RobotsPickedUp++;
            }

            h.OnRetrieve();
        }
    }

    #endregion

    /// <summary>
    /// Input key callbacks
    /// </summary>
    private void OnKeyPressed(KeyCode key, int id)
    {
        if(id != PlayerID)
        {
            // This isn't for us.
            return;
        }

        switch(key)
        {
            case KeyCode.A:
                ToggleTractorBeam();
                break;

            case KeyCode.B:
                SpawnHarvester();

                break;

            case KeyCode.X:
                SpawnWarrior();
                break;

            case KeyCode.Y:
                if (UIMode == UIMode.Planet)
                {
                    UIMode = UIMode.Map;
                }
                break;

            default:
                Debug.LogWarning("Unknown key received : " + key.ToString());
                break;
        }
    }
}
