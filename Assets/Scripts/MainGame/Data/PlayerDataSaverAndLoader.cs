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
    public MessageManager messageManager;

    public void LoadPlayerData(PlayerData data)
    {
        player.playerHealthManager.health = data.health;
        player.playerShootManager.backUpAmmo = data.backupAmmo;
        player.arrowSkillManager.arrowCount = data.arrowCount;
        //notify gamestate
    }

    public bool SavePlayerData()
    {
        if (DataManager.PlayerData is null) return false;
        DataManager.PlayerData.health = player.playerHealthManager.health == 0? 100 :player.playerHealthManager.health;
        DataManager.PlayerData.backupAmmo = player.playerShootManager.backUpAmmo;
        DataManager.PlayerData.arrowCount = player.arrowSkillManager.arrowCount;
        return true;
        
    }
}
