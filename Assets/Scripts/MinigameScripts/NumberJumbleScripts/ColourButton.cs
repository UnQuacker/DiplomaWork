using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ColourButton : MonoBehaviour
{
    public UnityEvent pressedButton;
    public int number;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        pressedButton.Invoke();
    }
}
