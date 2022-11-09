using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public TimeSpan TimeElapsed { get; private set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public Corridor CorridorMaster { get; set; }

    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject prefabBarrier;
    
    private DateTime startTime;
    private int intervalForIncreaseVerticalSpeed = 15;
    private bool wasCorridorAndBarriersSpeedSet;
    private List<Barrier> barriers = new();
    private int columnCellsPerBarrier = 6;  //N
    private int cellsCountInRow = 19;
    private int cellsCountInColumn = 9;
    private Vector3 basePosition = new(10.27f, 4.02f, 5f);
    private Player player;

    private void Awake()
    {
        var initializers = GameObject.FindGameObjectsWithTag("Initializer");
        if (initializers.Length > 1)
            DestroyImmediate(gameObject);
        else
            DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
        ResetSettings();
        SceneManager.sceneLoaded += HandlerSceneLoaded;
    }

    private void HandlerSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (IsSceneNameContains("mainscene"))
        {
            SpawnPlayer();
            var corridorGameObject = GameObject.Find("CorridorMaster");
            var corridor = corridorGameObject.GetComponent<Corridor>();
            CorridorMaster = corridor;
            SpawnBarriers();
        }    
    }

    private void Update()
    {
        if (IsSceneNameContains("mainscene"))
        {
            if (!wasCorridorAndBarriersSpeedSet)
                SetSpeedToCorridorAndBarriers();
            
            if (WasPlayerHit())
                GameOver();
            else
                IncreasePlayerSpeedIfNeed();
        }
    }

    private void SetSpeedToCorridorAndBarriers()
    {
        float moveSpeed = 0f;
        switch (DifficultyLevel)
        {
            case DifficultyLevel.Easy:
            {
                moveSpeed = 5f;
                break;
            }
            case DifficultyLevel.Normal:
            {
                moveSpeed = 7.5f;
                break;
            }
            case DifficultyLevel.Hard:
            {
                moveSpeed = 10f;
                break;
            }
        }

        CorridorMaster.MoveSpeed = moveSpeed;
        wasCorridorAndBarriersSpeedSet = true;

        foreach (var barrier in barriers)
        {
            barrier.MoveSpeed = moveSpeed;
            barrier.CellsCountInColumn = cellsCountInColumn;
            barrier.basePositionY = basePosition.y;
        }
    }

    private bool WasPlayerHit()
    {
        return player.WasPlayerHit;
    }

    private void IncreasePlayerSpeedIfNeed()
    {
        var timeElapsed = DateTime.Now - startTime;
        var intervalElapsed = (int)timeElapsed.TotalSeconds / intervalForIncreaseVerticalSpeed;
        player.VerticalSpeedFactor = 0.5f + intervalElapsed * 0.25f;
    }

    private bool IsSceneNameContains(string name)
    {
        var scene = SceneManager.GetActiveScene();
        var sceneName = scene.name;
        sceneName = sceneName.Trim();
        sceneName = sceneName.ToLower();
        return sceneName.Contains(name);
    }
    
    public void ResetSettings()
    {
        startTime = DateTime.Now;
        wasCorridorAndBarriersSpeedSet = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void GameOver()
    {
        TimeElapsed = DateTime.Now - startTime;
        SceneManager.LoadScene("GameOverScene");
    }
    
    private void SpawnBarriers()
    {
        var barriersCount = cellsCountInRow / columnCellsPerBarrier;
        for (int i = 0; i < barriersCount; i++)
        {
            var x = basePosition.x + i * columnCellsPerBarrier;
            var random = new Random();
            var y = basePosition.y - random.Next(0, cellsCountInColumn);
            var z = 5f;
            
            SpawnBarrier(new Vector3(x,y,z));
        }
    }

    private void SpawnBarrier(Vector3 position)
    {
        var barrierGameObject =
            Instantiate(prefabBarrier, position, Quaternion.identity);
        var barrier = barrierGameObject.GetComponent<Barrier>();
        barriers.Add(barrier);
    }
    
    private void SpawnPlayer()
    {
        var playerGameObject =
            Instantiate(prefabPlayer, new Vector3(-5, 0, 5), Quaternion.identity);
        player = playerGameObject.GetComponent<Player>();
    }
}