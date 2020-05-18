using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsLoader : MonoBehaviour
{
    [SerializeField] private TextMesh playerInfo;
    [SerializeField] private TextMesh totalKills;
    [SerializeField] private TextMesh playingTime;
    [SerializeField] private TextMesh currentLevel;
    [SerializeField] private CameraTransition cameraTransition;

    public void Load()
    {
        if (DataManager.PlayerData is null)
        {
            Debug.Log("Player data not exists");
        }
        else
        {
            playerInfo.gameObject.SetActive(true);
            totalKills.gameObject.SetActive(true);
            playingTime.gameObject.SetActive(true);
            currentLevel.gameObject.SetActive(true);
            playerInfo.text = DataManager.PlayerData.userName + "(health:" + DataManager.PlayerData.health + ")";
            totalKills.text = DataManager.PlayerData.totalKills.ToString();
            playingTime.text = DataManager.PlayerData.playingTime;
            currentLevel.text = DataManager.PlayerData.currentLevel.ToString();
            
            cameraTransition.SetOnTransitionEvents(new List<Action>
            {
                () =>
                {
                    playerInfo.gameObject.SetActive(false);
                    totalKills.gameObject.SetActive(false);
                    playingTime.gameObject.SetActive(false);
                    currentLevel.gameObject.SetActive(false);
                }
            });
        }
    }
}
