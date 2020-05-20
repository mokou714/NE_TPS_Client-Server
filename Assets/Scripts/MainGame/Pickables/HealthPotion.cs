using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HealthPotion : Pickable
{
    public int HealthPoint;
    
    protected override void PickUp(GameObject player)
    {
        base.PickUp(player);
        player.GetComponent<PlayerHealthManager>().IncreaseHealth(HealthPoint);
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);
        }
    }
}
