using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using PlayFab;

public class PanelsController : MonoBehaviour
{
    public GameObject teacherButton;
    public GameObject studentButton;
    public GameObject settingsButton;
    public GameObject exitButton;

    public GameObject teacherLoginPanel;
    public GameObject teacherPanel;
    public GameObject studentPanel;

    public GameObject teacherSettingsPanel;

    public GameObject firstLevelButton;
    public GameObject secondLevelButton;
    public GameObject thirdLevelButton;
    public GameObject fourthLevelButton;
    public GameObject fifthLevelButton;
    public GameObject sixthLevelButton;

    public GameObject basketMinigameButton;
    public GameObject faultyRobotMinigameButton;
    public GameObject numberJumbleMinigameButton;

    public GameObject basketSettings;
    public GameObject faultyRobotSettings;
    public GameObject numberJumbleSettings;

    public GameObject backButton;
    
    private GameSettings gameSettings;


    private int currentLevel;
    private string currentMinigame;

    private void Start()
    {
        //gameSettings = new GameSettings();
    }

    #region TeacherAndStudentPanel
    public void EnableTeacherSettingsPanel()
    {
        if (teacherPanel.activeSelf)
        {
            teacherPanel.SetActive(false);
            teacherSettingsPanel.SetActive(true);
        }
        else
        {
            teacherPanel.SetActive(true);
            teacherSettingsPanel.SetActive(false);
        }
    }
    public void enableTeacherLoginPanel()
    {
        if(teacherButton.activeSelf == true && studentButton.activeSelf == true)
        {
            teacherLoginPanel.SetActive(true);
            FirstButtonsVisible(false);
        }
        else
        {
            teacherLoginPanel.SetActive(false);
            FirstButtonsVisible(true);
        }
    }
    public void enableStudentPanel()
    {
        if (teacherButton.activeSelf == true && studentButton.activeSelf == true)
        {
            studentPanel.SetActive(true);
            FirstButtonsVisible(false);
        }
        else
        {
            studentPanel.SetActive(false);
            FirstButtonsVisible(true);
        }
    }

    private void FirstButtonsVisible(bool visible)
    {
        settingsButton.SetActive(visible);
        exitButton.SetActive(visible);
        teacherButton.SetActive(visible);
        studentButton.SetActive(visible);
    }
    #endregion TeacherAndStudentPanel

    #region TeacherPanel

    public void ShowTeacherSettingsPanel(bool show)
    {
        teacherPanel.SetActive(!show);
        teacherSettingsPanel.SetActive(show);
        ShowLevelsButtons(true);
    }

    public void ChooseLevel(int level)
    {
        currentLevel = level;
        ShowLevelsButtons(false);
        ShowMinigamesButtons(true);
    }


    public void ChooseBasket()
    {
        currentMinigame = "Basket";
        ShowMinigamesButtons(false);
        ShowBasketSettings(true);
    }
    public void ChooseFaultyRobot()
    {
        currentMinigame = "FaultyRobot";
        ShowMinigamesButtons(false);
        ShowFaultyRobotSettings(true);
    }
    public void ChooseNumberJumble()
    {
        currentMinigame = "NumberJumble";
        ShowMinigamesButtons(false);
        ShowNumberJumbleSettings(true);
    }


    private void ShowLevelsButtons(bool show)
    {
        firstLevelButton.SetActive(show);
        secondLevelButton.SetActive(show);
        thirdLevelButton.SetActive(show);
        fourthLevelButton.SetActive(show);
        fifthLevelButton.SetActive(show);
        sixthLevelButton.SetActive(show);
    }
    private void ShowMinigamesButtons(bool show)
    {
        basketMinigameButton.SetActive(show);
        faultyRobotMinigameButton.SetActive(show);
        numberJumbleMinigameButton.SetActive(show);
    }

    private void ShowBasketSettings(bool show)
    {
        basketSettings.SetActive(show);

    }
    private void ShowFaultyRobotSettings(bool show)
    {
        faultyRobotSettings.SetActive(show);
    }
    private void ShowNumberJumbleSettings(bool show)
    {
        numberJumbleSettings.SetActive(show);
    }

