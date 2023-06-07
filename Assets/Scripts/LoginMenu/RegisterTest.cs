using System;
using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class RegisterTest : MonoBehaviour
{
    private bool continueFlag = false;
    private bool continueSaveFlag = false;
    private void Start()
    {
        Login();
    }
    public void Login()
    {
        StartCoroutine(CallPlayFabApi());
    }
    private void OnSuccess(LoginResult loginResult)
    {
        Debug.Log("Register Test Success!");
        continueFlag = true;
    }
    private void OnError(PlayFabError playFabError)
    {
        Debug.Log("error");
        Debug.Log(playFabError.GenerateErrorReport());
    }

    public void SaveData()
    {
        //DataPresistanceManager.instance.SaveGame();
    }

    IEnumerator CallPlayFabApi()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
        yield return new WaitWhile(() => !continueFlag);
        //Debug.Log("Quack");
        continueFlag = false;
    }

    public void CreateDummyData()
    {
        GameData dummyData = new GameData();

        string dataToStore = JsonConvert.SerializeObject(dummyData, Formatting.Indented);
        Debug.Log(dataToStore);

        using (FileStream stream = new FileStream(Application.persistentDataPath + "\\TestData.txt", FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
        }
        StartCoroutine(SaveDataToPlayFab(dummyData));
    }
    IEnumerator SaveDataToPlayFab(GameData dummyData)
    {
        var saveRequest = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"GameData", JsonConvert.SerializeObject(dummyData, Formatting.Indented)}
            }
        };

        PlayFabClientAPI.UpdateUserData(
            saveRequest,
            resultCallback =>
            {
                Debug.Log("Success");
                continueSaveFlag = true;
            },
            errorCallback => { Debug.Log("Error"); }
        );
        yield return new WaitWhile(() => !continueSaveFlag);
    }
}
