using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialoguePanelBasket : MonoBehaviour, IDataPersistence
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogueNotWonInput;
    public string[] dialogueWonInput;


    private int dialogueIndexNotWon;
    private int dialogueIndexWon;


    public bool isGameBeaten;

    public GameObject continueButton;
    public bool isPlayerClose;
    public float dialogueSpeed;

    public string minigameId;

    public int scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerClose = false;
            ResetDialoguePanel();
        }
    }

    private void Start()
    {
        dialogueText.text = "";

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && isPlayerClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                ResetDialoguePanel();
            }

            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
        if (!isGameBeaten)
        {
            if (dialogueText.text == dialogueNotWonInput[dialogueIndexNotWon])
            {
                continueButton.SetActive(true);
            }
        }
        else
        {
            if (dialogueText.text == dialogueWonInput[dialogueIndexWon])
            {
                continueButton.SetActive(true);
            }
        }
    }

    IEnumerator Typing()
    {
        if (isGameBeaten)
        {
            foreach (char letter in dialogueWonInput[dialogueIndexNotWon].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(dialogueSpeed);
            }
        }
        else if (!isGameBeaten)
        {
            foreach (char letter in dialogueNotWonInput[dialogueIndexNotWon].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(dialogueSpeed);
            }
        }
    }

    public void nextLine()
    {
        continueButton.SetActive(false);


        if (!isGameBeaten && dialogueIndexNotWon < dialogueNotWonInput.Length - 1)
        {
            dialogueIndexNotWon++;
            dialogueText.text = "";
            StartCoroutine(Typing());

        }
        else if (isGameBeaten && dialogueIndexWon < dialogueWonInput.Length - 1)
        {
            dialogueIndexWon++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ResetDialoguePanel();
            if (!isGameBeaten)
            {
                DataPresistanceManager.instance.SaveGame();
                SceneManager.LoadScene(scene);
            }
        }
    }

    public void ResetDialoguePanel()
    {
        dialogueText.text = "";
        dialogueIndexNotWon = 0;
        dialogueIndexWon = 0;
        dialoguePanel.SetActive(false);
    }

    public void LoadData(GameData gameData)
    {
        gameData.minigamesCompleted.TryGetValue(minigameId, out isGameBeaten);
    }

    public void SaveData(ref GameData gameData)
    {
        if (gameData.minigamesCompleted.ContainsKey(minigameId))
        {
            gameData.minigamesCompleted.Remove(minigameId);
        }
        gameData.minigamesCompleted.Add(minigameId, isGameBeaten);
    }
}
