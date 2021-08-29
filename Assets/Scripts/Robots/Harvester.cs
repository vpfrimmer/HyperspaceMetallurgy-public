using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    private Derelict _currentDerelict = null;

    public bool IsFree => _currentDerelict == null;

    public void Carry(Derelict der)
    {
        if(IsFree)
        {
            _currentDerelict = der;
        }
    }

    public void OnRetrieve()
    {
        if(_currentDerelict != null)
        {
            _currentDerelict.Disappear();
            _currentDerelict = null;
        }
    }

    public void OnDeath()
    {
        if(_currentDerelict != null)
        {
            _currentDerelict.Release();
            _currentDerelict = null;
        }
    }
}
