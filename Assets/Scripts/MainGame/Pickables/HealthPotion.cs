using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HealthPotion : Pickable
{
    public int HealthPoint;
    
    protected override void PickUp(GameObject player)
    {
        player.GetComponent<PlayerHealthManager>().IncreaseHealth(HealthPoint);
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            PickUp(other.gameObject);
    }
}
