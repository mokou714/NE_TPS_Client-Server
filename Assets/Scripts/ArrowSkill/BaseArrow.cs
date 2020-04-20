using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseArrow : MonoBehaviour
{
    
    //components
    private Rigidbody _rigidbody;
    
    //helper data
    private float _lifetime;
    private float _startTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateDirection();
        CheckLifetime();
    }

    public void Initialize(Vector3 direction, float lifeTime, Vector3 spawnOrigin, Vector3 shootForce)
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.LookRotation(direction);
        _startTime = Time.time;
        _lifetime = lifeTime;
        transform.position = spawnOrigin;
        _rigidbody.AddForce(shootForce);
        Debug.Log("Arrow initialized");
    }
    
    public void Reset()
    {
        gameObject.SetActive(false);
        _rigidbody.velocity = Vector3.zero;
        Debug.Log("Arrow Reset");
    }
    

    protected virtual void UpdateDirection()
    {
        transform.rotation = Quaternion.LookRotation(_rigidbody.velocity.normalized);
    }

    protected virtual void CheckLifetime()
    {
        Debug.Log(Time.time);
        Debug.Log(_startTime);
        Debug.Log(_lifetime);
        
        if(Time.time > _startTime + _lifetime)
            Reset();
    }
    
    

    protected abstract void OnTriggerEnter(Collider other);

}
