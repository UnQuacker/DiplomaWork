using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectWining : MonoBehaviour, IDataPersistence, ISettingsPresistance
{
    private bool wonGame = false;
    [SerializeField]
    public int scene = 1;
    [SerializeField]
    public string minigameId;
    public GameObject winPanel;
    public GameObject player;
    public int delayAfterWinPanel;

    private int currentHighscore;
    private int highscore;
    private int highscoreMultiplier;

    void Update()
    {
        if(player.transform.position == this.gameObject.transform.position && !wonGame)
        {
            wonGame = true;
            winPanel.SetActive(true);
            GetHighscore();
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
        Invoke("LoadScene",3);
    }
    private void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadSettings(GameSettings gameSettings)
    {
        Dictionary<string, string> settings = gameSettings.gameSettings[minigameId];
        string highscoreMultipliertemp;
        string tempX;
        string tempY;
        settings.TryGetValue("TargetX", out tempX);
        settings.TryGetValue("TargetY", out tempY);
        Vector3 newPosition = new Vector3(float.Parse(tempX, CultureInfo.InvariantCulture.NumberFormat), float.Parse(tempY, CultureInfo.InvariantCulture.NumberFormat), 0);
        settings.TryGetValue("HighscoreMultiplier", out highscoreMultipliertemp);
        highscoreMultiplier = int.Parse(highscoreMultipliertemp);
        this.transform.position = newPosition;

    }
}
