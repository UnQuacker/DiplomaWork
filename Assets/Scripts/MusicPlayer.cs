using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public Scrollbar musicScrollbar;
    private AudioSource music;
    public GameObject musicSettings;

    private void Awake()
    {
        GameObject musicGameObject = GameObject.FindWithTag("GameMusic");
        music = musicGameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("Music Volume"))
        {
            music.volume = PlayerPrefs.GetFloat("Music Volume");
            musicScrollbar.value = PlayerPrefs.GetFloat("Music Volume");
        }
    }
    public void ChangeMusicVolume()
    {
        music.volume = musicScrollbar.value;
    }

    public void ClosePanel(bool close)
    {
        if (close)
        {
            PlayerPrefs.SetFloat("Music Volume", musicScrollbar.value);
            PlayerPrefs.Save();
            musicSettings.gameObject.SetActive(!close);
        }
        else
        {
            musicSettings.gameObject.SetActive(!close);
        }
    }

}
