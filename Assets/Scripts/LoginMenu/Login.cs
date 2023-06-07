using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using PlayFab.ProfilesModels;

public class Login : MonoBehaviour
{
    public GameObject teacherLoginPanel;
    public GameObject teacherPanel;
    public TMP_InputField teacherEmail;
    public TMP_InputField teacherPassword;
    public TMP_InputField studentName;
    public TMP_InputField studentPassword;
    public TMP_InputField studentTeacher;

    public GameObject leaderboardPanel;
    public GameObject listingPrefab;

    public Transform listingContainer;

    public TMP_Text teacherErrorMessage;
    public TMP_Text studentErrorMessage;

    #region TeacherLogin
    private void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteKey("EMAIL");
        PlayerPrefs.DeleteKey("PASSWORD");
        PlayerPrefs.DeleteKey("USERNAME");
        PlayerPrefs.DeleteKey("EMAIL");
    }
    public void EnableTeacherPanel()
    {
        if (teacherLoginPanel.activeSelf)
        {
            teacherLoginPanel.SetActive(false);
            teacherPanel.SetActive(true);
        }
        else
        {
            teacherLoginPanel.SetActive(true);
            teacherPanel.SetActive(false);
        }
    }
    public void OnClickLoginTeacher()
    {
        if (teacherEmail.text!="" && teacherPassword.text != "")
        {
            var request = new LoginWithEmailAddressRequest { Email = teacherEmail.text, Password = teacherPassword.text };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnTeacherLoginSuccess, OnTeacherLoginFailure);
        }
    }
    private void OnTeacherLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged in as a teacher");
        EnableTeacherPanel();
        DeletePlayerPrefs();
        PlayerPrefs.SetString("EMAIL", teacherEmail.text);
        PlayerPrefs.SetString("PASSWORD", teacherPassword.text);
        PlayerPrefs.Save();
    }
    private void OnTeacherLoginFailure(PlayFabError error)
    {
        var request = new RegisterPlayFabUserRequest { Email = teacherEmail.text, Password = teacherPassword.text, RequireBothUsernameAndEmail = false, DisplayName = teacherEmail.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnTeacherRegisterSuccess, errorCallback=> { teacherErrorMessage.text = "Введены Неверные Данные!"; });
    }
    private void OnTeacherRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SetGameSettings();
        EnableTeacherPanel();
        DeletePlayerPrefs();
        PlayerPrefs.SetString("EMAIL", teacherEmail.text);
        PlayerPrefs.SetString("PASSWORD", teacherPassword.text);
        PlayerPrefs.Save();
    }
    #endregion TeacherLogin

    #region StudentLogin
    int currentLevel = 1;
    public void OnClickLoginStudent()
    {
        if (studentName.text != "" & studentPassword.text != "")
        {
            var request = new LoginWithPlayFabRequest { Username = studentName.text, Password = studentPassword.text };
            PlayFabClientAPI.LoginWithPlayFab(request, OnStudentLoginSuccess, OnStudentLoginFailure);
        }
    }
    private void OnStudentLoginSuccess(LoginResult result)
    {
        DeletePlayerPrefs();
        Debug.Log("Login success as " + studentName.text);
        PlayerPrefs.SetString("USERNAME", studentName.text);
        PlayerPrefs.SetString("PASSWORD", studentPassword.text);
        currentLevel = PlayerPrefs.GetInt("CURRENTLEVEL", currentLevel);
        Debug.Log("Current Level: " + currentLevel);
        PlayerPrefs.Save();
        SceneManager.LoadScene(currentLevel);
    }
    private void OnStudentLoginFailure(PlayFabError error)
    {
        var request = new RegisterPlayFabUserRequest { Username = studentName.text, Password = studentPassword.text , RequireBothUsernameAndEmail  = false, DisplayName = studentName.text};
        PlayFabClientAPI.RegisterPlayFabUser(request, OnStudentRegisterSuccess, errorCallback => { studentErrorMessage.text = "Введены Неверные Данные!"; });
    }
    private void OnStudentRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SetStudentTeacher();
        DeletePlayerPrefs();
        Debug.Log("Register success ");
        PlayerPrefs.SetString("USERNAME", studentName.text);
        PlayerPrefs.SetString("PASSWORD", studentPassword.text);
        PlayerPrefs.Save();
        setStudentHighscore();
    }

    private void setStudentHighscore()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "Highscore", Value = 0 },
    }
        }, result => { SceneManager.LoadScene(currentLevel); }, error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    private void SetStudentTeacher()
    {
        Teacher teacher = new Teacher();
        teacher.teacher = studentTeacher.text;
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Teacher", JsonConvert.SerializeObject(teacher)}
            },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(
            request,
            resultCallback => { Debug.Log("success"); },
            errorCallback => { Debug.Log("error"); }
            );
    }
    #endregion StudentLogin

    #region Leaderboard

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest { StatisticName = "Highscore", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, errorCallback=> { });
    }
    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        teacherPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        int counter = 0;
        foreach(PlayerLeaderboardEntry entry in result.Leaderboard)
        {
            counter++;
            var request = new GetUserDataRequest { PlayFabId = entry.PlayFabId.ToString() };
            PlayFabClientAPI.GetUserData(request, resultCallback => {
                if (resultCallback.Data.ContainsKey("Teacher"))
                {
                    Teacher teacher = JsonConvert.DeserializeObject<Teacher>(resultCallback.Data["Teacher"].Value);
                    Debug.Log("teacher.teacher: " + teacher.teacher);
                    Debug.Log("PlayerPrefs.GetString(EMAIL)): " + PlayerPrefs.GetString("EMAIL"));
                    Debug.Log(entry.DisplayName);
                    if (teacher.teacher == PlayerPrefs.GetString("EMAIL"))
                    {
                        Debug.Log("Here!");
                        GameObject tempListing = Instantiate(listingPrefab, listingContainer);
                        LeaderboardListing listing = tempListing.GetComponent<LeaderboardListing>();
                        listing.playerNameText.text = entry.DisplayName;
                        listing.playerScoreText.text = entry.StatValue.ToString();
                    }
                }
                else
                {
                    Debug.Log("WHAT DA FUCK?!!");
                }
            }, errorCallback => { Debug.Log(errorCallback.ErrorMessage); });
        }
        Debug.Log(counter);
    }
    public void CloseLeaderboardPanel()
    {
        teacherPanel.SetActive(true);
        leaderboardPanel.SetActive(false);
        for(int i=listingContainer.childCount-1; i>=0; i--)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }
    }



    #endregion Leaderboard

    //public void createStats()
    //{
    //    PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
    //    {
    //        Statistics = new List<StatisticUpdate>
    //        {
    //            new StatisticUpdate { StatisticName = "Highscore", Value = 5},
    //        }
    //    },
    //    resultCallback => { },
    //    errorCallback => { }
    //    );
    //}

    //private void Test()
    //{
    //    var request = new GetEntityProfilesRequest {DataAsObject = false };
    //    PlayFabProfilesAPI.GetProfiles(request, resultCallback => {

    //    }, errorCallback => { 

    //    });
    //}
    //private void onTestSuccess(GetEntityProfilesResponse responses)
    //{
    //    foreach(var response in responses.Profiles)
    //    {
    //        if (response.Statistics.ContainsKey("Teacher"))
    //        {
    //            response.

    //        }
    //    }

    //}

    #region gameSettings
    public void SetGameSettings()
    {
        string teacher = PlayerPrefs.GetString("EMAIL");
        GameSettings gameSettings = new GameSettings();
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "setTitleData",
            FunctionParameter = new
            {
                key = teacher,
                value = JsonConvert.SerializeObject(gameSettings)
            }

        };
        PlayFabClientAPI.ExecuteCloudScript(request, resultCallback => { Debug.Log("Executed the script successfully"); }, errorCallback => { Debug.Log("Failed executing the script"); });
    }

    //public void getCloudDataTest()
    //{
    //    List<string> keys = new List<string>();
    //    keys.Add(PlayerPrefs.GetString("EMAIL"));
    //    var request = new GetTitleDataRequest {Keys = keys};
    //    PlayFabClientAPI.GetTitleData(request, onGetCloudDataTest, errorCallback => { Debug.Log("getCloudDataTest failed!"); });
    //}
    //private void onGetCloudDataTest(GetTitleDataResult result)
    //{
    //    foreach (var data in result.Data)
    //    {
    //        Teacher teacher = JsonConvert.DeserializeObject<Teacher>(data.Value);
    //        string key = data.Key;
    //        Debug.Log("Title Data - Key: " + key + ", Value: " + teacher.teacher);
    //    }
    //}
    #endregion gameSettings
}
