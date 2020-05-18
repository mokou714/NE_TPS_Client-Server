
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseArrow : MonoBehaviour
{
    //setting
    public int damage;
    [Range(0,2f)] public float ShakingRadious;
    [Range(0, 10f)] public float ShakingSpeed;
    
    
    //status
    public bool isAiming;
    public bool isOnAir;
    
    //components
    private Rigidbody _rigidbody;
    [SerializeField] private MeshRenderer arrowMesh;
    
    //helper data
    private float _lifetime;
    private float _startTime;
    private Transform _originalLocalTransform;
    private float _defaultShakingSpeed;
    private float _defaultShakingRadius;
    private Transform _parent;
    
    //damage massage
    [SerializeField] protected DamageMessageManager damageMessageManager;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _defaultShakingSpeed = ShakingSpeed;
        _defaultShakingRadius = ShakingRadious;
        _parent = transform.parent;
    }


    protected virtual void Update()
    {
        UpdateDirection();
        CheckLifetime();
    }

    public void Initialize(Transform originalTransform)
    {
        _originalLocalTransform = originalTransform;
        transform.position = originalTransform.position;
        transform.rotation = originalTransform.rotation;
        
    }

    public void Activate()
    {
        arrowMesh.enabled = true;
        StartCoroutine(Shaking());
    }

    public void Deactivate()
    {
        arrowMesh.enabled = false;
        StopCoroutine(Shaking());
    }

    public void Shoot(Vector3 direction, float lifeTime, Vector3 shootForce)
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.LookRotation(direction);
        _startTime = Time.time;
        _lifetime = lifeTime;
        _rigidbody.AddForce(shootForce);
        isAiming = false;
        isOnAir = true;
        _rigidbody.isKinematic = false;
        transform.parent = null;
        Debug.Log("Shoot an arrow");
    }
    
    protected virtual void Reset()
    {
        if(isAiming) return;

        _rigidbody.velocity = Vector3.zero;
        isAiming = true;
        isOnAir = false;
        ShakingSpeed = _defaultShakingSpeed;
        ShakingRadious = _defaultShakingRadius;
        transform.position = _originalLocalTransform.position;
        transform.rotation = _originalLocalTransform.rotation;
        transform.parent = _parent;
        Deactivate();
        Debug.Log("Arrow Reset");
    }
    


    protected virtual void UpdateDirection()
    {
        if(isAiming) return;
        
        transform.rotation = Quaternion.LookRotation(_rigidbody.velocity.normalized);
    }

    protected virtual void CheckLifetime()
    {
        if (isAiming) return;
        
        if(Time.time > _startTime + _lifetime)
            Reset();
    }

    //apply skill effects then reset
    protected abstract void OnTriggerEnter(Collider other);
       

    private IEnumerator Shaking()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/(ShakingSpeed * 70f));

            if (isAiming)
            {
                _rigidbody.Sleep();
                var _radious = ShakingRadious / 50f;
                var xOffset = Random.Range(-_radious, _radious);
                var yOffset = Random.Range(-_radious, _radious);
                var zOffset = Random.Range(-_radious, _radious);
                transform.position = _originalLocalTransform.position + new Vector3(xOffset, yOffset, zOffset);
            }
        }
        
    }
}
