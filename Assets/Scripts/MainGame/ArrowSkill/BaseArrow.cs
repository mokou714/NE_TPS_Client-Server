
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
    [SerializeField] private TrailRenderer trail;
    
    //helper data
    private float _lifetime;
    private float _startTime;
    private Transform _originTransform;
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
        CheckLifetime();
    }

    protected virtual void LateUpdate()
    {
        UpdateDirection();
    }

    public void Initialize(Transform originTransform)
    {
        _originTransform = originTransform;
        transform.position = originTransform.position;
        transform.rotation = originTransform.rotation;
        
    }
    
    //display when switching to this arrow
    public void OnEnable()
    {
        isAiming = true;
        StartCoroutine(Shaking());
    }



    public void Shoot(float lifeTime, Vector3 shootForce)
    {
        gameObject.SetActive(true);
        _startTime = Time.time;
        _lifetime = lifeTime;
       
        transform.rotation = Quaternion.LookRotation(shootForce.normalized); //look at force direction
        _rigidbody.AddForce(shootForce);
        
        
        isAiming = false;
        //isOnAir = true;
        _rigidbody.isKinematic = false;
        transform.parent = null;
        Debug.Log("Shoot an arrow");
        StartCoroutine(DelayTrailRendering(0.02f)); //velocity dir is downwards in first a few frames
   
    }
    
    public virtual void Reset()
    {
        _rigidbody.velocity = Vector3.zero;
        isAiming = false;
        isOnAir = false;
        ShakingSpeed = _defaultShakingSpeed;
        ShakingRadious = _defaultShakingRadius;
        transform.parent = _parent;
        transform.position = _originTransform.position;
        transform.rotation = _originTransform.rotation;
        trail.Clear();
        trail.enabled = false;
        Debug.Log("Arrow Reset");
    }
    
    protected virtual void UpdateDirection()
    {
        if(isAiming || !isOnAir) return;
        transform.rotation = Quaternion.LookRotation(_rigidbody.velocity.normalized);

    }

    protected virtual void CheckLifetime()
    {
        if(isAiming || !isOnAir) return;

        if (Time.time > _startTime + _lifetime)
        {
            Reset();
            gameObject.SetActive(false);
        }
    }

    //apply skill effects then reset
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balloon"))
        {
            other.gameObject.GetComponent<Balloon>().Dropdown();
        }
        Reset();
        gameObject.SetActive(false);
    }
       

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
                transform.position = _originTransform.position + new Vector3(xOffset, yOffset, zOffset);
            }
        }
    }


    private IEnumerator DelayTrailRendering(float time)
    {
        yield return new WaitForSeconds(time);
        trail.enabled = true;
        isOnAir = true; // also delay updateDirection()
    }
}
