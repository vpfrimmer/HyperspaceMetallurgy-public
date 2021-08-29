using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Derelict : MonoBehaviour
{
    private const float PICKUP_TIME = 1.0F;

    private SpriteRenderer _renderer;
    private Robot _currentCarrier;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Hide()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
    }

    public void SetAtPosition(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        _collider.enabled = true;
        _renderer.enabled = true;
    }

    public void StartPickup(Robot caller)
    {
        if(_currentCarrier == null)
        {
            StartCoroutine(Harvest_Routine(caller));
        }
    }

    public void Release()
    {
        SetAtPosition(_currentCarrier.derelictLocation.transform.position,
                      _currentCarrier.derelictLocation.transform.rotation);
        _currentCarrier = null;
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }

    public IEnumerator Harvest_Routine(Robot caller)
    {
        _currentCarrier = caller;
        Hide();
        caller.CurrentState = RobotState.Harvesting;

        yield return new WaitForSeconds(PICKUP_TIME);

        caller.CurrentState = RobotState.Idle;
    }
}
