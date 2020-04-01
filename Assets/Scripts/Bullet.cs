using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _velocity;
    private float _lifetime;
    private bool _isMoving;
    private float _startTime;
    private Transform _spawnTransform;
    private DamageMessageManager _damageMessageManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
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

    public void Initialize(Transform spawnTransform, DamageMessageManager damageMessageManager)
    {
        _spawnTransform = spawnTransform;
        _damageMessageManager = damageMessageManager;
    }
    

    public void Go(Vector3 velocity, float lifetime)
    {
        _velocity = velocity;
        _lifetime = lifetime;
        _isMoving = true;
        _startTime = Time.time;
        transform.position = _spawnTransform.position;
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            _damageMessageManager.ShowMessage(other.gameObject.GetComponent<EnemyAI>().center.position);
            Reset();
        }
    }
}
