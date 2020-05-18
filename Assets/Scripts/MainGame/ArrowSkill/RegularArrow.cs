using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularArrow : BaseArrow
{

    protected override void OnTriggerEnter(Collider other)
    {
        //for now: only play can shoot arrows
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.GetComponent<EnemyHealthManager>().DealDamage(damage))
                damageMessageManager.ShowMessage(damage, other.gameObject.GetComponent<EnemyAIController>().Center.transform.position);
        }
        Reset();
    }
}
