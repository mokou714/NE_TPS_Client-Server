using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSupply : Pickable
{
    public int ammo;
    [SerializeField] private Text backupAmmoUI;
    
    protected override void PickUp(GameObject player)
    {
        var shootManager = player.GetComponent<PlayerShootManager>();
        shootManager.backUpAmmo += ammo;
        backupAmmoUI.text = shootManager.backUpAmmo.ToString();
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp(other.gameObject);
            Destroy(gameObject);
        }
    }

}
