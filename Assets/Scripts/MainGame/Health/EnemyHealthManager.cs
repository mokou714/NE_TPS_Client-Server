using UnityEngine;
using UnityEngine.AI;
public class EnemyHealthManager : BaseHealthManager
{
        [SerializeField] private EnemyHealthBar enemyHealthBar;
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
}
