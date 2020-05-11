using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private int _damage;
    [SerializeField] private EnemyAIController _aiController;

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_aiController.AiState == AIState.ATTACK && other.gameObject.CompareTag("Player"))
        {
            var playerController = other.GetComponent<BaseHealthManager>();
            playerController.DealDamage(_damage);
        }
    }
}
