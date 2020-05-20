using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHealthManager : BaseHealthManager
{
        [SerializeField] private EnemyHealthBar enemyHealthBar;
        [SerializeField] private GameObject[] dropsOnDie;
        private NavMeshAgent _agent;
        protected override void Start()
        {
                base.Start();
                _agent = GetComponent<NavMeshAgent>();
        }

        protected override void CharacterDied()
        {
                base.CharacterDied();
                _agent.isStopped = true;
                StartCoroutine(DelayDropping());


        }

        public override bool DealDamage(int damage)
        {
                if (base.DealDamage(damage))
                {
                        enemyHealthBar.LostHealthFraction((float)damage/maxHealth);
                        return true;
                }

                return false;
        }

        IEnumerator DelayDropping()
        {      
                yield return new WaitForSeconds(droppingDelay);
                var rnd = Random.Range(0, dropsOnDie.Length + 1);
                // rnd == Length => no dropping
                if (rnd != dropsOnDie.Length)
                { 
                        var obj = Instantiate(dropsOnDie[rnd], null);
                        obj.transform.position += transform.position;
                }

        }

        
        
}
