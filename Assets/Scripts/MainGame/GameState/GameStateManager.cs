using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
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
    [SerializeField] private Image[] countDownElements;
    [SerializeField] private float countDownInterval;
    [SerializeField] private PlayerDataSaverAndLoader saverAndLoader;
    public MessageManager messageManager;
    
    private int enemyCount;
    private int gameLevel;
    private bool gameStarted;
    private bool countDownAnimating;
    
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
            levelUI.text = "LEVEL\n" + gameLevel;
        }
        
        enemyCount = enemies.Length;
        dataManager = DataManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitPlayer();
        InitEnemies();
        StartCoroutine(CountDown(GameBegins));
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
    
    private void InitEnemies()
    {
        
        var j = 0;
        while(j < enemies.Length)
        {
            var i = Random.Range(j, enemySpawns.Length);
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

    private void InitPlayer()
    {
        var i = Random.Range(0, playerSpawns.Length);
        player.transform.position = playerSpawns[i].position;
        player.transform.rotation = playerSpawns[i].rotation;
        player.GetComponent<PlayerHealthManager>().SetGSM(this);
        player.SetActive(true);
    }

    private void GameBegins()
    {
        player.GetComponent<CharacterStatus>().isAlive = true;
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<CharacterStatus>().isAlive = true;
        }
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
        BGMPlayer.Instance.transform.parent = null;
        DontDestroyOnLoad(BGMPlayer.Instance);
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        Time.timeScale = 1;
        BGMPlayer.Instance.transform.parent = null;
        DontDestroyOnLoad(BGMPlayer.Instance);
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

    private IEnumerator CountDown(Action OnCountDownEnd)
    {
        for(var i = 0; i < countDownElements.Length; ++ i)
        {
            StartCoroutine(CountDownAnimation(i));
            yield return new WaitUntil(()=>!countDownAnimating);
        }
        OnCountDownEnd();
    }


    private IEnumerator CountDownAnimation(int elementIndex)
    {
        var startTime = Time.time;
        countDownAnimating = true;
        countDownElements[elementIndex].gameObject.SetActive(true);
        var _color = countDownElements[elementIndex].color;
        var _scale = countDownElements[elementIndex].transform.localScale;
        
        while (Time.time < startTime + countDownInterval)
        {
            //not animate the last element("Start!")
            if (elementIndex != countDownElements.Length - 1)
            {
                countDownElements[elementIndex].color -= new Color(0, 0, 0, Mathf.Lerp(0, 1, 0.002f));
                countDownElements[elementIndex].transform.localScale -= Vector3.Lerp(Vector3.zero, _scale, 0.002f);
            }

            yield return new WaitForSeconds(0.01f);
        }

        countDownElements[elementIndex].color = _color;
        countDownElements[elementIndex].transform.localScale = _scale;
        countDownElements[elementIndex].gameObject.SetActive(false);
        countDownAnimating = false;
    }
}
