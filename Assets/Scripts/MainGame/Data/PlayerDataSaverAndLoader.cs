using System;
using System.Data;
using UnityEngine;

public class PlayerDataSaverAndLoader : MonoBehaviour
{
    [Serializable]
    public struct Player
    {
        public PlayerHealthManager playerHealthManager;
        public PlayerShootManager playerShootManager;
        public ArrowSkillManager arrowSkillManager;
    }

    public Player player;
    public GameStateManager gameStateManager;
    
    
    private void Awake()
    {
        if (DataManager.PlayerData.userId is null)
        {
            Debug.Log("Failed to load player data. Player data not exists.");
        }
        else
        {
            Load(DataManager.PlayerData);
        }
    }

    private void Load(PlayerData data)
    {
        player.playerHealthManager.health = data.health;
        player.playerShootManager.backUpAmmo = data.backupAmmo;
        player.arrowSkillManager.arrowCount = data.arrowCount;
        //notify gamestate
    }

    private void Save()
    {
        if (DataManager.PlayerData is null)
        {
            Debug.Log("Failed to save player data. Target data container not exits.");
        }
        else
        {
            DataManager.PlayerData.health = player.playerHealthManager.health;
            DataManager.PlayerData.arrowCount = player.playerShootManager.backUpAmmo;
            DataManager.PlayerData.arrowCount = player.arrowSkillManager.arrowCount;
            //notify gamestate
        }
    }
}
