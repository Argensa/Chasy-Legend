using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerSignScript : MonoBehaviour
{
    GameObject playerHolder;
    GameObject[] APC;
    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        mesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        APC = GameObject.FindGameObjectsWithTag("APC");
        foreach (GameObject enemy in APC)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<Enemy2AI>().playerDetected != false)
                {

                    if (transform.GetChild(0).GetComponent<MeshRenderer>() != null)
                    {

                        mesh.enabled = true;
                    }
                }
                if (enemy.GetComponent<Enemy2AI>().playerDetected == false)
                {
                    if (transform.GetChild(0).GetComponent<MeshRenderer>() != null)
                    {

                        mesh.enabled = false;
                    }
                }
            }
            else if (enemy == null)
            {
                if (transform.GetChild(0).GetComponent<MeshRenderer>() != null)
                {

                    mesh.enabled = false;
                }
            }
        }
       
        transform.position = new Vector3(playerHolder.transform.position.x + 3f, playerHolder.transform.position.y + 0.45f, playerHolder.transform.position.z + 3f);
    }
}
