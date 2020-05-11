using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HealthPotion : Pickable
{
    public int HealthPoint;
    private PlayerHealthManager _playerHealth;
    protected override void PickUp()
    {
        _playerHealth = _player.GetComponent<PlayerHealthManager>();
        _playerHealth.IncreaseHealth(HealthPoint);
        Destroy(gameObject);
    }
}
