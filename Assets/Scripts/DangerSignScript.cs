using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class DangerSignScript : MonoBehaviour
{
    GameObject playerHolder;
    GameObject[] APC;
    MeshRenderer mesh;

    public Vignette vignette;
    bool danger;
    public GameObject mainCam;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        mesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        mesh.enabled = false;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        mainCam.GetComponent<PostProcessVolume>().profile.TryGetSettings<Vignette>(out vignette);
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
                        vignette.color.value = Color.red;
                    }
                }
                if (enemy.GetComponent<Enemy2AI>().playerDetected == false)
                {
                    if (transform.GetChild(0).GetComponent<MeshRenderer>() != null)
                    {

                        mesh.enabled = false;
                        vignette.color.value = Color.black;
                    }
                }
            }
            else if (enemy == null)
            {
                if (transform.GetChild(0).GetComponent<MeshRenderer>() != null)
                {

                    mesh.enabled = false;
                    vignette.color.value = Color.black;
                }
            }
        }
       
        transform.position = new Vector3(playerHolder.transform.position.x + 3f, playerHolder.transform.position.y + 0.45f, playerHolder.transform.position.z + 3f);
    }
}
