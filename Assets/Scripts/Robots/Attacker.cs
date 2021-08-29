using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public bool isAttacking;
    public int val1;
    public int val2;



    public void Attack(Robot caller, Robot enemy, GameObject hitParticlePrefab)
    {
        if (!isAttacking && caller.CurrentState != RobotState.Dying)
        {
            StartCoroutine(Attack_Routine(caller, enemy, hitParticlePrefab));
        }
    }

    public IEnumerator Attack_Routine(Robot caller, Robot enemy, GameObject hitParticlePrefab)
    {
        isAttacking = true;
        caller.CurrentState = RobotState.Attack;
        enemy.TakeHit(Random.value > 0.5f ? val1 : val2);

        Vector3 hitPos = enemy.transform.position;
        Instantiate(hitParticlePrefab, hitPos, Quaternion.identity);

        if(Planet.IsCurrentPlanet(caller.Planet))
        {
            if(caller.GetComponent<Harvester>())
            {
                AudioManager.Instance.PlayAudio(SoundType.RobotCombat);
            }
            else
            {
               AudioManager.Instance.PlayAudio(SoundType.RobotLaser);
            }
        }

        yield return new WaitForSeconds(1f);

        caller.CurrentState = RobotState.Idle;
        isAttacking = false;
    }
}
