using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private Transform[] enemySpawns; //spawns should exceed enemies
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Text levelUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    private int enemyCount;
    private int gameLevel;
    private bool gameStarted;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = enemies.Length;
        GameBegins();
        
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
        
        if(enemyCount == 0)
            GameEnds(true);
    }

    private void GameBegins()
    {
        levelUI.text = "LEVEL\n" + gameLevel.ToString();
        InitCharacters();
        gameStarted = true;
    }


    public void NextLevel()
    {
        gameLevel++;
        Time.timeScale = 1;
        LoadGameScene();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        LoadGameScene();
    }

    private void GameEnds(bool playerWins)
    {
        Time.timeScale = 0;
        gameStarted = false;
        Cursor.visible = true;
        if(playerWins)
            winUI.SetActive(true);
        else
            loseUI.SetActive(true);
    }

    public void LoadStartScene()
    {
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
}
