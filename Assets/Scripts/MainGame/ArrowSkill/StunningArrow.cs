using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StunningArrow : BaseArrow
{
    public float stunningRange;
    public int stunningTime;

    private List<EffectManager> stunnedCharacters = new List<EffectManager>();
    
    protected override void OnTriggerEnter(Collider other)
    {
        //for now: only play can shoot arrows
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
            var skd = enemy.gameObject.GetComponent<EffectManager>();
            skd.Stun(true, stunningTime);
            stunnedCharacters.Add(skd);
            damageMessageManager.ShowMessage(damage, enemy.gameObject.GetComponent<EnemyAIController>().Center.transform.position);
        }
        
        //deal damage to other enemies around
        var others = Physics.OverlapSphere(enemy.GetComponent<EnemyAIController>().Center.transform.position, stunningRange);
        foreach (var e in others)
        {
            //if can deal damage, then apply effect
            if (e.gameObject.CompareTag("Enemy") &&
                e.gameObject.GetComponent<EnemyHealthManager>().DealDamage(damage))
            {
                var skd = e.gameObject.GetComponent<EffectManager>();
                skd.Stun(true, stunningTime);
                damageMessageManager.ShowMessage(damage, e.gameObject.GetComponent<EnemyAIController>().Center.transform.position);
                stunnedCharacters.Add(skd);
            }
        }
    }

    
}
