using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameManager GameManager { get; set; }
    public int TriesNumber { get; set; }
    
    [SerializeField] private TMP_Dropdown dropDownDifficultyLevel;
    [SerializeField] private GameObject startGameButtonGameObject;
    [SerializeField] private GameObject restartGameButtonGameObject;
    [SerializeField] private GameObject changeDifficultyButtonGameObject;
    [SerializeField] private TextMeshProUGUI triesNumberValue;
    [SerializeField] private TextMeshProUGUI timeElapsedValue;
    
    private CustomButton startGameButton;
    private CustomButton restartGameButton;
    private CustomButton changeDifficultyButton;
    private CustomButton gameOverButton;
   
    private void Start()
    {
        var scene = SceneManager.GetActiveScene();
        var sceneName = scene.name;
        sceneName = sceneName.Trim();
        sceneName = sceneName.ToLower();
        if (sceneName.Contains("start"))
        {
            startGameButton = startGameButtonGameObject.GetComponent<CustomButton>();
            startGameButton.ButtonClick += OnStartGameButtonClick;
        }

        if (sceneName.Contains("gameover"))
        {
            restartGameButton = restartGameButtonGameObject.GetComponent<CustomButton>();
            changeDifficultyButton = changeDifficultyButtonGameObject.GetComponent<CustomButton>();
            restartGameButton.ButtonClick += OnStartGameButtonClick;
            changeDifficultyButton.ButtonClick += OnChangeDifficultyButtonClick;

            var timeElapsed = GameManager.TimeElapsed;
            var secondsElapsed = (int)timeElapsed.TotalSeconds;
            timeElapsedValue.text = String.Format("{0} сек", secondsElapsed);
            triesNumberValue.text = TriesNumber.ToString();
        }
    }

    private void OnStartGameButtonClick(object sender, ButtonClickEventArgs buttonClickEventArgs)
    {
        if (dropDownDifficultyLevel != null)
            GameManager.DifficultyLevel = (DifficultyLevel)dropDownDifficultyLevel.value;
        GameManager.StartGame();
    }

    private void OnChangeDifficultyButtonClick(object sender, ButtonClickEventArgs buttonClickEventArgs)
    {
        GameManager.ExitToMainMenu();
    }
    
    private void OnDestroy()
    {
        if (startGameButton != null)
            startGameButton.ButtonClick -= OnStartGameButtonClick;

        if (restartGameButton != null)
            restartGameButton.ButtonClick -= OnStartGameButtonClick;

        if (changeDifficultyButton != null)
            changeDifficultyButton.ButtonClick -= OnChangeDifficultyButtonClick;
    }
}