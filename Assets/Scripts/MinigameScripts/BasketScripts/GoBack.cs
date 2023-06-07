using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBack : MonoBehaviour
{
    [SerializeField]
    public int level_id;
    public void GoBackButton()
    {
        SceneManager.LoadScene(level_id);
    }
}
