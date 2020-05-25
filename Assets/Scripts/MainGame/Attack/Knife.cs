using UnityEngine;

public class Knife : MonoBehaviour
{
    private int _damage;
    [SerializeField] private AIStatus aiStatus;

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.CompareTag("Player") &&  aiStatus.aiState == AIState.ATTACK && aiStatus.isAlive)
        {
            var playerController = other.GetComponent<BaseHealthManager>();
            playerController.DealDamage(_damage);
        }
    }
}
