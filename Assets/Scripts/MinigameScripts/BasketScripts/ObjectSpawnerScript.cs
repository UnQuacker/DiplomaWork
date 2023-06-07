using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class ObjectSpawnerScript : MonoBehaviour, ISettingsPresistance
{
    public GameObject[] gameObjects;
    public float timeBetweenSpawns = 1f;
    public float minX = -2f;
    public float maxX = 2f;
    public int range=10;
    public string minigameId;
    public int targetNumber;

    private Func<int,int, bool> WinChecker = (a,b)=>false;

    private string winCondition;
    void Start()
    {
        Debug.Log("timeBetweenSpawns: " + timeBetweenSpawns);
        StartCoroutine(ObjectSpawner());
    }

    // Update is called once per frame
    IEnumerator ObjectSpawner()
    {
        if (Time.timeScale == 0)
        {
            yield return new WaitForSecondsRealtime(timeBetweenSpawns);
        }
        while (true)
        {
            var pos = UnityEngine.Random.Range(minX, maxX);
            var spawnPosition = new Vector3(pos, transform.position.y+4);
            var number = UnityEngine.Random.Range(1, range);
            GameObject gameObject = Instantiate(gameObjects[UnityEngine.Random.Range(0, gameObjects.Length)], spawnPosition, Quaternion.identity);
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
            if(WinChecker(number,targetNumber))
            {
                gameObject.tag = "CorrectFruit";
            }
            else
            {
                gameObject.tag = "InCorrectFruit";
            }
            yield return new WaitForSeconds(timeBetweenSpawns);

        }

    }

    public void LoadSettings(GameSettings gameSettings)
    {
        Dictionary<string, string> settings = gameSettings.gameSettings[minigameId];

        string tempValue = "";

        settings.TryGetValue("SpawnRate", out tempValue);
        timeBetweenSpawns = float.Parse(tempValue, CultureInfo.InvariantCulture.NumberFormat);

        settings.TryGetValue("SpawnRange", out tempValue);
        range = int.Parse(tempValue);

        settings.TryGetValue("WinCondition", out tempValue);
        winCondition = tempValue;

        switch (winCondition)
        {
            case "Greater than":
                WinChecker = (a, b) => (a > b);
                break;
            case "Less than":
                WinChecker = (a, b) => (a < b);
                break;
            case "Even":
                WinChecker = (a, b) => (a % 2 == 0);
                break;
            case "Odd":
                WinChecker = (a, b) => (a % 2 != 0);
                break;

        }

        settings.TryGetValue("WinConditionNumber", out tempValue);
        if (tempValue != null)
        {
            targetNumber = int.Parse(tempValue);
        }
    }
}
