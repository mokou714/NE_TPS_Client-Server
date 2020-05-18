using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSupply : Pickable
{
    public int arrows;
    public Text arrowUI;
    
    protected override void PickUp(GameObject player)
    {
        var skillManager = player.GetComponent<ArrowSkillManager>();
        skillManager.arrowCount += arrows;
        arrowUI.text = skillManager.arrowCount.ToString();
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
