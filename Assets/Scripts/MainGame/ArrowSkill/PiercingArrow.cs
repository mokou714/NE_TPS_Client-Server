using UnityEngine;

public class PiercingArrow : BaseArrow
{
   protected override void OnTriggerEnter(Collider other)
   {
      if (!isOnAir) return;
      //for now: only player can shoot arrows
      if (other.gameObject.CompareTag("Enemy"))
      {
         if(other.GetComponent<EnemyHealthManager>().DealDamage(damage))
            damageMessageManager.ShowMessage(damage, other.gameObject.GetComponent<EnemyAIController>().Center.transform.position);
      }
      else
      {
         base.OnTriggerEnter(other);
      }
   }
}
