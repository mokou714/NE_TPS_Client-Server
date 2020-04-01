using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : BaseAnimationController
{
    //currently player doesn't have its own animations

    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<PlayerController>();
        _shootManager = GetComponent<PlayerShootManager>();
    }
    
    public void SetJump(bool isJumping)
    {
        _animator.SetBool("jumping",isJumping);
    }
}
