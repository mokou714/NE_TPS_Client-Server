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

    //AI status
    [SerializeField] private AIState _aiState;
    [SerializeField] private PlayerController _targetPlayer;
    [SerializeField] private Vector3 aimingDir;
    
    //helper data
    private float vigilantStartTime;

    //components
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        DetectPlayer();
        ChasePlayer();
        LoseTargetPlayer();
        StayingVigilant();
        UpdateBodyYAxisRotation();
        UpdateBodyXAxisRotation();
    }

    protected override void Move()
    {
        //todo
    }

    private void DetectPlayer()
    {
        if (_aiState != AIState.DETECT) return;

        foreach (var player in players)
        {
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

            if (Vector3.Angle(facingHoriDir, horiDir) > viewHorizontalRange / 2
                ||
                Vector3.Angle(facingVertDir, vertDir) > viewVerticalRange / 2)
                return;

            _aiState = AIState.CHASE;
            facingDir = playerDir;
            _targetPlayer = player;
            SwitchBodyPosture();
            Debug.Log("Found Player");
        }
    }

    private void ChasePlayer()
    {
        if (_aiState != AIState.CHASE) return;
        if (_targetPlayer == null) throw new Exception("No Target Player");

        var distance = (_targetPlayer.Center.position - Center.position).magnitude;

        if (distance < attackDistance)
        {
            _aiState = AIState.ATTACK;
            _agent.isStopped = true;
            _animationController.SetWalk(false, Direction.FORWARD);
        }
        else
        {
            _agent.SetDestination(_targetPlayer.transform.position);
            _animationController.SetWalk(true, Direction.FORWARD);
            _agent.isStopped = false;
        }
    }

    private void StayingVigilant()
    {
        if (_aiState != AIState.VIGILENT) return;
        if (Time.time - vigilantStartTime > vigilantTime)
        {
            _aiState = AIState.DETECT;
            SwitchBodyPosture();
        }
        
        //to add some behaviours in vigilant state
    }

    private void LoseTargetPlayer()
    {
        if (_aiState == AIState.DETECT || _aiState == AIState.VIGILENT) return;
        
        if (_targetPlayer == null) throw new Exception("No Target Player");

        var distance = (_targetPlayer.Center.position - Center.position).magnitude;
        
        //to detect state
        if (distance > viewDistance)
        {
            _aiState = AIState.VIGILENT;
            _targetPlayer = null;
            _animationController.SetWalk(false, Direction.FORWARD);
            _agent.isStopped = true;
            vigilantStartTime = Time.time;
            Debug.Log("Lose Player");
        }
        //to chase state
        else if(distance > attackDistance)
        {
            _aiState = AIState.CHASE;
        }
    }

    private void SwitchBodyPosture()
    {
        posture = posture == BodyPosture.COMBAT ? BodyPosture.GUARD : BodyPosture.COMBAT;
        _animationController.SwitchCombatState();
    }

    protected override void UpdateBodyYAxisRotation()
    {
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
    
    public Transform EyeTransform => eyeTransform;
    public PlayerController TargetPlayer => _targetPlayer;
    public AIState AiState => _aiState;
}
