using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "basket" && (gameObject.tag == "CorrectFruit"|| gameObject.tag == "InCorrectFruit"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "FruitShredder")
        {
            Destroy(gameObject);
        }
    }
}
