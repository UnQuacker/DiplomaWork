using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour, ISettingsPresistance
{
    public GameObject player;
    public Button moveButton1;
    public Button moveButton2;
    public Button moveButton3;
    public Button resetButton;
    public string minigameId;
    public string id;
    public float x;
    public float y;

    private float xTargetPosition;
    private float yTargetPosition;
    public float speed = 1f;

    private bool isMovingX = false;
    private bool isMovingY = false;

    private float threshold = 0.08f;

    public void MoveToXTarget()
    {
        if (!isMovingX)
        {
            isMovingX = true;
            player.tag = "Moving";
            StartCoroutine(MoveXCoroutine());
        }
    }
    public void MoveToYTarget()
    {
        if (!isMovingY && !isMovingX)
        {
            isMovingY = true;
            StartCoroutine(MoveYCoroutine());
        }
    }

    public void MoveToXYTarget()
    {
        xTargetPosition = player.transform.position.x + x;
        yTargetPosition = player.transform.position.y + y;

        if (yTargetPosition<=3.5 && yTargetPosition >= -3.5 && xTargetPosition >= -7.5 && xTargetPosition <= 3.5)
        {
            setInteractable(false);
            MoveToXTarget();
        }

    }

    private IEnumerator MoveXCoroutine()
    {
        Vector3 targetPosition = new Vector3(xTargetPosition, player.transform.position.y, player.transform.position.z);

        while (Mathf.Abs(player.transform.position.x - xTargetPosition) > threshold)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        player.transform.position = targetPosition;
        isMovingX = false;
        MoveToYTarget();
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator MoveYCoroutine()
    {
        while (isMovingX)
        {
            yield return null;
        }

        Vector3 targetPosition = new Vector3(player.transform.position.x, yTargetPosition, player.transform.position.z);

        while (Mathf.Abs(player.transform.position.y - yTargetPosition) > threshold)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        player.tag = "Player";
        player.transform.position = targetPosition;

        isMovingY = false;
        setInteractable(true);
    }

    public void ResetRobotPosition()
    {
        player.transform.position = new Vector3(-7.5f, -3.5f, 0);
    }

    private void setInteractable(bool interactable)
    {
        moveButton1.interactable = interactable;
        moveButton2.interactable = interactable;
        moveButton3.interactable = interactable;
        resetButton.interactable = interactable;
    }

    public void LoadSettings(GameSettings gameSettings)
    {
        if(id == "ResetButton")
        {
            return;
        }
        Dictionary<string, string> settings = gameSettings.gameSettings[minigameId];
        string tempValue;

        settings.TryGetValue(id +"X", out tempValue);
        //Debug.Log(id);
        x = float.Parse(tempValue, CultureInfo.InvariantCulture.NumberFormat);

        settings.TryGetValue(id + "Y", out tempValue);
        y = float.Parse(tempValue, CultureInfo.InvariantCulture.NumberFormat);
    }
}
