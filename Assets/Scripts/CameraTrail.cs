using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrail : MonoBehaviour
{
    Vector3 offset;
    public GameObject playerHolder;
    public Camera mainCamera;
    public GameObject gameController;
    RaycastHit hit;

    bool doAlready = false;
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
            if (doAlready == false)
            {
                playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
                offset = playerHolder.transform.position - transform.position;
                doAlready = true;

            }
            transform.position = playerHolder.transform.position - offset;
        }
      
       // Transparent();
    }
    void Transparent() //Does not work :(
    {

        //  Ray ray = mainCamera.ScreenPointToRay(playerHolder.transform.position);

         
        
        if (Physics.Raycast(playerHolder.transform.position, transform.position, out hit)) //throw out a raycast from the camera
        {
            Transform objectHit = hit.transform;


            if (objectHit.gameObject.layer == 16) //if that ray hits the ground then
            {
               
                if (objectHit.GetComponent<MeshRenderer>() != null)
                {
                    Debug.Log("hit");
                    Color color = objectHit.GetComponent<MeshRenderer>().material.color;
                    color.a = 0.5f;
                    objectHit.GetComponent<MeshRenderer>().material.color = color;
                }
                else if (objectHit.GetComponent<MeshRenderer>() == null)
                {
                    if (objectHit.GetComponentInChildren<MeshRenderer>() != null)
                    {
                        Color colorChild = objectHit.GetComponentInChildren<MeshRenderer>().material.color;
                        colorChild.a = 0.5f;
                        objectHit.GetComponentInChildren<MeshRenderer>().material.color = colorChild;
                    }
                }

            }
        }

    }
}
