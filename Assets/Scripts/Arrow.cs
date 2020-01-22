using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Arrow : MonoBehaviour
{

    public GameObject money;
    public GameObject playerHolder;
    Vector3 direction;
    GameObject distText;
    float distMeter;
    public TextMeshProUGUI distText_Text;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        money = GameObject.FindGameObjectWithTag("Dollar");
        distText = GameObject.FindGameObjectWithTag("DistanceText");
        distText_Text = distText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerHolder.transform.position;
        //direction = new Vector3(money.transform.position.x - transform.position.x, transform.position.y, money.transform.position.z - transform.position.z);
        transform.LookAt(money.transform);
        distMeter = Mathf.Round(Vector3.Distance(money.transform.position, playerHolder.transform.position));
        distText_Text.text = distMeter.ToString() + "m";
    }

}
