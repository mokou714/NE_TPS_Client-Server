using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private int _damage;
    private AIStatus _status;


    private void Start()
    {
        _status = GetComponent<AIStatus>();
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_status.aiState == AIState.ATTACK && other.gameObject.CompareTag("Player"))
        {
            var playerController = other.GetComponent<BaseHealthManager>();
            playerController.DealDamage(_damage);
        }
    }
}
