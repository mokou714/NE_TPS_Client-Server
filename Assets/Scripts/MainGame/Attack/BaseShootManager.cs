using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class BaseShootManager : MonoBehaviour
{
    //status data
    public int maxAmmo;
    public int currentAmmo;
    public int backUpAmmo;
    
    //properties
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform bulletInitTransform;
    [SerializeField] protected float fireCD;
    [SerializeField] protected float burstFireCD;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletLifetime;
    [SerializeField] protected int layerMask;
    [SerializeField] protected bool burstMode;
    [SerializeField] protected int bulletMinDamage;
    [SerializeField] protected int bulletMaxDamage;

    //helper vars
    protected bool _isShooting = false;
    protected bool _isReloading = false;
    [SerializeField]protected Bullet[] _bulletPool;
    
    //other components
    protected BaseCharacterController characterController;
    protected BaseAnimationController animationController;
    protected EffectManager effectManager;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip shootSFX;
    [SerializeField] protected AudioClip reloadSFX;
    //status
    protected CharacterStatus _status;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _status = GetComponent<CharacterStatus>();
        characterController = GetComponent<BaseCharacterController>();
        animationController = GetComponent<BaseAnimationController>();
        effectManager = GetComponent<EffectManager>();
        InitBulletPool();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //SwitchMode();
        Shoot();
        Reload();
    }

    protected virtual void LateUpdate()
    {
        SwitchMode();
    }

    public virtual void FinishReloading()
    {
        _isReloading = false;
        var ammoToLoad = maxAmmo - currentAmmo;
        if (backUpAmmo > ammoToLoad)
        {
            backUpAmmo -= ammoToLoad;
            currentAmmo += ammoToLoad;
        }
        else
        {
            currentAmmo += backUpAmmo;
            backUpAmmo = 0;
        }
    }
    public abstract Vector3 GetAimingDirection();
    public abstract Vector3 GetBulletDirection();

    protected virtual void Shoot()
    {
        //check alive
        if(!_status.isAlive) return;
        //check base character state
        if(characterController.posture == BodyPosture.GUARD) return;
        //check shooting state
        if( _isShooting || _isReloading) return;
        
        _isShooting = true;
        StartCoroutine(ShootBullet(burstMode ? 4 : 1));
    }

    protected virtual void Reload()
    {
        //check alive
        if(!_status.isAlive) return;
        if (_isReloading || _isShooting || backUpAmmo == 0) return;
        _isReloading = true;
        audioSource.PlayOneShot(reloadSFX);
        animationController.Reload();
    }

    protected virtual void SwitchMode()
    {
        //check alive
        if(!_status.isAlive) return;
        if (characterController.posture == BodyPosture.GUARD) return;
        burstMode = !burstMode;
    }
    protected virtual void InitBulletPool()
    {
        _bulletPool = new Bullet[maxAmmo];
        for (var i = 0; i < maxAmmo; ++i)
        {
            GameObject obj = Instantiate(bulletPrefab, null);
            _bulletPool[i] = obj.GetComponent<Bullet>();
            _bulletPool[i].Initialize(Random.Range(bulletMinDamage, bulletMaxDamage+1), bulletInitTransform,null);
        }
    }
    
     protected virtual IEnumerator ShootBullet(int number)
    {
        var _number = currentAmmo - number < 0 ? currentAmmo : number;
        
        for (var i = 0; i < _number; ++i)
        {
            _bulletPool[currentAmmo-1].Go(GetBulletDirection() * bulletSpeed, bulletLifetime);
            --currentAmmo;
            effectManager.GunFire();
            audioSource.PlayOneShot(shootSFX);
            animationController.Shoot(burstMode);
            yield return new WaitForSeconds(burstMode? burstFireCD:fireCD);
        }

        _isShooting = false;
    }
    
}