    public void goBack()
    {
        if (basketSettings.activeSelf)
        {
            ShowBasketSettings(false);
            ShowMinigamesButtons(true);
        }
        else if (faultyRobotSettings.activeSelf)
        {
            ShowFaultyRobotSettings(false);
            ShowMinigamesButtons(true);
        }
        else if (numberJumbleSettings.activeSelf)
        {
            ShowNumberJumbleSettings(false);
            ShowMinigamesButtons(true);
        }
        else if (basketMinigameButton.activeSelf)
        {
            ShowMinigamesButtons(false);
            ShowLevelsButtons(true);
        }
        else if (firstLevelButton.activeSelf)
        {
            ShowTeacherSettingsPanel(false);
        }
    }

    public void ConfirmButton()
    {
        switch (currentMinigame)
        {
            case "Basket":
                string teacherName1 = PlayerPrefs.GetString("EMAIL");
                List<string> keys1 = new List<string>();
                var request1 = new GetTitleDataRequest { Keys = keys1 };
                PlayFabClientAPI.GetTitleData(request1, resultCallback => {
                    foreach (var data in resultCallback.Data)
                    {
                        if (data.Key == teacherName1)
                        {
                            this.gameSettings = JsonConvert.DeserializeObject<GameSettings>(data.Value);
                            Debug.Log("Set game settings");
                        }
                    }
                    SaveBasketSettings();
                }, errorCallback => { Debug.Log("LoadSettings failed!"); });
                break;
            case "FaultyRobot":
                string teacherName2 = PlayerPrefs.GetString("EMAIL");
                List<string> keys2 = new List<string>();
                var request2 = new GetTitleDataRequest { Keys = keys2 };
                PlayFabClientAPI.GetTitleData(request2, resultCallback => {
                    foreach (var data in resultCallback.Data)
                    {
                        if (data.Key == teacherName2)
                        {
                            this.gameSettings = JsonConvert.DeserializeObject<GameSettings>(data.Value);
                            Debug.Log("Set game settings");
                        }
                    }
                    SaveFaultyRobotSettings();
                }, errorCallback => { Debug.Log("LoadSettings failed!"); });
                break;
            case "NumberJumble":
                string teacherName3 = PlayerPrefs.GetString("EMAIL");
                List<string> keys3 = new List<string>();
                var request3 = new GetTitleDataRequest { Keys = keys3 };
                PlayFabClientAPI.GetTitleData(request3, resultCallback => {
                    foreach (var data in resultCallback.Data)
                    {
                        if (data.Key == teacherName3)
                        {
                            this.gameSettings = JsonConvert.DeserializeObject<GameSettings>(data.Value);
                            Debug.Log("Set game settings");
                        }
                    }
                    SaveNumberJumbleSettings();
                }, errorCallback => { Debug.Log("LoadSettings failed!"); });
                break;
        }
    }
    
    private string GetKey()
    {
        string key = currentMinigame += currentLevel.ToString();
        return key;
    }

