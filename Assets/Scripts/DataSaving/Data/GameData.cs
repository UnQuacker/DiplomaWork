using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    public Dictionary<string, bool> minigamesCompleted;
    public Dictionary<int, float[]> coordinates;

    public GameData()
    {
        minigamesCompleted = new Dictionary<string, bool>();
        coordinates = new Dictionary<int, float[]>();
        coordinates.Add(1, new float[3] { 0, 0, 0 });
        coordinates.Add(5, new float[3] { 0, 0, 0 });
        coordinates.Add(9, new float[3] { 10, 0, 0 });
        coordinates.Add(13, new float[3] { 10, 0, 0 });
        coordinates.Add(17, new float[3] { 10, 0, 0 });
        coordinates.Add(21, new float[3] { 10, 0, 0 });
    }
}
