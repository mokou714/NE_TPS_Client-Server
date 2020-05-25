using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _velocity;
    private float _lifetime;
    private bool _isMoving;
    private float _startTime;
    private Transform _spawnTransform;
    private DamageMessageManager _damageMessageManager;
    private int _damage;
    
    void Update()
    {
        Move();
        CheckLifetime();
    }

    void Move()
    {
        if (!_isMoving) return;
        transform.Translate(_velocity*Time.deltaTime,Space.World);
    }

    //initialize all bullets when game starts
    public void Initialize(int damage, Transform spawnTransform, DamageMessageManager damageMessageManager)
    {
        _damage = damage;
        _spawnTransform = spawnTransform;
        _damageMessageManager = damageMessageManager;
    }
    
    //called when shoot this bullet
    public void Go(Vector3 velocity, float lifetime)
    {
        _velocity = velocity;
        _lifetime = lifetime;
        _isMoving = true;
        _startTime = Time.time;
        transform.position = _spawnTransform.position;
        transform.rotation = Quaternion.LookRotation(velocity);
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        _isMoving = false;
        gameObject.SetActive(false);
    }

    void CheckLifetime()
    {
        if(Time.time>_startTime+_lifetime)
            Reset();
    }


    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "Enemy":
            case "Player":
                var characterController = other.gameObject.GetComponent<BaseCharacterController>();
                var healthManager = other.gameObject.GetComponent<BaseHealthManager>();
                //check if dealt damage successfully, then show message
                if (healthManager.DealDamage(_damage) && _damageMessageManager != null)
                {
                    _damageMessageManager.ShowMessage(_damage, characterController.Center.position);
                }
                Reset(); 
                break;
            case "Balloon":
                other.gameObject.GetComponent<Balloon>().Dropdown();
                break;
            default:
                Reset(); 
                return;
        }
    }
}
