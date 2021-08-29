using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to move a robot
/// </summary>
[CreateAssetMenu(fileName = "HarvestProgram", menuName = "ScriptableObjects/HarvestProgram")]
public class Harvest_Program : RobotProgram
{
    public override void Execute(Robot caller)
    {
        var hits = new List<Derelict>();
        RaycastHit2D[] allHits = Physics2D.RaycastAll(caller.transform.position, caller.transform.right, 0.75f, 1 << LayerMask.NameToLayer("Derelict"));

        foreach(var h in allHits)
        {
            hits.Add(h.collider.gameObject.GetComponent<Derelict>());
        }

        if(hits.Count == 0)
        {
            return;
        }

        Harvester harvester = caller.GetComponent<Harvester>();

        if(harvester.IsFree)
        {
            var _currentDerelict = hits[0];
            _currentDerelict.StartPickup(caller);
            harvester.Carry(_currentDerelict);

            if(Planet.IsCurrentPlanet(caller.Planet))
            {
                AudioManager.Instance.PlayAudio(SoundType.RobotCollect);
            }

            //caller.Owner.Notifications.Notify(TranslationFile.HarvestNotification + " " + caller.Planet.PlanetName);
        }

       
    }
}
