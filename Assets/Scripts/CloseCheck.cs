using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCheck : MonoBehaviour
{
    public GameObject playerSpawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16)
        {
            playerSpawner.GetComponent<PlayerSpawner>().tooClose = true;
        }
    }
}
