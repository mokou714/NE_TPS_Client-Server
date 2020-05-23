using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private Transform[] enemySpawns; //spawns should exceed enemies
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Text levelUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private PlayerDataSaverAndLoader saverAndLoader;
    public MessageManager messageManager;
    private int enemyCount;
    private int gameLevel;
    private bool gameStarted;
    
    //statistics
    private int playerKills;
    private float playingTime;
    
    //network
    private DataManager dataManager;

    private void Awake()
    {
        if (DataManager.PlayerData.userId == "")
        {
            messageManager.Display("Failed to load player data. Player data not exists.");
            Debug.Log("Failed to load player data. Player data not exists.");
        }
        else
        {
            saverAndLoader.LoadPlayerData(DataManager.PlayerData);
            playerKills = DataManager.PlayerData.totalKills;
            playingTime = HMSToTime(DataManager.PlayerData.playingTime);
            gameLevel = DataManager.PlayerData.currentLevel;
        }
        
        enemyCount = enemies.Length;
        GameBegins();
    }
    // Start is called before the first frame update
    void Start()
    {
        dataManager = DataManager.Instance;
    }
    private void InitCharacters()
    {
        var i = Random.Range(0, playerSpawns.Length);
        player.transform.position = playerSpawns[i].position;
        player.transform.rotation = playerSpawns[i].rotation;
        player.GetComponent<PlayerHealthManager>().SetGSM(this);
        player.SetActive(true);

        var j = 0;
        while(j < enemies.Length)
        {
            i = Random.Range(j, enemySpawns.Length);
            enemies[j].transform.position = enemySpawns[i].position;
            enemies[j].transform.rotation = enemySpawns[i].rotation;
            enemies[j].GetComponent<EnemyHealthManager>().SetGSM(this);
            enemies[j].SetActive(true);
            //swap i and j
            var temp = enemySpawns[j];
            enemySpawns[j] = enemySpawns[i];
            enemySpawns[i] = temp;
            ++j;
        }
    }


    public void OnPlayerDies()
    {
        GameEnds(false);
    }

    public void OnEnemyDie()
    {
        enemyCount--;
        playerKills++;
        
        if(enemyCount == 0)
            GameEnds(true);
    }

    private void GameBegins()
    {
        levelUI.text = "LEVEL\n" + gameLevel;
        InitCharacters();
        gameStarted = true;
    }


    public void NextLevel()
    {
        //save player data to memory
        if (saverAndLoader.SavePlayerData())
        {
            DataManager.PlayerData.currentLevel = gameLevel + 1;
            DataManager.PlayerData.playingTime = TimeToHMS(Time.time + playingTime);
            DataManager.PlayerData.totalKills = playerKills;
        }

        gameLevel++;
        Time.timeScale = 1;
        LoadGameScene();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        LoadGameScene();
    }

    public void Save(bool playerWin)
    {
        messageManager.Display("Saving...");
        //save player data to memory
        if (saverAndLoader.SavePlayerData())
        {
            DataManager.PlayerData.currentLevel = playerWin ? gameLevel + 1 : gameLevel;
            DataManager.PlayerData.playingTime = TimeToHMS(Time.time + playingTime);
            DataManager.PlayerData.totalKills = playerKills;
            //send to database...
            dataManager.SavePlayerData(processSavingResult);
        }
        else
        {
            messageManager.Display("Failed to save player data. Target data object not exists.");
            Debug.Log("Failed to save player data. Target data object not exists.");
        }
    }

    private void GameEnds(bool playerWins)
    {
        Time.timeScale = 0;
        gameStarted = false;
        Cursor.visible = true;
        Screen.lockCursor = false;
        if(playerWins)
            winUI.SetActive(true);
        else
            loseUI.SetActive(true);
    }

    public void LoadStartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }


    public bool IsGameRunning()
    {
        return gameStarted;
    }

    private String TimeToHMS (float time)
    {
        var hour = (int)time / 60 / 60;
        var min = (int) time / 60 % 60;
        var sec = (int) time % 60;

        return hour + "h " + min + "m " + sec + "s";
    }

    private float HMSToTime(string time)
    {
        var tokens = time.Split(' ');
        var t = int.Parse(tokens[0].Substring(0, 1)) + int.Parse(tokens[1].Substring(0, 1)) +
                int.Parse(tokens[2].Substring(0, 1));
        return t;
    }

    private void processSavingResult(string result)
    {
        if (result == "success")
        {
            messageManager.Display("Successfully saved player data");
        }
        else
        {
            messageManager.Display("Fail to save player data");
        }

    }
    
}
