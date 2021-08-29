using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    private Player owner;
    private bool _isActive = false;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(value);
            }

            _isActive = value;
        }
    }

    private void Awake()
    {
        IsActive = false;
    }

    public void Start()
    {
        owner = GetComponentInParent<Player>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive && collision.tag == "Robot" && collision.gameObject.GetComponent<Robot>().Owner == owner && collision.gameObject.GetComponent<Robot>().hasExitStartArea)
        {
            collision.gameObject.GetComponent<Robot>().CurrentState = RobotState.Tracted;
            AudioManager.Instance.PlayAudio(SoundType.RobotBeamed);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Robot")
        {
            collision.gameObject.GetComponent<Robot>().hasExitStartArea = true;
        }
    }
}
