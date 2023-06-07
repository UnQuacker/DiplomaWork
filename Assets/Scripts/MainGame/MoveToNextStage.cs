using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MoveToNextStage : MonoBehaviour, IDataPersistence
{

    public string[] minigamesIDs;

    public int nextScene = 5;
    private bool canProceed = true;

    public UnityEvent canEnter;
    public void LoadData(GameData gameData)
    {
        foreach(string minigameId in minigamesIDs)
        {
            bool isComplete;
            gameData.minigamesCompleted.TryGetValue(minigameId, out isComplete);
            if (!isComplete)
            {
                canProceed = false;
            }
        }
        if (canProceed)
        {
            canEnter.Invoke();
        }

    }


    public void SaveData(ref GameData gameData)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(nextScene);
    }

}
