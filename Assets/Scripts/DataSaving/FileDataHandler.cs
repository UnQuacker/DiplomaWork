using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System.Threading;

public class FileDataHandler
{
    private string dirPath = "";
    private string dataFileName = "";
    //GameData loadedData = null;
    //private bool isLoaded = false;

    public FileDataHandler(string dirPath, string dataFileName)
    {
        this.dirPath = dirPath;
        this.dataFileName = dataFileName;
        //Login();
    }

    //public GameData Load()
    //{
        //string fullPath = Path.Combine(dirPath, dataFileName);
        //if (File.Exists(fullPath)) 
        //{ 
        //    try 
        //    { 
        //        // load the serialized data from the file 
        //        string dataToLoad = ""; 
        //        using (FileStream stream = new FileStream(fullPath, FileMode.Open)) 
        //        { 
        //            using (StreamReader reader = new StreamReader(stream)) 
        //            { 
        //                dataToLoad = reader.ReadToEnd(); 
        //            } 
        //        } 

        //        // deserialize the data from Json back into the C# object 
        //        loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad); 
        //    } 
        //    catch (Exception e) 
        //    { 
        //        Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e); 
        //    } 
        //} 
        //Debug.Log("x coordinate loaded in is: " + loadedData.coordinates[0].GetValue(0)); 

        //Debug.Log("FileDataHandler is Used!");
        //getUserDataFromPlayFab();
        //Debug.Log("Loaded Data");
        //return this.loadedData;
    //}
    //private void getUserDataFromPlayFab()
    //{
    //    //Debug.Log("Called getUserDataFromPlayFab");
    //    PlayFabClientAPI.GetUserData(new GetUserDataRequest(), resultCallback=> {
    //        OnSuccessDeserialize(resultCallback);
    //    }, errorCallback => { Debug.Log("error"); });
    //    //Debug.Log("Stopped getUserDataFromPlayFab");
    //}

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(gameData, Formatting.Indented);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }

        CallPlayFabApiSaveData(gameData);

    }

    //private void OnSuccessDeserialize(GetUserDataResult result)
    //{
    //    if (result.Data != null && result.Data.ContainsKey("GameData"))
    //    {
    //        this.loadedData = JsonConvert.DeserializeObject<GameData>(result.Data["GameData"].Value);
    //        //continueLoadFlag = true;
    //        //Debug.Log("x coordinate loaded in is: " + loadedData.coordinates[0].GetValue(0)); 
    //        Debug.Log("Data loaded in is: " + result.Data["GameData"].ToString());
    //    }
    //    else
    //    {
    //        Debug.Log("Loaded Data is F'd up, m8");
    //    }
    //}

    private void CallPlayFabApiSaveData(GameData gameData)
    {
        string username = PlayerPrefs.GetString("USERNAME");
        Debug.Log("username in CallPlayFabApiSaveData is: " + username );
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
    //private void Login()
    //{
    //    if (PlayerPrefs.HasKey("USERNAME"))
    //    {
    //        string username = PlayerPrefs.GetString("USERNAME");
    //        string password = PlayerPrefs.GetString("PASSWORD");
    //        var request = new LoginWithPlayFabRequest { Username = username, Password = password };
    //        PlayFabClientAPI.LoginWithPlayFab(request, resultCallback => {
    //            getUserDataFromPlayFab();
    //        }, errorCallback => { Debug.Log("Could not Load User Information!"); });
    //    }
    //    else
    //    {
    //        Debug.Log("PlayerPrefs does not have a key USERNAME ");
    //    }
    //}
}