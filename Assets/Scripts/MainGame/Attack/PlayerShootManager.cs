﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;


public class PlayerShootManager : BaseShootManager
{
    
    //UI
    [SerializeField] private BulletIconAnim bulletIconAnim;
    [SerializeField] private DamageMessageManager damageMessageManager;
    [SerializeField] private Text backupAmmoUI;
    [SerializeField] private Text currentAmmoUI;

    //helper data
    private Vector3 _mousePositionOffset;
    private Vector3 _screenCenter;

    //other components
    [SerializeField] private TPSCameraController _cameraController;
    private ArrowSkillManager _arrowSkillManager;

    protected override void LateUpdate()
    {
        base.LateUpdate();
        UpdateScreenCenterOnSizeChanged();
    }
    
    protected override void Start()
    {
        base.Start();
        characterController = GetComponent<PlayerController>();
        animationController = GetComponent<PlayerAnimationController>();
        _arrowSkillManager = GetComponent<ArrowSkillManager>();
        currentAmmo = maxAmmo;
        currentAmmoUI.text = maxAmmo.ToString();
        backupAmmoUI.text = backUpAmmo.ToString();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        _screenCenter = new Vector3(Screen.width/2f,Screen.height/2f,0f);
    }

    public override Vector3 GetAimingDirection()
    {
        var cursorRay = _cameraController.getCamera().ScreenPointToRay(_screenCenter);
        return cursorRay.direction;
    }

    public override Vector3 GetBulletDirection()
    {
        var cursorRay = _cameraController.getCamera().ScreenPointToRay(_screenCenter);
        return 
            Physics.Raycast(cursorRay, out var hitInfo, 100f, 1 << layerMask) ?
            (hitInfo.point - bulletInitTransform.position).normalized : //direction from bullet to hit point
            cursorRay.direction; //aiming ray not hit, return aiming direction
    }

    public override void FinishReloading()
    {
        base.FinishReloading();
        currentAmmoUI.text = currentAmmo.ToString();
        backupAmmoUI.text = backUpAmmo.ToString();
    }

    protected override void Shoot()
    {
        //not automatic reloading for players
        if (currentAmmo == 0) return;
        //check player controller status
        //if (((PlayerController) characterController).isJumping) return;
        //check arrow aiming state
        if (_arrowSkillManager.inAimingState) return;

        if (Input.GetMouseButtonDown(0) && !burstMode || (Input.GetMouseButton(0) && burstMode))
        {
            base.Shoot();
        }
    }

    protected override void SwitchMode()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            base.SwitchMode();
            bulletIconAnim.SwitchMode(burstMode);
        }
    }

    protected override void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            base.Reload();
        }
    }

    protected override void InitBulletPool()
    {
        _bulletPool = new Bullet[maxAmmo];
        for (var i = 0; i < maxAmmo; ++i)
        {
            var obj = Instantiate(bulletPrefab, null);
            _bulletPool[i] = obj.GetComponent<Bullet>();
            _bulletPool[i].Initialize(UnityEngine.Random.Range(bulletMinDamage,bulletMaxDamage+1), bulletInitTransform,damageMessageManager);
        }
    }



    protected override IEnumerator ShootBullet(int number)
    {
        var _number = currentAmmo - number < 0 ? currentAmmo : number;
        
        for (var i = 0; i < _number; ++i)
        {
            _bulletPool[currentAmmo-1].Go(GetBulletDirection() * bulletSpeed, bulletLifetime);
            --currentAmmo;
            currentAmmoUI.text = currentAmmo.ToString();
            effectManager.GunFire();
            audioSource.PlayOneShot(shootSFX);
            animationController.Shoot(burstMode);
            yield return new WaitForSeconds(burstMode? burstFireCD:fireCD);
        }

        _isShooting = false;
    }

    public void ObtainAmmo(int ammo)
    {
        backUpAmmo += ammo;
        backupAmmoUI.text = backUpAmmo.ToString();
    }

    private void UpdateScreenCenterOnSizeChanged()
    {
        if (!Mathf.Approximately(Screen.width / 2f, _screenCenter.x))
        {
            _screenCenter =  new Vector3(Screen.width/2f,Screen.height/2f,0f);
        }
    }
    
    
    
}
