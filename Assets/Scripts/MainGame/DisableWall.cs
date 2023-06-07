using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWall : MonoBehaviour
{
    public void disableWall()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
