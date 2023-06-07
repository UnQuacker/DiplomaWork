using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class ColourMixer : MonoBehaviour, IDataPersistence, ISettingsPresistance
{
    public GameObject mainObject;
    public GameObject triangle;
    public GameObject square;
    public GameObject circle;

    public GameObject targetNumberObject;
    [SerializeField]
    public int scene = 1;
    private bool wonGame = false;
    public int triangleNumber;
    public int squareNumber;
    public int circleNumber;
    public GameObject winPanel;
    [SerializeField]
    public string minigameId;

    private int currentNumber = 0;
    private int targetNumber;
    private List<Color> colors = new List<Color>();
    private Color startingColour;

    private int highscoreMultiplier;
    private int currentHighscore;
    private int highscore;

    private void InitializeGame()
    {
        //targetNumber = int.Parse(targetNumberObject.GetComponentInChildren<TextMeshProUGUI>().text);
        targetNumberObject.GetComponentInChildren<TextMeshProUGUI>().text = targetNumber.ToString();
        float r = mainObject.GetComponent<SpriteRenderer>().material.color.r;
        float g = mainObject.GetComponent<SpriteRenderer>().material.color.g;
        float b = mainObject.GetComponent<SpriteRenderer>().material.color.b;
        startingColour = new Color(r, g, b);
    }


    void Update()
    {
        if (Time.timeScale == 0) { return; }
        if (currentNumber.Equals(targetNumber) && wonGame == false)
        {
            wonGame = true;
            GetHighscore();
            winPanel.SetActive(true);
        }
    }

    private void GetHighscore()
    {
        highscore = highscoreMultiplier;
        Debug.Log(highscoreMultiplier);
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

    public void addTriange()
    {
        currentNumber += triangleNumber;
        mainObject.GetComponentInChildren<TextMeshProUGUI>().text = currentNumber.ToString();
        Color triangleColour = triangle.GetComponent<SpriteRenderer>().color;
        colors.Add(triangleColour);
        changeColour();

    }

    public void addSquare()
    {
        currentNumber += squareNumber;
        mainObject.GetComponentInChildren<TextMeshProUGUI>().text = currentNumber.ToString();
        Color squareleColour = square.GetComponent<SpriteRenderer>().color;
        colors.Add(squareleColour);
        changeColour();
    }

    public void addCircle()
    {
        currentNumber += circleNumber;
        mainObject.GetComponentInChildren<TextMeshProUGUI>().text = currentNumber.ToString();
        Color circleColour = circle.GetComponent<SpriteRenderer>().color;
        colors.Add(circleColour);
        changeColour();
    }

    private void changeColour()
    {
        float r = 0f;
        float g = 0f;
        float b = 0f;

        foreach(var colour in colors)
        {
            r += colour.r;
            g += colour.g;
            b += colour.b;
        }

        r = r / colors.Count;
        g = g / colors.Count;
        b = b / colors.Count;

        var mainObjectRenderer = mainObject.GetComponent<SpriteRenderer>();
        mainObjectRenderer.material.SetColor("_Color", new Color(r, g, b));
    }

    public void resetColors()
    {
        var mainObjectRenderer = mainObject.GetComponent<SpriteRenderer>();
        mainObjectRenderer.material.SetColor("_Color", startingColour);
        currentNumber = 0;
        mainObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    private void switchScene()
    {
        mainObject.SetActive(false);
        winPanel.SetActive(true);
        DataPresistanceManager.instance.SaveGame();
        Invoke("LoadScene", 3);
    }
    private void LoadScene()
    {
        SceneManager.LoadScene(scene);
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

    public void LoadSettings(GameSettings gameSettings)
    {
        Dictionary<string, string> settings = gameSettings.gameSettings[minigameId];
        string tempValue;

        settings.TryGetValue("Button1", out tempValue);
        triangleNumber = int.Parse(tempValue);

        settings.TryGetValue("Button2", out tempValue);
        squareNumber = int.Parse(tempValue);

        settings.TryGetValue("Button3", out tempValue);
        circleNumber = int.Parse(tempValue);

        settings.TryGetValue("TargetNumber", out tempValue);
        targetNumber = int.Parse(tempValue);

        settings.TryGetValue("HighscoreMultiplier", out tempValue);
        highscore = int.Parse(tempValue);

        InitializeGame();
    }
}
