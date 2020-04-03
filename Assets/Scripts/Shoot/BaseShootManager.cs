using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseShootManager : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform bulletInitTransform;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int backUpAmmo;
    [SerializeField] protected float fireCDTime;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletLifetime;
    [SerializeField] protected int layerMask;
    [SerializeField] protected bool burstMode;

    //helper data
    protected bool _isShooting = false;
    protected bool _isReloading = false;
    protected Bullet[] _bulletPool;
    
    //other components
    protected BaseCharacterController _characterController;
    protected BaseAnimationController _animationController;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _characterController = GetComponent<BaseCharacterController>();
        _animationController = GetComponent<BaseAnimationController>();
        InitBulletPool();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Shoot();
        Reload();
        SwitchMode();
    }
    
    public virtual void FinishReloading()
    {
        _isReloading = false;
        currentAmmo = maxAmmo;
    }
    public abstract Vector3 GetAimingDirection();
    public abstract Vector3 GetBulletDirection();

    protected virtual void Shoot()
    {
        //check player state
        if(_characterController.posture == BodyPosture.GUARD) return;
        //check shooting state
        if( _isShooting || _isReloading) return;
        
        _isShooting = true;
        _animationController.Shoot(burstMode);
        StartCoroutine(ShootBullet(burstMode ? 4 : 1));
    }

    protected virtual void Reload()
    {
        if (_isReloading || _isShooting) return;
        _isReloading = true;
        _animationController.Reload();
    }

    protected virtual void SwitchMode()
    {
        if (_characterController.posture == BodyPosture.GUARD) return;
        burstMode = !burstMode;
    }
    protected virtual void InitBulletPool()
    {
        _bulletPool = new Bullet[maxAmmo];
        for (int i = 0; i < maxAmmo; ++i)
        {
            GameObject obj = Instantiate(bulletPrefab, null);
            _bulletPool[i] = obj.GetComponent<Bullet>();
            _bulletPool[i].Initialize(bulletInitTransform,null);
        }
    }
    
     protected virtual IEnumerator ShootBullet(int number)
    {
        int _number = currentAmmo - number < 0 ? currentAmmo : number;
        
        for (int i = 0; i < _number; ++i)
        {
            _bulletPool[currentAmmo-1].Go(GetBulletDirection() * bulletSpeed, bulletLifetime);
            --currentAmmo;
            yield return new WaitForSeconds(fireCDTime);
        }

        _isShooting = false;
    }
    
}
