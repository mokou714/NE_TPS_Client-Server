using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIShootManager : BaseShootManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        characterController = GetComponent<EnemyAIController>();
        _status = GetComponent<AIStatus>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Enemy can only shoot
        Shoot();
    }

    public override Vector3 GetAimingDirection()
    {
        
        var aiController = (EnemyAIController) characterController;
        return (aiController.EyeTransform.position - aiController.TargetPlayer.Center.position).normalized;
    }

    public override Vector3 GetBulletDirection()
    {
        var aiController = (EnemyAIController) characterController;
        return (aiController.TargetPlayer.Center.position -bulletInitTransform.position).normalized;
    }

    protected override void Shoot()
    {
        if (!_status.isAlive) return;
        //automatic reloading
        if (currentAmmo == 0)
        {
            Reload();
            return;
        }
        if( ((AIStatus)_status).aiState == AIState.ATTACK)
            base.Shoot();
        
    }

 
}
