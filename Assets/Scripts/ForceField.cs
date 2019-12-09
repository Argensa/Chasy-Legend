using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    Vector3 targetScale;
    Vector3 orgScale;
    public float speed;
    float timeCount;
    float liveTime;
    public bool active = false;
    public GameObject playerHolder;
    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        orgScale = transform.localScale;
        targetScale = orgScale * 1.2f;
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        mesh = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
           {
            mesh.enabled = true;
            Vector3 updatePos = new Vector3(playerHolder.transform.position.x, playerHolder.transform.position.y, playerHolder.transform.position.z);
            transform.position = updatePos;
            liveTime += Time.deltaTime;
                if (liveTime >= 1200)
                {
                      active = false;
                }

                timeCount += Time.deltaTime;
                if (timeCount <= .5f)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);
                }
                else if (timeCount >= 1f && timeCount <= 1.5f)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, orgScale, speed * Time.deltaTime);
                }
                else if (timeCount > 1.5f)
                {
                    timeCount = 0f;
                }
            }
        else if (active == false)
        {
            mesh.enabled = false;
            transform.position = new Vector3(0, -20, 0);
            liveTime = 0f;
        }
       
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<EnemyTargetScript>().health = 0f;
        } 
      
    }
}
