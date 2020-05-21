using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : BaseCharacterController
{
    //player properties
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float jumpForce;
    
    //player status
    public bool isJumping = false;

    //other components
    [SerializeField] private TPSCameraController cameraController;
    
    protected override void Start()
    {
        base.Start();
        _animationController = GetComponent<PlayerAnimationController>();
        _shootManager = GetComponent<PlayerShootManager>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        UpdateAnimState();
    }

    private void FixedUpdate()
    {
        Jump();
    }

    protected override void Move()
    {
        if (!_status.isAlive) return;
        
        var movingDir = Vector3.zero;

        var isWalking = !Input.GetKey(KeyCode.LeftShift);

        //forward
        if (Input.GetKey(KeyCode.W))
        {
            movingDir += facingDir;
            _animationController.SetWalk(isWalking,Direction.FORWARD);
            _animationController.SetRun(!isWalking,Direction.FORWARD);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _animationController.SetWalk(false,Direction.FORWARD);
            _animationController.SetRun(false,Direction.FORWARD);
        }
        //back
        if (Input.GetKey(KeyCode.S))
        {
            movingDir += -facingDir;
            _animationController.SetWalk(isWalking,Direction.BACK);
            _animationController.SetRun(!isWalking,Direction.BACK);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _animationController.SetWalk(false,Direction.BACK);
            _animationController.SetRun(false,Direction.BACK);
        }
        //left
        if (Input.GetKey(KeyCode.A))
        {
            movingDir += Quaternion.AngleAxis(-90, Vector3.up) * facingDir;
            _animationController.SetWalk(isWalking,Direction.LEFT);
            _animationController.SetRun(!isWalking,Direction.LEFT);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _animationController.SetWalk(false,Direction.LEFT);
            _animationController.SetRun(false,Direction.LEFT);
        }
        //right
        if (Input.GetKey(KeyCode.D))
        {
            movingDir += Quaternion.AngleAxis(90, Vector3.up) * facingDir;
            _animationController.SetWalk(isWalking,Direction.RIGHT);
            _animationController.SetRun(!isWalking,Direction.RIGHT);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _animationController.SetWalk(false,Direction.RIGHT);
            _animationController.SetRun(false,Direction.RIGHT);
        }
        
        //check if got any moving keys
        if (movingDir != Vector3.zero)
        {
            isMoving = true;
            var speed = isWalking ? walkSpeed : runSpeed;
            transform.Translate(Time.deltaTime * speed * movingDir, Space.World);

            //play footstep audio
            if(isJumping) return;
            var _clip = isWalking ? footstepWalk : footstepRun;
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
        else
        {
            isMoving = false;
            audioSource.Stop();
        }

    }
    
    void Jump()
    {
        if (isJumping) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(jumpForce*50*Vector3.up);
            ((PlayerAnimationController)_animationController).SetJump(true);
            isJumping = true;
            audioSource.Stop();
        }
    }
    
    void UpdateAnimState()
    {
        if (isMoving) return; //disable switching posture while moving
        if(cameraController.IsSwitchingAiming()) return; //disable switching posture when switching

        var _posture = posture;
        if (Input.GetKeyDown(KeyCode.C))
        {
            posture = posture == BodyPosture.CROUCH ? BodyPosture.GUARD : BodyPosture.CROUCH;
            _animationController.SwitchCrouchState();
            if (_posture == BodyPosture.GUARD || posture == BodyPosture.GUARD)
            {
                cameraController.SwitchAimingMode();
                SwitchCrosshairDisplay();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            posture = posture == BodyPosture.COMBAT ? BodyPosture.GUARD : BodyPosture.COMBAT;
            _animationController.SwitchCombatState();
            if (_posture == BodyPosture.GUARD || posture == BodyPosture.GUARD)
            {
                cameraController.SwitchAimingMode();
                SwitchCrosshairDisplay();
            }
        }
    }
    protected override void UpdateBodyYAxisRotation()
    {
        if (!_status.isAlive) return;
        // Vector3 camPivotRotation = cameraController.pivot.transform.localEulerAngles;
        // camPivotRotation.x = camPivotRotation.z = 0; //only keep Y axis rotation
        // body.transform.localRotation = Quaternion.Euler(camPivotRotation);
        // _facingDir = Quaternion.Euler(camPivotRotation)*Vector3.forward;
        
        //rotate body horizontally
        var shootDirection = _shootManager.GetAimingDirection();
        var aimingRotation = Quaternion.LookRotation(shootDirection);
        var yAxisRotation = aimingRotation;
        yAxisRotation.x = yAxisRotation.z = 0;
        body.transform.rotation = yAxisRotation;
        facingDir = yAxisRotation*Vector3.forward;
        //Ray ray = new Ray(transform.position,facingDir);
        //Debug.DrawRay(ray.origin,ray.direction * 10, Color.red);
        
    }
    protected override void UpdateBodyXAxisRotation()
    {
        if (!_status.isAlive) return;
        
        //rotate body(spine) vertically, override spine animation
        if (posture == BodyPosture.COMBAT)
        {
            
            var shootDirection = _shootManager.GetAimingDirection();
            var zDir = Vector3.Cross(shootDirection, Vector3.up);
            var zLookAtDir = zDir + shootDirection;
            var yDir = Vector3.Cross(zLookAtDir, shootDirection);
            var toShootDirectionRotation = Quaternion.LookRotation(zLookAtDir, shootDirection);
            spine.transform.rotation = toShootDirectionRotation;
        }
        else if (posture == BodyPosture.CROUCH)
        {
            var shootDirection = _shootManager.GetAimingDirection();
            var zDir = Vector3.Cross(shootDirection, Vector3.up);
            var zLookAtDir = zDir + shootDirection;
            var yDir = Vector3.Cross(zLookAtDir, shootDirection);
            var toShootDirectionRotation = Quaternion.LookRotation(zLookAtDir, shootDirection);
            spine.transform.rotation = toShootDirectionRotation;
        }


        // Vector3 shootDirection = _shootManager.GetShootDirection(); 
        // Vector3 zLookAtDir = Vector3.Cross(shootDirection, Vector3.up);
        // Vector3 upDir = Vector3.Cross(zLookAtDir, shootDirection);
        // Quaternion toShootDirectionRotation = Quaternion.LookRotation(zLookAtDir, shootDirection);
        
        // rotate arms
        // Vector3 _rightArmOriginalRotation = rightArm.transform.localEulerAngles;
        // Vector3 xDir = Vector3.Cross(shootDirection, Vector3.up);
        // Vector3 zDir = Vector3.Cross(shootDirection,xDir);
        // Quaternion toShootDirectionRotation = Quaternion.LookRotation(zDir,-shootDirection);
        // rightArm.transform.rotation = toShootDirectionRotation;
        // Vector3 leftArmNewRotation = leftArm.transform.localEulerAngles;
        // Vector3 relativeRotation = rightArm.transform.localEulerAngles - _rightArmOriginalRotation;
        // relativeRotation.x = -relativeRotation.x;
        // relativeRotation.y =  relativeRotation.y;
        // relativeRotation.z = -relativeRotation.z;
        // leftArmNewRotation += relativeRotation;
        // leftArm.transform.localRotation = Quaternion.Euler(leftArmNewRotation);
        
        //Quaternion localRotation = cameraController.transform.localRotation;
        //localRotation.z = -localRotation.x;
        //localRotation.x = 0;

        // Vector3 _rotation = rightArm.transform.rotation.eulerAngles;
        // Vector3 localXaxis = Vector3.Cross(shootDirection, Vector3.up);
        // Quaternion aimmingRotation = Quaternion.FromToRotation(_facingDir, shootDirection);
        
        //aimmingRotation.y = aimmingRotation.x = 0;
        //leftArm.transform.rotation *= aimmingRotation;
        //aimmingRotation.x = aimmingRotation.x;
        //rightArm.transform.rotation *= aimmingRotation;

    }

    private void SwitchCrosshairDisplay()
    {
        crosshair.SetActive(!crosshair.activeSelf);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain") && isJumping)
        {
            isJumping = false;
            ((PlayerAnimationController)_animationController).SetJump(false);
        }
    }
}
