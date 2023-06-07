using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using PlayFab.ClientModels;
using PlayFab;
using Newtonsoft.Json;

public class DataPresistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private GameSettings gameSettings;

    //private bool loadGameDataComplete = false;
    //private bool loadSettingsComplete = false;
    //private bool firstLoad = true;
    public static DataPresistanceManager instance { get; private set; }
    [SerializeField] private GameObject loadingScreen;


    private List<IDataPersistence> dataPersistenceObjects;
    private List<ISettingsPresistance> settingsPresistanceObjects;


    //private FileDataHandler fileDataHandler;
    private void Awake()
    {
        //this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        //Debug.Log("Awake for DataPresistanceManager is called");
        if (instance != null)
        {
            //Debug.Log("A new DataPresistanceManager has been deleted");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        //Debug.Log("Calling Login for DataPresistanceManager is called");
        //Login();

    }
    private void NewGame()
    {
        this.gameData = new GameData();
        this.gameSettings = new GameSettings();
        
    }

    public void LoadGame()
    {
        //if (fileDataHandler == null)
        //{
        //    Debug.Log("FileHandler is null!");
        //}
        //this.gameData = fileDataHandler.Load();
        //if (!loadGameDataComplete || !loadSettingsComplete)
        //{
        //    return;
        //}
        //Debug.Log("Called LoadGame() method");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        this.settingsPresistanceObjects = FindAllSettingsPersistanceObjects();
        foreach (ISettingsPresistance settingsPresistanceObj in settingsPresistanceObjects)
        {
            settingsPresistanceObj.LoadSettings(gameSettings);
        }
        loadingScreenControl();
        Debug.Log("loading screen 2nd call " + loadingScreen.activeSelf);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("CURRENTLEVEL", SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Current Level saved: " + SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        CallPlayFabApiSaveData();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CURRENTLEVEL", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
        SaveGame();
        //PlayerPrefs.DeleteAll();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    private List<ISettingsPresistance> FindAllSettingsPersistanceObjects()
    {
        IEnumerable<ISettingsPresistance> settingsPresistanceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<ISettingsPresistance>();
        return new List<ISettingsPresistance>(settingsPresistanceObjects);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene Loaded");
        //Login(false);
        //if (firstLoad==false)
        //{
        //    Debug.Log("OnSceneLoaded");
        //    Login();
        //}
        loadingScreenControl();
        Debug.Log("loading screen 1st call " + loadingScreen.activeSelf);
        Login();
    }

    //private void CallPlayFabApiRegisterAndLoad()
    //{
    //    this.gameData = fileDataHandler.Load();
    //    if (this.gameData == null)
    //    {
    //        Debug.Log("new game");
    //        NewGame();
    //    }

    //    foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
    //    {
    //        dataPersistenceObj.LoadData(gameData);
    //    }
    //}

    private void Login()
    {
        if (PlayerPrefs.HasKey("USERNAME"))
        {
            string username = PlayerPrefs.GetString("USERNAME");
            string password = PlayerPrefs.GetString("PASSWORD");
            Debug.Log("username: " + username);
            var request = new LoginWithPlayFabRequest { Username = username, Password = password };
            PlayFabClientAPI.LoginWithPlayFab(request, resultCallback=>{
                Debug.Log("Logged In");
                LoadData();
                //if (firstLoad == true)
                //{
                //    firstLoad = false;
                //}
                return;
            }, errorCallback=> { Debug.Log("Could not Load User Information!"); });
        }
        else
        {
            Debug.Log("PlayerPrefs does not have a key USERNAME ");
        }
    }
    private void LoadData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), resultCallback => {
            OnSuccessDeserialize(resultCallback);
        }, errorCallback => { Debug.Log("error when loading the data"); });
    }
    private void OnSuccessDeserialize(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("GameData") && result.Data.ContainsKey("Teacher"))
        {
            gameData = JsonConvert.DeserializeObject<GameData>(result.Data["GameData"].Value);

            Teacher tempTeacher = new Teacher();
            tempTeacher = JsonConvert.DeserializeObject<Teacher>(result.Data["Teacher"].Value);

            LoadSettings(tempTeacher.teacher);
            //loadGameDataComplete = true;
            //continueLoadFlag = true;
            //Debug.Log("x coordinate loaded in is: " + loadedData.coordinates[0].GetValue(0)); 
            //Debug.Log("Data loaded in is: " + result.Data["GameData"].ToString());
        }
        else
        {
            Debug.Log("No game data is found, initializing a new game");
            NewGame();
            LoadGame();
        }
    }

    private void CallPlayFabApiSaveData()
    {
        string username = PlayerPrefs.GetString("USERNAME");
        //Debug.Log("username in CallPlayFabApiSaveData is: " + username);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"GameData", JsonConvert.SerializeObject(gameData, Formatting.Indented)}
            }
        };
        PlayFabClientAPI.UpdateUserData(
            request,
            resultCallback => { Debug.Log("success "); },
            errorCallback => { Debug.Log("error"); }
            );
        //yield return new WaitWhile(() => !continueSaveFlag); 
    }

    private void loadingScreenControl()
    {
        if (loadingScreen.activeSelf==false)
        {
            loadingScreen.SetActive(true);
            Debug.Log("The time has stopped");
            Time.timeScale = 0f;
        }
        else if (loadingScreen.activeSelf == true)
        {
            loadingScreen.SetActive(false);
            Debug.Log("The time is running again");
            Time.timeScale = 1f;
        }
    }

    private void LoadSettings(string teacherName)
    {
        List<string> keys = new List<string>();
        var request = new GetTitleDataRequest { Keys = keys };
        PlayFabClientAPI.GetTitleData(request, resultCallback=> { onLoadSettings(resultCallback, teacherName); }, errorCallback => { Debug.Log("LoadSettings failed!"); });
    }
    private void onLoadSettings(GetTitleDataResult result, string teacherName)
    {
        foreach (var data in result.Data)
        {
            if(data.Key == teacherName)
            {
                this.gameSettings = JsonConvert.DeserializeObject<GameSettings>(data.Value);
                Debug.Log("Set game settings");
            }
        }
        LoadGame();
    }
}