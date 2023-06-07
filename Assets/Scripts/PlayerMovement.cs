using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    public float movementSpeed = 5f;
    public Rigidbody2D rigidBody2D;
    public GameObject playerObject;
    public Animator animator;
    public int scene;
    Vector2 movement;

    public GameObject musicPanel;

    private float[] coordinates;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            musicPanel.SetActive(true);
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (movement.sqrMagnitude != 0)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        
    }

    private void FixedUpdate()
    {
        rigidBody2D.MovePosition(rigidBody2D.position + movement.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    public void LoadData(GameData gameData)
    {
        gameData.coordinates.TryGetValue(scene, out coordinates);
        Debug.Log("Moved the player to:"+ coordinates[0]+" " + coordinates[1] + " " + coordinates[2] + " ");
        Vector3 position = new Vector3(coordinates[0], coordinates[1], coordinates[2]);
        this.transform.position = position;

    }

    public void SaveData(ref GameData gameData)
    {
        coordinates = new float[3];
        coordinates[0] = this.transform.position.x;
        coordinates[1] = this.transform.position.y;
        coordinates[2] = this.transform.position.z;

        if (gameData.coordinates.ContainsKey(scene))
        {
            gameData.coordinates.Remove(scene);
        }
        gameData.coordinates.Add(scene, coordinates);
    }
}
