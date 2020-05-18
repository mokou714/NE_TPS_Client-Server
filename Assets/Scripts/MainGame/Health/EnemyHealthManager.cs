using UnityEngine.AI;
public class EnemyHealthManager : BaseHealthManager
{
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
}