    private void SaveBasketSettings()
    {
        string key = GetKey();

        Dictionary<string, string> basketSettings = new Dictionary<string, string>();

        string MovementSpeed;
        string SpawnRange;
        string WinCondition;
        string WinConditionNumber;
        string HighscoreMultiplier;
        string SpawnRate;

        GameObject basketMovementSpeed = GameObject.Find("Basket Movement Speed");
        TMP_Dropdown basketMovementSpeedValue;
        basketMovementSpeed.TryGetComponent<TMP_Dropdown>(out basketMovementSpeedValue);
        MovementSpeed = basketMovementSpeedValue.options[basketMovementSpeedValue.value].text;

        GameObject basketRange = GameObject.Find("Basket Range");
        TMP_Dropdown basketRangeValue;
        basketRange.TryGetComponent<TMP_Dropdown>(out basketRangeValue);
        SpawnRange = basketRangeValue.options[basketRangeValue.value].text;

        GameObject winCondition = GameObject.Find("Basket Win Condition");
        TMP_Dropdown winConditionValue;
        winCondition.TryGetComponent<TMP_Dropdown>(out winConditionValue);
        WinCondition = winConditionValue.options[winConditionValue.value].text;
        if(WinCondition=="Больше чем")
        {
            WinCondition = "Greater than";
        }
        else if (WinCondition == "Меньше чем")
        {
            WinCondition = "Smaller than";
        }
        else if (WinCondition == "Чётное число")
        {
            WinCondition = "Even";
        }
        else if (WinCondition == "Нечётное число")
        {
            WinCondition = "Odd";
        }

        GameObject windConditionNumber = GameObject.Find("Win Condition Number");
        TMP_InputField windConditionNumberValue;
        windConditionNumber.TryGetComponent<TMP_InputField>(out windConditionNumberValue);
        WinConditionNumber = windConditionNumberValue.text;

        GameObject basketHighscoreMultiplyer = GameObject.Find("Basket Highscore Multiplier");
        TMP_Dropdown basketHighscoreMultiplyerValue;
        basketHighscoreMultiplyer.TryGetComponent<TMP_Dropdown>(out basketHighscoreMultiplyerValue);
        HighscoreMultiplier = basketHighscoreMultiplyerValue.options[basketHighscoreMultiplyerValue.value].text;

        GameObject basketSpawnRate = GameObject.Find("Basket Spawn Rate");
        TMP_Dropdown basketSpawnRateValue;
        basketSpawnRate.TryGetComponent<TMP_Dropdown>(out basketSpawnRateValue);
        SpawnRate = basketSpawnRateValue.options[basketSpawnRateValue.value].text;

        basketSettings.Add("MovementSpeed", MovementSpeed);
        basketSettings.Add("SpawnRange", SpawnRange);
        basketSettings.Add("WinCondition", WinCondition);
        basketSettings.Add("WinConditionNumber", WinConditionNumber);
        basketSettings.Add("HighscoreMultiplier", HighscoreMultiplier);
        basketSettings.Add("SpawnRate", SpawnRate);

        gameSettings.gameSettings[key] = basketSettings;

        SaveSettings();
    }

