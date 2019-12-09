using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnStart : MonoBehaviour
{
    public GameObject gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetComponent<SceneController>().gameState == 1)
        {
            gameObject.SetActive(false);
        }
    }
}
