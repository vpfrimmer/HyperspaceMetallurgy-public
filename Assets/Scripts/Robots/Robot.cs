using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RobotState
{
    Warmup,
    Idle,
    Run,
    Instanciating,
    Harvesting,
    Tracted,
    Dying,
    Attack
}

public class Robot : MonoBehaviour
{
    private const float COLLISION_CHECK_DISTANCE = 0.75f;
    private float INITIAL_DELAY = 1f;

    [HideInInspector]
    public Rigidbody2D Rigidbody;

    public SpriteRenderer MainSprite;
    public List<RobotProgram> Programs = new List<RobotProgram>();

    public float Speed;
    public AnimatorOverrideController Player1AnimatorController;
    public AnimatorOverrideController Player2AnimatorController;

    private Player _owner;
    private Animator _animator;

    RobotState _robotState = RobotState.Instanciating;
    public Transform derelictLocation;
    public int lifeMeter = 10;
    public RobotState CurrentState
    {
        get => _robotState;
        set
        {
            if (value == _robotState) return;

            switch(_robotState)
            {
                case RobotState.Warmup:
                    Rigidbody.constraints = RigidbodyConstraints2D.None;
                    break;
            }

            switch(value)
            {
                case RobotState.Idle:
                    _animator.SetTrigger("idle");
                    break;

                case RobotState.Run:
                    _animator.SetTrigger("run");
                    break;

                case RobotState.Tracted:
                    Tracted();
                    break;

                case RobotState.Warmup:
                    Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    break;

                case RobotState.Harvesting:
                    _animator.SetTrigger("carry");
                    _animator.SetBool("carrying", true);
                    break;

                case RobotState.Attack:
                    _animator.SetTrigger("attack");
                    break;

                case RobotState.Dying:
                    Kill();
                    break;

                default:
                    Debug.LogWarning("Unknown state. Skipping.");
                    return;
                    
            }

            _robotState = value;
        }
    }

    public Player Owner => _owner;
    public Planet Planet { get; private set; }

    public bool hasExitStartArea { get; internal set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void Initialize(Player caller, Planet planet)
    {
        _owner = caller;
        Planet = planet;
        CurrentState = RobotState.Warmup;
        _animator.runtimeAnimatorController = caller.PlayerID == 0 ? Player1AnimatorController : Player2AnimatorController;
    }
    
    private void Update()
    {
        Vector3 originalRightDir = transform.right;

        switch(CurrentState)
        {
            case RobotState.Warmup:
                if (INITIAL_DELAY <= 0) CurrentState = RobotState.Run;
                break;

            case RobotState.Run:
                if(!IsThereAnyRobotsInFrontOfUs())
                {
                    Move();
                    originalRightDir = transform.right;
                }
                else
                {
                    CurrentState = RobotState.Idle;
                    Stop();
                }
                break;

            case RobotState.Idle:
                if(!IsThereAnyRobotsInFrontOfUs())
                {
                    CurrentState = RobotState.Run;
                }
                else
                {
                    Stop();
                }
                break;

                // WARNING BEING KILLED STOP UPDATE HERE
            case RobotState.Dying:
                return;
        }

        // Used to call programs
        foreach (var p in Programs)
        {
            p.CallCommand(this);
        }

        INITIAL_DELAY = Mathf.Max(INITIAL_DELAY - Time.deltaTime, 0f);
    }

    private void Stop()
    {
        Rigidbody.velocity = Vector2.zero;
    }

    private void Move()
    {
        Rigidbody.velocity = transform.right.normalized * Speed * Time.deltaTime;
    }

    public bool IsThereAnyRobotsInFrontOfUs(out List<Robot> hits)
    {
        hits = new List<Robot>();
        RaycastHit2D[] allHits = Physics2D.RaycastAll(transform.position, transform.right, COLLISION_CHECK_DISTANCE, 1 << LayerMask.NameToLayer("Robot"));

        if (allHits.Length > 0)
        {
            foreach (var h in allHits)
            {
                if (h.collider.gameObject != gameObject)
                {
                    hits.Add(h.collider.gameObject.GetComponent<Robot>());
                    
                }
            }
        }

        if(hits.Count > 0)
        {
        //    Debug.DrawRay(transform.position, transform.right * COLLISION_CHECK_DISTANCE, Color.green);
            return true;
        }
        else
        {
        //    Debug.DrawRay(transform.position, transform.right * COLLISION_CHECK_DISTANCE, Color.red);
            return false;
        }        
    }

    private bool IsThereAnyRobotsInFrontOfUs()
    {
        return IsThereAnyRobotsInFrontOfUs(out var hit);
    }

    public bool IsThereAnEnemy(out Robot enemy)
    {
        List<Robot> hits = new List<Robot>();
        IsThereAnyRobotsInFrontOfUs(out hits);
        foreach (var hit in hits)
        {
            if (hit._owner.PlayerID != _owner.PlayerID && hit.CurrentState != RobotState.Dying)
            {
                enemy = hit;
                return true;
            }
        }
        enemy = null;
        return false;
    }

    public void TakeHit(int hitValue)
    {
        Debug.Log("Hit:" + hitValue);
        lifeMeter -= hitValue;
        if(lifeMeter <= 0)
        {
            CurrentState = RobotState.Dying;           
        }
    }

    private void Kill()
    {
        StartCoroutine(Death_Routine());
    }

    public IEnumerator Death_Routine()
    {
        // Launch death
        _animator.SetTrigger("death");

        // If robot is currently displayed, play death sound
        if(Planet.IsCurrentPlanet(Planet))
        {
            AudioManager.Instance.PlayAudio(SoundType.RobotDeath);
        }

        // Wait for animation end
        yield return new WaitForSeconds(1.4f);

        // Spawn a derelict for the now deceased robot
        SpawnDerelict();

        // If this robot was a harvester, release the derelict it was carrying
        Harvester harvester = GetComponent<Harvester>();
        if(harvester)
        {
            harvester.OnDeath();
        }

        Owner.OnRobotDeath(this);
        Destroy(gameObject);
    }

    private void SpawnDerelict()
    {
        Derelict newDerelict = Instantiate(GameManager.Instance.DerelictPrefab, derelictLocation.position, derelictLocation.rotation);
    }

    private void Tracted()
    {
        StartCoroutine(Tracted_Routine());
    }

    public IEnumerator Tracted_Routine()
    {
        _animator.SetTrigger("idle");
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition; // Remove gravity
        int nbUps = 30;
        while (nbUps > 0) {
            transform.position += transform.up * 0.05f;
            nbUps--;
            yield return new WaitForSeconds(0.05f);
        }
        _owner.OnRobotRetrieve(this);           

        Destroy(gameObject);
    }

}
