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
    
    //character status
    public Transform center;
    public Vector3 facingDir = Vector3.forward;
    public bool isMoving;
    public BodyPosture posture = BodyPosture.GUARD;
    
    //other components
    protected BaseAnimationController _animationController;
    protected BaseShootManager _shootManager;
    protected Rigidbody _rigidbody;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _animationController = GetComponent<BaseAnimationController>();
        _shootManager = GetComponent<BaseShootManager>();
        _rigidbody = GetComponent<Rigidbody>();
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
}
