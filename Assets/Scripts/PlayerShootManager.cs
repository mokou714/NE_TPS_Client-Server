﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootManager : BaseShootManager
{
    
    //UI
    [SerializeField] private BulletIconAnim bulletIconAnim;
    [SerializeField] private DamageMessageManager damageMessageManager;
    [SerializeField] private Text maxAmmoUI;
    [SerializeField] private Text currentAmmoUI;

    //helper data
    private Vector3 _mousePositionOffset;
    private Vector3 _screenCenter;

    //other components
    [SerializeField]private TPSCameraController _cameraController;
    
    
    protected override void Start()
    {
        _characterController = GetComponent<PlayerController>();
        _animationController = GetComponent<PlayerAnimationController>();
        currentAmmo = maxAmmo;
        currentAmmoUI.text = maxAmmoUI.text = maxAmmo.ToString();
        Cursor.visible = false;
        _screenCenter = new Vector3(Screen.width/2f,Screen.height/2f,0f);
        InitBulletPool();
    }


    protected override void Update()
    {
        base.Update();
        CursorState();
    }


    public override Vector3 GetAimingDirection()
    {
        Ray cursorRay = _cameraController.getCamera().ScreenPointToRay(_screenCenter);
        //Debug.DrawRay(cursorRay.origin, cursorRay.direction * 10, Color.yellow);
        return cursorRay.direction;
    }

    public override Vector3 GetBulletDirection()
    {
        Ray cursorRay = _cameraController.getCamera().ScreenPointToRay(_screenCenter);
        return 
            Physics.Raycast(cursorRay, out var hitInfo, 100f, 1 << layerMask) ?
            (hitInfo.point - bulletInitTransform.position).normalized : //direction from bullet to hit point
            cursorRay.direction; //aiming ray not hit, return aiming direction
    }

    public override void FinishReloading()
    {
        base.FinishReloading();
        currentAmmoUI.text = currentAmmo.ToString();
    }

    protected override void Shoot()
    {
        //check player state
        if(((PlayerController)_characterController).isJumping || _characterController.posture == BodyPosture.GUARD) return;
        //check shooting state
        if(currentAmmo == 0 || _isShooting || _isReloading) return;

        if (Input.GetMouseButtonDown(0) && !burstMode || (Input.GetMouseButton(0) && burstMode))
        {
            _isShooting = true;
            _animationController.Shoot(burstMode);
            StartCoroutine(ShootBullet(burstMode ? 4 : 1));
        }
    }

    protected override void SwitchMode()
    {
        if (_characterController.posture == BodyPosture.GUARD) return;
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            burstMode = !burstMode;
            bulletIconAnim.SwitchMode(burstMode);
        }
    }

    protected override void Reload()
    {
        if (_isReloading || _isShooting) return;
        
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            _isReloading = true;
            _animationController.Reload();
        }
    }
    
    protected override void InitBulletPool()
    {
        _bulletPool = new Bullet[maxAmmo];
        for (int i = 0; i < maxAmmo; ++i)
        {
            GameObject obj = Instantiate(bulletPrefab, null);
            _bulletPool[i] = obj.GetComponent<Bullet>();
            _bulletPool[i].Initialize(bulletInitTransform,damageMessageManager);
        }
    }

    private void CursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.visible = true;
        else if (Input.GetMouseButton(0) && Cursor.visible)
            Cursor.visible = false;
    }

    IEnumerator ShootBullet(int number)
    {
        int _number = currentAmmo - number < 0 ? currentAmmo : number;
        
        for (int i = 0; i < _number; ++i)
        {
            _bulletPool[currentAmmo-1].Go(GetBulletDirection() * bulletSpeed, bulletLifetime);
            --currentAmmo;
            currentAmmoUI.text = currentAmmo.ToString();
            yield return new WaitForSeconds(fireCDTime);
        }

        _isShooting = false;
    }

    
    
    
    
    
}
