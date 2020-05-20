using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StunningArrow : BaseArrow
{
    public float stunningRange;
    public int stunningTime;
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        //for now: only player can shoot arrows
        if (other.gameObject.CompareTag("Enemy"))
        {
            InitStunningEffect(other.gameObject);
        }
        Reset();
    }
    

    private void InitStunningEffect(GameObject enemy)
    {
        //deal damage to this enemy
        if (enemy.gameObject.GetComponent<EnemyHealthManager>().DealDamage(damage))
        {
            enemy.gameObject.GetComponent<EffectManager>().Stun(stunningTime);
            damageMessageManager.ShowMessage(damage, enemy.gameObject.GetComponent<EnemyAIController>().Center.transform.position);
        }
        
        //deal damage to other enemies around
        var others = Physics.OverlapSphere(enemy.GetComponent<EnemyAIController>().Center.transform.position, stunningRange);
        foreach (var e in others)
        {
            if(e.gameObject == enemy) continue; //not deal damage to arrow-receiving enemy again 
            
            //if can deal damage, then apply effect
            if (e.gameObject.CompareTag("Enemy") &&
                e.gameObject.GetComponent<EnemyHealthManager>().DealDamage(damage))
            {
                e.gameObject.GetComponent<EffectManager>().Stun(stunningTime);
                damageMessageManager.ShowMessage(damage, e.gameObject.GetComponent<EnemyAIController>().Center.transform.position);
            }
        }
    }

    
}
