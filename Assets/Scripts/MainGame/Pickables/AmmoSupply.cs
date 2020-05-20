using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSupply : Pickable
{
    public int ammo;

    protected override void PickUp(GameObject player)
    { 
        base.PickUp(player);
        player.GetComponent<PlayerShootManager>().ObtainAmmo(ammo);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);
        }
    }

}
