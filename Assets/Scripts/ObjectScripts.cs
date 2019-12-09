using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScripts : MonoBehaviour
{
    Rigidbody rb;
    private float fallCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.x > 55 || transform.eulerAngles.x < -55
         || transform.eulerAngles.z > 55 || transform.eulerAngles.z < -55)
        {
            fallCount = fallCount + Time.deltaTime;
            if (fallCount > 5f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
