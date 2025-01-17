﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : BaseCharacterController
{
    //AI settings
    [SerializeField] private float viewVerticalRange;
    [SerializeField] private float viewHorizontalRange;
    [SerializeField] private float viewDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float vigilantTime;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private PlayerController[] players;

    //helper data
    private float vigilantStartTime;
    private PlayerController _targetPlayer;

    //components
    private NavMeshAgent _agent;
    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _status = GetComponent<AIStatus>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
        ChasePlayer();
        DetectLosingTargetPlayer();
        StayingVigilant();
        UpdateBodyYAxisRotation();
        UpdateBodyXAxisRotation();
        
        //apply actions in next frame, if finding the player
        DetectPlayer();
        
        _Debug();
        
    }

    //called per frame
    protected override void Move()
    {
    }

    private void DetectPlayer()
    {
        if (!_status.isAlive || _status.isStunned) return;
        if ( ((AIStatus)_status).aiState != AIState.DETECT &&  ((AIStatus)_status).aiState != AIState.VIGILANT ) return;

        if (FindPlayer())
        {
            SwitchBodyPosture(true);
            ((AIStatus)_status).aiState = AIState.CHASE;
            Debug.Log("Found Player");
        }
    }

    private void ChasePlayer()
    {
        if (!_status.isAlive || _status.isStunned) return;
        if (((AIStatus)_status).aiState != AIState.CHASE) return;
        if (_targetPlayer == null) throw new Exception("No Target Player");

        var distance = (_targetPlayer.Center.position - Center.position).magnitude;

        if (distance < attackDistance)
        {
            ((AIStatus)_status).aiState = AIState.ATTACK;
            StopMoving();
        }
        else
        {
            StartMoving();
        }
    }

    private void StayingVigilant()
    {
        if (!_status.isAlive || _status.isStunned) return;
        if (((AIStatus)_status).aiState != AIState.VIGILANT) return;
        if (Time.time - vigilantStartTime > vigilantTime)
        {
            ((AIStatus)_status).aiState = AIState.DETECT;
            SwitchBodyPosture(false);
        }
        
        //to add some behaviours in vigilant state
    }

    private void DetectLosingTargetPlayer()
    {
        if (!_status.isAlive || _status.isStunned) return;
        if (((AIStatus)_status).aiState == AIState.DETECT || ((AIStatus)_status).aiState == AIState.VIGILANT) return;
        
        if (_targetPlayer == null) throw new Exception("No Target Player");

        var distance = (_targetPlayer.Center.position - Center.position).magnitude;
        
        //lose player -> Vigilant state
        if (distance > viewDistance || !_targetPlayer.GetComponent<CharacterStatus>().isAlive)
        {
            ((AIStatus)_status).aiState = AIState.VIGILANT;
            vigilantStartTime = Time.time;
            LosePlayer();
            StopMoving();
            Debug.Log("Lose Player");
            
        }
        //play out of attack range -> chase state
        else if(distance > attackDistance)
        {
            ((AIStatus)_status).aiState = AIState.CHASE;
        }
    }

    //helpers
    private bool FindPlayer()
    {
        foreach (var player in players)
        {
            if (!player.GetComponent<CharacterStatus>().isAlive){
                continue; //player dead, go next
            }
            
            //check view distance(use center position)
            var distance = (player.Center.position - Center.position).magnitude;
            if (distance > viewDistance) continue;

            //check view angles(use eye position)
            var playerDir = player.Center.position - eyeTransform.position;
            var vertDir = playerDir;
            var horiDir = playerDir;
            vertDir.x = vertDir.z = 0;
            horiDir.y = 0;

            var facingVertDir = facingDir;
            var facingHoriDir = facingDir;
            facingVertDir.x = facingVertDir.z = 0;
            facingHoriDir.y = 0;
            
            if (Vector3.Angle(facingHoriDir, horiDir) < viewHorizontalRange / 2
                && Vector3.Angle(facingVertDir, vertDir) < viewVerticalRange / 2)
            {
                _targetPlayer = player;
                facingDir = playerDir;
                return true;
            }
           
        }

        return false;
    }

    private void _Debug()
    {
        Debug.DrawRay(eyeTransform.position,facingDir*30,Color.red);
        foreach (var player in players)
        {
            Debug.DrawLine(eyeTransform.position, player.Center.transform.position);
        }
    }

    protected override void UpdateBodyYAxisRotation()
    {
        if (!_status.isAlive || _status.isStunned) return;
        if (_targetPlayer == null) return;

        facingDir = (_targetPlayer.Center.position - Center.position).normalized;
        facingDir.y = 0;
        
        var aimingRotation = Quaternion.LookRotation(facingDir);
        body.transform.rotation = aimingRotation;
    }

    protected override void UpdateBodyXAxisRotation()
    {
        //throw new System.NotImplementedException();
    }
    
    public void SwitchBodyPosture(bool toCombat)
    {
        //not switch if same states
        if ( (posture == BodyPosture.COMBAT) ==  toCombat) return;
        posture = posture == BodyPosture.COMBAT ? BodyPosture.GUARD : BodyPosture.COMBAT;
        _animationController.SwitchCombatState();
    }

    public void StartMoving()
    {
        //update destination
        _agent.SetDestination(_targetPlayer.transform.position);

        _agent.isStopped = false;
            
        if (_status.isWalking)
        {
            _animationController.SetWalk(true, Direction.FORWARD);
            _agent.speed = walkSpeed;
        }
        else
        {
            _animationController.SetRun(true, Direction.FORWARD);
            _agent.speed = runSpeed;
        }
            
            
        var _clip =  _status.isWalking ? footstepWalk : footstepRun;
        //walk <-> run
        if (audioSource.clip != _clip)
        {
            audioSource.Stop();
            audioSource.clip = _clip;
            audioSource.Play();
        }
        //stop -> move
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMoving()
    { 
        //simply stop moving, not changing other attributes
        if(_status.isWalking)
            _animationController.SetWalk(false, Direction.FORWARD);
        else
            _animationController.SetRun(false, Direction.FORWARD);
        Debug.Log("Enemy stopped");
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        audioSource.Stop();
    }

    public void LosePlayer()
    {
        _targetPlayer = null;
    }
    
    public Transform EyeTransform => eyeTransform;
    public PlayerController TargetPlayer => _targetPlayer;
}
