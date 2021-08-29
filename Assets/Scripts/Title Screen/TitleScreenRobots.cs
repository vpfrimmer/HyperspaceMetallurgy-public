using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenRobots : MonoBehaviour
{
    [SerializeField]
    private float RobotsRotationSpeed;

    private GameObject[] ClockwiseRobots;
    private GameObject[] CounterclockwiseRobots;

    private const string RobotRunTriggerName = "run";
    private const string RobotCarryingTriggerName = "carry";

    private void Start()
    {
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            a.SetTrigger(RobotRunTriggerName);
            TitleScreenRobot robot = a.GetComponent<TitleScreenRobot>();
            if (robot != null && robot.ReaperCarrying)
                a.SetTrigger(RobotCarryingTriggerName);
        }
        ClockwiseRobots = GameObject.FindGameObjectsWithTag("TitleClockwiseRobot");
        CounterclockwiseRobots = GameObject.FindGameObjectsWithTag("TitleCounterclockwiseRobot");
    }

    private void Update()
    {
        foreach (GameObject go in ClockwiseRobots)
        {
            go.transform.Rotate(0f, 0f, -Time.deltaTime * RobotsRotationSpeed);
        }
        foreach (GameObject go in CounterclockwiseRobots)
        {
            go.transform.Rotate(0f, 0f, Time.deltaTime * RobotsRotationSpeed);
        }
    }
}
