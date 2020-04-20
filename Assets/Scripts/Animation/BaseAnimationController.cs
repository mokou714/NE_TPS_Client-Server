using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum Direction
{
    FORWARD,BACK,LEFT,RIGHT
}

public class BaseAnimationController : MonoBehaviour
{
    protected Animator _animator;
    protected Random _random;

    //other components
    protected BaseCharacterController _characterController;
    protected BaseShootManager _shootManager;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<BaseCharacterController>();
        _shootManager = GetComponent<BaseShootManager>();
        _random = new Random();
    }

    public void KnifeAttack()
    {
        var rnd = _random.Next(0,2);
        if (_characterController.posture == BodyPosture.CROUCH)
        {
            _animator.SetTrigger(rnd==0? "crouch_attack_1":"crouch_attack_2");
        }
        else if (_characterController.posture == BodyPosture.COMBAT)
        {
            _animator.SetTrigger(rnd==0? "combat_attack_1":"combat_attack_2");
        }

    }

    public void Shoot(bool burstMode)
    {
        if (_characterController.posture == BodyPosture.CROUCH)
        {
            _animator.SetTrigger(burstMode? "crouch_burst_shoot":"crouch_shoot");
        }
        else if (_characterController.posture == BodyPosture.COMBAT)
        {
            _animator.SetTrigger(burstMode? "combat_burst_shoot":"combat_shoot");
        }
    }

    public void Reload()
    {
        _animator.SetTrigger("reload");
        StartCoroutine(CheckReloading());
    }

    public void TakeDamage()
    {
        _animator.SetTrigger("take_damage");
    }

    public void SetWalk(bool isWalking, Direction direction )
    {
        if (_characterController.posture == BodyPosture.COMBAT)
        {
            if(direction == Direction.FORWARD)
                _animator.SetBool("combat_walk_forward",isWalking);
            else if(direction == Direction.BACK)
                _animator.SetBool("combat_walk_back",isWalking);
            else if(direction == Direction.LEFT)
                _animator.SetBool("combat_walk_left",isWalking);
            else if(direction == Direction.RIGHT)
                _animator.SetBool("combat_walk_right",isWalking);
            else
                throw new SystemException("Unknown Moving Direction");
            
        }
        else if (_characterController.posture == BodyPosture.CROUCH)
        {
            if(direction == Direction.FORWARD)
                _animator.SetBool("crouch_walk_forward",isWalking);
            else if(direction == Direction.BACK)
                _animator.SetBool("crouch_walk_back",isWalking);
            else if(direction == Direction.LEFT)
                _animator.SetBool("crouch_walk_left",isWalking);
            else if(direction == Direction.RIGHT)
                _animator.SetBool("crouch_walk_right",isWalking);
            else
                throw new SystemException("Unknown Moving Direction");
            
        }
        else if (_characterController.posture == BodyPosture.GUARD)
        {
            _animator.SetBool("guard_walk",isWalking);
        }
    }
    
    public void SetRun(bool isRunning, Direction direction)
    {
       
        if (_characterController.posture == BodyPosture.COMBAT)
        {
            if(direction == Direction.FORWARD)
                _animator.SetBool("combat_run_forward",isRunning);
            else if(direction == Direction.BACK)
                _animator.SetBool("combat_run_back",isRunning);
            else if(direction == Direction.LEFT)
                _animator.SetBool("combat_run_left",isRunning);
            else if(direction == Direction.RIGHT)
                _animator.SetBool("combat_run_right",isRunning);
            else
                throw new SystemException("Unknown Moving Direction");
            
            _animator.SetBool("running",isRunning);
        }
        else if (_characterController.posture == BodyPosture.GUARD)
        {
            _animator.SetBool("running",isRunning);
        }
        //no running for crouch
        else if(_characterController.posture == BodyPosture.CROUCH && isRunning)
        {
            SetWalk(true,direction);
        }
    }

    

    public void SwitchCrouchState()
    {
        _animator.SetTrigger("crouch");
    }

    public void SwitchCombatState()
    {
        _animator.SetTrigger("combat");
    }


    private IEnumerator CheckReloading()
    {
        yield return new WaitUntil(
            ()=>
                _animator.GetAnimatorTransitionInfo(1).IsName("Shoot.reload -> Shoot.NULL")
        );
        _shootManager.FinishReloading();
    }
}
