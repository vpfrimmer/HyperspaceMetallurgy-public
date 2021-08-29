using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDecorationsController : MonoBehaviour
{
    [SerializeField]
    private Vector2 RotationSpeedRange = new Vector2(0.5f, 1.2f);
    [SerializeField]
    private Transform[] DecorationTransforms;
    [SerializeField]
    private float RadiusOffset = 2f;
    [SerializeField]
    private float RadiusOffsetRandomRange = 0.5f;

    private float rotationSpeed;
    private int rotationDirection;

    private void Start()
    {
        rotationSpeed = Random.Range(RotationSpeedRange.x, RotationSpeedRange.y);
        rotationDirection = Random.Range(-1f, 1f) > 0 ? 1 : -1;
        transform.Rotate(0f, 0f, Random.Range(0, 360f));
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, Time.deltaTime * rotationSpeed * rotationDirection);
    }

    public void UpdateDecorations(float planetRadius)
    {
        foreach (Transform t in DecorationTransforms)
        {
            t.localPosition = new Vector3(0f, planetRadius + RadiusOffset + Random.Range(-RadiusOffsetRandomRange, RadiusOffsetRandomRange), 0f);
        }
    }
}