    private void SaveFaultyRobotSettings()
    {
        string key = GetKey();
        if (key == null)
        {
            Debug.Log("And error has occured, current minigame is set to null!");
            return;
        }

        Dictionary<string, string> faultyRobotSettings = new Dictionary<string, string>();

        string Button1X;
        string Button1Y;
        string Button2X;
        string Button2Y;
        string Button3X;
        string Button3Y;
        string TargetX;
        string TargetY;
        string HighscoreMultiplier;

        GameObject button1X = GameObject.Find("Button 1 X");
        TMP_Dropdown button1XValue;
        button1X.TryGetComponent<TMP_Dropdown>(out button1XValue);
        Button1X = button1XValue.options[button1XValue.value].text;

        GameObject button1Y = GameObject.Find("Button 1 Y");
        TMP_Dropdown button1YValue;
        button1Y.TryGetComponent<TMP_Dropdown>(out button1YValue);
        Button1Y = button1YValue.options[button1YValue.value].text;

        GameObject button2X = GameObject.Find("Button 2 X");
        TMP_Dropdown button2XValue;
        button2X.TryGetComponent<TMP_Dropdown>(out button2XValue);
        Button2X = button2XValue.options[button2XValue.value].text;

        GameObject button2Y = GameObject.Find("Button 2 Y");
        TMP_Dropdown button2YValue;
        button2Y.TryGetComponent<TMP_Dropdown>(out button2YValue);
        Button2Y = button2YValue.options[button2YValue.value].text;

        GameObject button3X = GameObject.Find("Button 3 X");
        TMP_Dropdown button3XValue;
        button3X.TryGetComponent<TMP_Dropdown>(out button3XValue);
        Button3X = button3XValue.options[button3XValue.value].text;

        GameObject button3Y = GameObject.Find("Button 3 Y");
        TMP_Dropdown button3YValue;
        button3Y.TryGetComponent<TMP_Dropdown>(out button3YValue);
        Button3Y = button3YValue.options[button3YValue.value].text;

        GameObject targetX = GameObject.Find("Target X");
        TMP_Dropdown targetXValue;
        targetX.TryGetComponent<TMP_Dropdown>(out targetXValue);
        TargetX = targetXValue.options[targetXValue.value].text;

        GameObject targetY = GameObject.Find("Target Y");
        TMP_Dropdown targetYValue;
        targetY.TryGetComponent<TMP_Dropdown>(out targetYValue);
        TargetY = targetYValue.options[targetYValue.value].text;

        GameObject faultyRobotHighscoreMultiplier = GameObject.Find("Faulty Robot Highscore Multiplier");
        TMP_Dropdown faultyRobotHighscoreMultiplierValue;
        faultyRobotHighscoreMultiplier.TryGetComponent<TMP_Dropdown>(out faultyRobotHighscoreMultiplierValue);
        HighscoreMultiplier = faultyRobotHighscoreMultiplierValue.options[faultyRobotHighscoreMultiplierValue.value].text;

        faultyRobotSettings.Add("Button1X", Button1X);
        faultyRobotSettings.Add("Button1Y", Button1Y);
        faultyRobotSettings.Add("Button2X", Button2X);
        faultyRobotSettings.Add("Button2Y", Button2Y);
        faultyRobotSettings.Add("Button3X", Button3X);
        faultyRobotSettings.Add("Button3Y", Button3Y);
        faultyRobotSettings.Add("TargetX", TargetX);
        faultyRobotSettings.Add("TargetY", TargetY);
        faultyRobotSettings.Add("HighscoreMultiplier", HighscoreMultiplier);

        gameSettings.gameSettings[key] = faultyRobotSettings;
        SaveSettings();
    }
    private void SaveNumberJumbleSettings()
    {
        string key = GetKey();
        if (key == null)
        {
            Debug.Log("And error has occured, current minigame is set to null!");
            return;
        }

        Dictionary<string, string> numberJumbleSettings = new Dictionary<string, string>();

        string Button1;
        string Button2;
        string Button3;
        string TargetNumber;
        string HighscoreMultiplier;

        GameObject button1 = GameObject.Find("Button1");
        TMP_InputField button1Value;
        button1.TryGetComponent<TMP_InputField>(out button1Value);
        Button1 = button1Value.text;

        GameObject button2 = GameObject.Find("Button2");
        TMP_InputField button2Value;
        button2.TryGetComponent<TMP_InputField>(out button2Value);
        Button2 = button2Value.text;

        GameObject button3 = GameObject.Find("Button3");
        TMP_InputField button3Value;
        button3.TryGetComponent<TMP_InputField>(out button3Value);
        Button3 = button3Value.text;

        GameObject targetNumber = GameObject.Find("Target Number");
        TMP_InputField targetNumberValue;
        targetNumber.TryGetComponent<TMP_InputField>(out targetNumberValue);
        TargetNumber = targetNumberValue.text;

        GameObject numberJumbleHighscoreMultiplier = GameObject.Find("Number Jumble Highscore Multiplier");
        TMP_Dropdown numberJumbleHighscoreMultiplierValue;
        numberJumbleHighscoreMultiplier.TryGetComponent<TMP_Dropdown>(out numberJumbleHighscoreMultiplierValue);
        HighscoreMultiplier = numberJumbleHighscoreMultiplierValue.options[numberJumbleHighscoreMultiplierValue.value].text;

        numberJumbleSettings.Add("Button1", Button1);
        numberJumbleSettings.Add("Button2", Button2);
        numberJumbleSettings.Add("Button3", Button3);
        numberJumbleSettings.Add("TargetNumber", TargetNumber);
        numberJumbleSettings.Add("HighscoreMultiplier", HighscoreMultiplier);

        gameSettings.gameSettings[key] = numberJumbleSettings;

        SaveSettings();
    }

    private void SaveSettings()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "setTitleData",
            FunctionParameter = new
            {
                key = PlayerPrefs.GetString("EMAIL"),
                value = JsonConvert.SerializeObject(gameSettings)
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, resultCallback => { Debug.Log("Executed the script successfully"); }, errorCallback => { Debug.Log("Failed executing the script"); });
    }
    #endregion TeacherPanel
}
