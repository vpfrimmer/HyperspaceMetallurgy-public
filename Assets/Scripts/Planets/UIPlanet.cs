using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UIPlanet : MonoBehaviour
{
    [SerializeField]
	private Planet _target;
    private SolarSystem _solarSystem;
    private RectTransform _rectTransform;
    private Color _planetColor;

    [Header("UI thingies")]
    public TMP_Text NameText;

    [Space]
    public GameObject RobotCountObject;
    public TMP_Text RobotCountText;

    [Space]
    public GameObject EnemyPresenceObject;
    public TMP_Text EnemyPresenceText;

    [Space]
    public GameObject EnemyRobotPresenceObject;

    [Space]
    public Image Aura;
    public Image PlayerShipImage;

    public Planet Planet => _target;
       
    public void Awake() 
    {
        _rectTransform = GetComponent<RectTransform>();
        _solarSystem = GetComponentInParent<SolarSystem>();
        _planetColor = GetComponent<Image>().color;
        UnSelect();

        ToggleEnemyPresence(false);
        ToggleEnemyRobotPresence(false);
        UpdateRobotCount(0);
    }

    public void Start()
    {
        
        EnemyPresenceText.text = TranslationFile.EnemyDetected;
        NameText.text = Planet.PlanetName;
    }

    public void Select()
    {
        Aura.gameObject.SetActive(true);
        PlayerShipImage.gameObject.SetActive(true);
        GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void UnSelect()
    {
        Aura.gameObject.SetActive(false);
        PlayerShipImage.gameObject.SetActive(false);
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
    }

    public void ToggleEnemyPresence(bool state)
    {
        EnemyPresenceObject.SetActive(state);
    }

    public void ToggleEnemyRobotPresence(bool state)
    {
        EnemyRobotPresenceObject.SetActive(state);
    }

    public void UpdateRobotCount(int count)
    {
        if(count == 0)
        {
            RobotCountObject.SetActive(false);
        }
        else
        {
            RobotCountText.text = TranslationFile.Robots + count.ToString();
            RobotCountObject.SetActive(true);
        }
    }
}
