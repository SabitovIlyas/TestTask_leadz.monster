using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{
    private UIController uiController;
    private GameManager gameManager;
    [SerializeField] private GameObject prefabGameManager;
    private int triesNumber;

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
        SceneManager.sceneLoaded += HandlerSceneLoaded;
        SpawnGameManager();
        InitializeUIController();
    }
    
    private void InitializeUIController()
    {
        var uiControllerGameObject = GameObject.FindWithTag("UIController");
        uiController = uiControllerGameObject.GetComponent<UIController>();
        uiController.GameManager = gameManager;
    }
    
    private void HandlerSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        InitializeUIController();
        
        var sceneName = scene.name;
        sceneName = sceneName.Trim();
        sceneName = sceneName.ToLower();
        if (sceneName.Contains("gameover"))
        {
            var key = "TriesNumber";
            if (PlayerPrefs.HasKey(key))
                triesNumber = PlayerPrefs.GetInt(key);

            triesNumber++;
            PlayerPrefs.SetInt(key, triesNumber);
            PlayerPrefs.Save();
            uiController.TriesNumber = triesNumber;
        }
        else
            gameManager.ResetSettings();

        if (sceneName.Contains("mainscene"))
        {
            var corridorGameObject = GameObject.Find("CorridorMaster");
            var corridor = corridorGameObject.GetComponent<Corridor>();
            gameManager.CorridorMaster = corridor;
        }
    }

    private void SpawnGameManager()
    {
        var gameManagerGameObject =
            Instantiate(prefabGameManager, new Vector3(0, 0, 0), Quaternion.identity);
        gameManager = gameManagerGameObject.GetComponent<GameManager>();
    }
}