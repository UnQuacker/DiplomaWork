 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Globalization;

public class MinigameBasketMovement : MonoBehaviour, IDataPersistence, ISettingsPresistance
{
    public float movementSpeed = 5f;
    public Rigidbody2D rigidBody2D;
    public bool wonGame = false;
    public GameObject winPanel;
    public TextMeshProUGUI currentNumberLabel;
    public TextMeshProUGUI targetNumberLabel;

    [SerializeField]
    public string minigameId;
    [SerializeField]
    public int scene = 1;
    [SerializeField]
    public int fruitCounter = 0;
    public int targetNumber = 5;

    private int highscoreMultiplier;
    private int currentHighscore;
    private int highscore;

    private Vector2 movement;

    void Start()
    {
        targetNumberLabel.text = targetNumber.ToString();
        currentNumberLabel.text = fruitCounter.ToString();

    }
    void Update()
    {
        if (Time.timeScale == 0) { return; }
        movement.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rigidBody2D.MovePosition(rigidBody2D.position + movement.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CorrectFruit" && gameObject.tag == "basket")
        {
            fruitCounter++;
            updateLabel(fruitCounter.ToString());
        }
        if(collision.gameObject.tag == "InCorrectFruit" && gameObject.tag == "basket" && fruitCounter > 0)
        {
            fruitCounter--;
            updateLabel(fruitCounter.ToString());
        }
        if (fruitCounter >= targetNumber && this.wonGame!=true)
        {
            gameObject.tag = "Untagged";
            this.wonGame = true;
            winPanel.SetActive(true);
            GetHighscore();
        }
    }

    private void GetHighscore()
    {
        highscore = highscoreMultiplier;
        Debug.Log(targetNumber + "*" + highscoreMultiplier);
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            OnGetStatistics, errorCallback => { Debug.Log("error retrieving statistics"); });
    }
    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            if (eachStat.StatisticName == "Highscore")
            {
                currentHighscore = eachStat.Value;
            }
        }
        UpdateHighscore();
    }

    private void UpdateHighscore()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "Highscore", Value = highscore + currentHighscore },}
        }, result => { switchScene(); }, error => { Debug.LogError(error.GenerateErrorReport()); });

    }
    public void LoadData(GameData gameData)
    {
        
    }

    public void SaveData(ref GameData gameData)
    {
        if (gameData.minigamesCompleted.ContainsKey(minigameId))
        {
            gameData.minigamesCompleted.Remove(minigameId);
        }
        gameData.minigamesCompleted.Add(minigameId, wonGame);

    }

    private void switchScene()
    {
        DataPresistanceManager.instance.SaveGame();
        Invoke("LoadScene", 3);
    }
    private void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }


    public void updateLabel(string currentNumber)
    {
        currentNumberLabel.text = currentNumber;
    }

    public void LoadSettings(GameSettings gameSettings)
    {
        string tempValue = "";
        //gameSettings.basketMovementSpeed.TryGetValue(minigameId, out tempSpeed);
        Dictionary<string, string> settings = gameSettings.gameSettings[minigameId];
        settings.TryGetValue("MovementSpeed", out tempValue);
        movementSpeed = float.Parse(tempValue, CultureInfo.InvariantCulture);

        settings.TryGetValue("WinConditionNumber", out tempValue);
        if (tempValue!= null)
        {
            targetNumber = int.Parse(tempValue);
        }

        settings.TryGetValue("HighscoreMultiplier", out tempValue);
        highscoreMultiplier = int.Parse(tempValue);

    }
}
