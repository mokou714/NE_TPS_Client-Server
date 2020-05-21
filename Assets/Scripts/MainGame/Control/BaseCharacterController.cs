using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseCharacterController : MonoBehaviour
{
    //character properties
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected GameObject body;
    [SerializeField] protected GameObject spine;
    [SerializeField] protected Transform center;
    
    //character status
    public Vector3 facingDir;
    public bool isMoving;
    public BodyPosture posture = BodyPosture.GUARD;
    
    //status class
    protected CharacterStatus _status;
    
    //other components
    protected BaseAnimationController _animationController;
    protected BaseShootManager _shootManager;
    protected Rigidbody _rigidbody;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip footstepWalk;
    [SerializeField] protected AudioClip footstepRun;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _animationController = GetComponent<BaseAnimationController>();
        _shootManager = GetComponent<BaseShootManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _status = GetComponent<CharacterStatus>();
        facingDir =  transform.rotation * Vector3.forward;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateBodyYAxisRotation();
        Move();
    }
    
    protected virtual void LateUpdate()
    {
        //override spine animation
        UpdateBodyXAxisRotation();
    }
    
    protected abstract void Move();
    //change look at direction
    protected abstract void UpdateBodyYAxisRotation();
    //called inside LateUpdate(), override Body animation
    protected abstract void UpdateBodyXAxisRotation();

    //getters, setters
    public Transform Center => center;

    public float RunSpeed
    {
        get => runSpeed;
        set => runSpeed = value;
    }

    public float WalkSpeed
    {
        get => walkSpeed;
        set => walkSpeed = value;
    }
    
}
