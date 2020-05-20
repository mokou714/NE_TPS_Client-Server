using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSupply : Pickable
{
    public int arrows;

    protected override void PickUp(GameObject player)
    {
        base.PickUp(player);
        player.GetComponent<ArrowSkillManager>().ObtainArrows(arrows);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter(other);
        }
        
       
    }
    
}
