using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to move a robot
/// </summary>
[CreateAssetMenu(fileName = "AttackProgram", menuName = "ScriptableObjects/AttackProgram")]
public class Attack_Program : RobotProgram
{
    public GameObject HitParticlePrefab;

    public override void Execute(Robot caller)
    {
        if (caller.IsThereAnEnemy(out Robot enemy))
        {
            caller.GetComponent<Attacker>().Attack(caller, enemy, HitParticlePrefab);
        }
    }


}
