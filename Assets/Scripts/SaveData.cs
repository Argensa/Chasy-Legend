using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData
{

    public int killCount = 0;
    public int money = 0;
    public float inGameTime;


    public SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>();
    }

    void OnGameEnd()
    {
        killCount = sceneController.killCount;
        money = sceneController.money;
        inGameTime = sceneController.inGameTime;
    }
}
