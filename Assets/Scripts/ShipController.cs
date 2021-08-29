using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Transform spawnRobot;

    public Vector3 GetSpawnRobotPosition()
    {
        return spawnRobot.transform.position;
    }

    public Quaternion GetSpawnRobotRotation()
    {
        return spawnRobot.rotation;
    }


}
