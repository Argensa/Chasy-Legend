using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelGrounded : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = true;
           
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = false;
        }
    }
}
