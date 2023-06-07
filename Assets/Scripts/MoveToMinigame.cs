using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToMinigame : MonoBehaviour
{
    [SerializeField]
    public int scene;
    void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(scene);
    }
    public void MoveToScene()
    {
        SceneManager.LoadScene(scene);
    }
}
