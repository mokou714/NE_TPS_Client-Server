using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIShootManager : BaseShootManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _characterController = GetComponent<EnemyAIController>();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Enemy can only shoot
        Shoot();
    }

    public override Vector3 GetAimingDirection()
    {
        var aiController = (EnemyAIController) _characterController;
        return (aiController.EyeTransform.position - aiController.TargetPlayer.Center.position).normalized;
    }

    public override Vector3 GetBulletDirection()
    {
        var aiController = (EnemyAIController) _characterController;
        return (aiController.TargetPlayer.Center.position -bulletInitTransform.position).normalized;
    }

    protected override void Shoot()
    {
        //automatic reloading
        if (currentAmmo == 0)
        {
            Reload();
            return;
        }
        var aiController = (EnemyAIController) _characterController;
        if(aiController.AiState == AIState.ATTACK)
            base.Shoot();
        
    }

 
}
