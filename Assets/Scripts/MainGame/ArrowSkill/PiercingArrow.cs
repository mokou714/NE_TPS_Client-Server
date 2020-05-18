using UnityEngine;

public class PiercingArrow : BaseArrow
{
   protected override void OnTriggerEnter(Collider other)
   {
      //for now: only play can shoot arrows
      if (other.gameObject.CompareTag("Enemy"))
      {
         if(other.GetComponent<EnemyHealthManager>().DealDamage(damage))
            damageMessageManager.ShowMessage(damage, other.gameObject.GetComponent<EnemyAIController>().Center.transform.position);
      }
      else
      {
         Reset();
      }
   }
}
