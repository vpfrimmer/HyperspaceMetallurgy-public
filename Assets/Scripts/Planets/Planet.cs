using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Planet : MonoBehaviour
{
    public string PlanetName = "Unnamed";
    public Transform[] ShipPositions = new Transform[2];
    public PlanetVisualController Model;
    public CircleCollider2D GroundCollider;
    public CircleCollider2D GravityWellZone;
    public Material SkyboxMaterial;
    public float ColliderRadiusOffset = 0f;
    public Sprite PlanetMiniature;
    
    public void Initialize(Derelict DerelictPrefab, int derelictCount)
    {
        Model.UpdateMeshes();

        float initialRadius = GroundCollider.radius;
        float radius = Model.Radius + ColliderRadiusOffset;

        // Adapting colliders size
        GroundCollider.radius = radius;
        GravityWellZone.radius = 2f * radius;

        // Adjusting ship positions
        foreach(var sp in ShipPositions)
        {
            sp.Translate(sp.up * (radius - initialRadius), Space.World);
        }

        // Generating derelicts
        for(int i = 0; i < derelictCount; i++)
        {
            Vector3 derPos = (Random.insideUnitCircle.normalized * radius);
            derPos += transform.position;

            Derelict newDerelict = Instantiate(DerelictPrefab, transform);

            newDerelict.transform.position = derPos;
            newDerelict.transform.up = (derPos - transform.position).normalized;
        }
    }

    /// <summary>
    /// Called by clicking on a planet in the stellar map
    /// </summary>
    public void Activate(int playerId)
    {
        Player.Players[playerId].SetCurrentPlanet(this);
    }

    /// <summary>
    /// Used for debug in editor
    /// </summary>
    [Button("Debug Activate")]
    public void DebugActivate()
    {
        Activate(0);
        Activate(1);
    }

    public static bool IsCurrentPlanet(Planet planet)
    {
        foreach(Player p in Player.Players)
        {
            if(planet == p.CurrentPlanet && p.UIMode == UIMode.Planet)
            {
                return true;
            }
        }

        return false;
    }
}
