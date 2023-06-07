using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    private Dictionary<string, string> basket1;
    private Dictionary<string, string> numberJumble1;
    private Dictionary<string, string> faultyRobot1;

    private Dictionary<string, string> basket2;
    private Dictionary<string, string> numberJumble2;
    private Dictionary<string, string> faultyRobot2;

    private Dictionary<string, string> basket3;
    private Dictionary<string, string> numberJumble3;
    private Dictionary<string, string> faultyRobot3;

    private Dictionary<string, string> basket4;
    private Dictionary<string, string> numberJumble4;
    private Dictionary<string, string> faultyRobot4;

    private Dictionary<string, string> basket5;
    private Dictionary<string, string> numberJumble5;
    private Dictionary<string, string> faultyRobot5;

    private Dictionary<string, string> basket6;
    private Dictionary<string, string> numberJumble6;
    private Dictionary<string, string> faultyRobot6;
    public Dictionary<string, Dictionary<string, string>> gameSettings;

    public GameSettings()
    {
        gameSettings = new Dictionary<string, Dictionary<string, string>>();
        basket1 = new Dictionary<string, string>();
        numberJumble1 = new Dictionary<string, string>();
        faultyRobot1 = new Dictionary<string, string>();

        basket2 = new Dictionary<string, string>();
        numberJumble2 = new Dictionary<string, string>();
        faultyRobot2 = new Dictionary<string, string>();

        basket3 = new Dictionary<string, string>();
        numberJumble3 = new Dictionary<string, string>();
        faultyRobot3 = new Dictionary<string, string>();

        basket4 = new Dictionary<string, string>();
        numberJumble4 = new Dictionary<string, string>();
        faultyRobot4 = new Dictionary<string, string>();

        basket5 = new Dictionary<string, string>();
        numberJumble5 = new Dictionary<string, string>();
        faultyRobot5 = new Dictionary<string, string>();

        basket6 = new Dictionary<string, string>();
        numberJumble6 = new Dictionary<string, string>();
        faultyRobot6 = new Dictionary<string, string>();


        basket1.Add("MovementSpeed","5");
        basket1.Add("SpawnRange", "10");
        basket1.Add("WinCondition", "Even");
        basket1.Add("WinConditionNumber", "5");
        basket1.Add("HighscoreMultiplier", "1");
        basket1.Add("SpawnRate", "1");

        faultyRobot1.Add("Button1X", "2");
        faultyRobot1.Add("Button1Y", "1");
        faultyRobot1.Add("Button2X", "3");
        faultyRobot1.Add("Button2Y", "-1");
        faultyRobot1.Add("Button3X", "-1");
        faultyRobot1.Add("Button3Y", "0");
        faultyRobot1.Add("TargetX", "1.5");
        faultyRobot1.Add("TargetY", "0.5");
        faultyRobot1.Add("HighscoreMultiplier", "1");

        numberJumble1.Add("Button1", "5");
        numberJumble1.Add("Button2", "7");
        numberJumble1.Add("Button3", "-10");
        numberJumble1.Add("TargetNumber", "42");
        numberJumble1.Add("HighscoreMultiplier", "1");


        basket2.Add("MovementSpeed", "4.5");
        basket2.Add("SpawnRange", "10");
        basket2.Add("WinCondition", "Greater than");
        basket2.Add("WinConditionNumber", "6");
        basket2.Add("HighscoreMultiplier", "1");
        basket2.Add("SpawnRate", "0.95");

        faultyRobot2.Add("Button1X", "3");
        faultyRobot2.Add("Button1Y", "3");
        faultyRobot2.Add("Button2X", "-1");
        faultyRobot2.Add("Button2Y", "-1");
        faultyRobot2.Add("Button3X", "0");
        faultyRobot2.Add("Button3Y", "-1");
        faultyRobot2.Add("TargetX", "3.5");
        faultyRobot2.Add("TargetY", "3.5");
        faultyRobot2.Add("HighscoreMultiplier", "1");

        numberJumble2.Add("Button1", "2");
        numberJumble2.Add("Button2", "-5");
        numberJumble2.Add("Button3", "9");
        numberJumble2.Add("TargetNumber", "20");
        numberJumble2.Add("HighscoreMultiplier", "1");


        basket3.Add("MovementSpeed", "4");
        basket3.Add("SpawnRange", "10");
        basket3.Add("WinCondition", "Less than");
        basket3.Add("WinConditionNumber", "7");
        basket3.Add("HighscoreMultiplier", "1");
        basket3.Add("SpawnRate", "0.8");

        faultyRobot3.Add("Button1X", "1");
        faultyRobot3.Add("Button1Y", "2");
        faultyRobot3.Add("Button2X", "-1");
        faultyRobot3.Add("Button2Y", "-3");
        faultyRobot3.Add("Button3X", "2");
        faultyRobot3.Add("Button3Y", "-4");
        faultyRobot3.Add("TargetX", "3.5");
        faultyRobot3.Add("TargetY", "3.5");
        faultyRobot3.Add("HighscoreMultiplier", "2");

        numberJumble3.Add("Button1", "20");
        numberJumble3.Add("Button2", "-17");
        numberJumble3.Add("Button3", "19");
        numberJumble3.Add("TargetNumber", "100");
        numberJumble3.Add("HighscoreMultiplier", "3");



        basket4.Add("MovementSpeed", "7");
        basket4.Add("SpawnRange", "21");
        basket4.Add("WinCondition", "Odd");
        basket4.Add("WinConditionNumber", "10");
        basket4.Add("HighscoreMultiplier", "3");
        basket4.Add("SpawnRate", "1");

        faultyRobot4.Add("Button1X", "2");
        faultyRobot4.Add("Button1Y", "1");
        faultyRobot4.Add("Button2X", "3");
        faultyRobot4.Add("Button2Y", "-1");
        faultyRobot4.Add("Button3X", "-1");
        faultyRobot4.Add("Button3Y", "0");
        faultyRobot4.Add("TargetX", "1.5");
        faultyRobot4.Add("TargetY", "0.5");
        faultyRobot4.Add("HighscoreMultiplier", "1");

        numberJumble4.Add("Button1", "5");
        numberJumble4.Add("Button2", "7");
        numberJumble4.Add("Button3", "-10");
        numberJumble4.Add("TargetNumber", "42");
        numberJumble4.Add("HighscoreMultiplier", "1");


        basket5.Add("MovementSpeed", "7");
        basket5.Add("SpawnRange", "21");
        basket5.Add("WinCondition", "Odd");
        basket5.Add("WinConditionNumber", "10");
        basket5.Add("HighscoreMultiplier", "3");
        basket5.Add("SpawnRate", "1");

        faultyRobot5.Add("Button1X", "2");
        faultyRobot5.Add("Button1Y", "1");
        faultyRobot5.Add("Button2X", "3");
        faultyRobot5.Add("Button2Y", "-1");
        faultyRobot5.Add("Button3X", "-1");
        faultyRobot5.Add("Button3Y", "0");
        faultyRobot5.Add("TargetX", "1.5");
        faultyRobot5.Add("TargetY", "0.5");
        faultyRobot5.Add("HighscoreMultiplier", "1");

        numberJumble5.Add("Button1", "5");
        numberJumble5.Add("Button2", "7");
        numberJumble5.Add("Button3", "-10");
        numberJumble5.Add("TargetNumber", "42");
        numberJumble5.Add("HighscoreMultiplier", "1");


        basket6.Add("MovementSpeed", "7");
        basket6.Add("SpawnRange", "21");
        basket6.Add("WinCondition", "Odd");
        basket6.Add("WinConditionNumber", "10");
        basket6.Add("HighscoreMultiplier", "3");
        basket6.Add("SpawnRate", "1");

        faultyRobot6.Add("Button1X", "2");
        faultyRobot6.Add("Button1Y", "1");
        faultyRobot6.Add("Button2X", "3");
        faultyRobot6.Add("Button2Y", "-1");
        faultyRobot6.Add("Button3X", "-1");
        faultyRobot6.Add("Button3Y", "0");
        faultyRobot6.Add("TargetX", "1.5");
        faultyRobot6.Add("TargetY", "0.5");
        faultyRobot6.Add("HighscoreMultiplier", "1");

        numberJumble6.Add("Button1", "5");
        numberJumble6.Add("Button2", "7");
        numberJumble6.Add("Button3", "-10");
        numberJumble6.Add("TargetNumber", "42");
        numberJumble6.Add("HighscoreMultiplier", "1");

        gameSettings.Add("Basket1", basket1);
        gameSettings.Add("NumberJumble1", numberJumble1);
        gameSettings.Add("FaultyRobot1", faultyRobot1);

        gameSettings.Add("Basket2", basket2);
        gameSettings.Add("NumberJumble2", numberJumble2);
        gameSettings.Add("FaultyRobot2", faultyRobot2);

        gameSettings.Add("Basket3", basket3);
        gameSettings.Add("NumberJumble3", numberJumble3);
        gameSettings.Add("FaultyRobot3", faultyRobot3);

        gameSettings.Add("Basket4", basket4);
        gameSettings.Add("NumberJumble4", numberJumble4);
        gameSettings.Add("FaultyRobot4", faultyRobot4);

        gameSettings.Add("Basket5", basket5);
        gameSettings.Add("NumberJumble5", numberJumble5);
        gameSettings.Add("FaultyRobot5", faultyRobot5);

        gameSettings.Add("Basket6", basket6);
        gameSettings.Add("NumberJumble6", numberJumble6);
        gameSettings.Add("FaultyRobot6", faultyRobot6);


    }
}
