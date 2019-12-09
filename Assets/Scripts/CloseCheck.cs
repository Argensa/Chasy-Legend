using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCheck : MonoBehaviour
{
    public GameObject playerSpawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16)
        {
            playerSpawner.GetComponent<PlayerSpawner>().tooClose = true;
        }
    }
}
