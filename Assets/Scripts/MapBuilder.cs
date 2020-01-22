using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    public int maxSize;
    public GameObject[] tile;
    public GameObject map;
    int random;
    public GameObject chosenTile;
    public GameObject safeZone;
    int randomSafeZone;
    int zoneCheck;
    int randomRot;
    Quaternion randomRotation;


    float a;


    public GameObject playerHolder;
    public bool spawnTile = false;
    public Transform onTile;
    public Transform oldTile;


    [SerializeField]
    float collideCount;
    float checkTime;
    float frequentCheckTime;

    // Start is called before the first frame update
    void Awake()

    {

        map = GameObject.FindGameObjectWithTag("Map");
      /*  for (int b = 0; b <= 17; b++)
        {
            Physics.IgnoreLayerCollision(18, b);
        }
        
    */
        transform.position = new Vector3(0, -1, 0);
             for (int i = 0; i <= maxSize - 1; i++)
             {
                 for (int j = 0; j <= maxSize - 1; j++)
                 {
                /* random = Random.Range(0, tile.Length);
                 zoneCheck = randomSafeZone;
                 randomRot = Random.Range(0, 3);
                 randomSafeZone = Random.Range(0, 8);

                 chosenTile = tile[random];

                 randomRotation.eulerAngles = new Vector3(0, randomRot * 90, 0);
                 transform.position = new Vector3(-130 + i * 100, -1, -130 + j * 100);
                 GameObject currentTile = SCR_Pool.GetFreeObject(chosenTile);
                 currentTile.transform.parent = map.transform;
                 currentTile.GetComponent<SpawnScript>().Spawn(transform.position, randomRotation);
                 if (randomSafeZone == 1 && zoneCheck != 1 && Vector3.Distance(transform.position, playerHolder.transform.position) > 150f)
                 {
                     GameObject safeZoneObj = SCR_Pool.GetFreeObject(safeZone);
                     safeZoneObj.transform.parent = map.transform;
                     safeZoneObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                 } */

                    transform.position = new Vector3(-100 + i * 100, -1, -100 + j * 100);
                    RayCastGroundCheck();
                    if (collideCount == 0)
                    {
                            SpawnTile(.02f);
                    }

                }

             }
         
       
    }
 
    // Update is called once per frame
    void LateUpdate()
    {

        CheckTile();

    }
    void CheckTile()
    {

        if (onTile != null)  //&& onTile != oldTile
        {
            a += Time.deltaTime;


            if (a >= 0 && a <= .1f)
            {
                
                transform.position = new Vector3(onTile.position.x + 100, onTile.position.y, onTile.position.z + 100);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
               
            }
            else if (a > .1f && a <= .2f)
            {

                transform.position = new Vector3(onTile.position.x + 100, onTile.position.y, onTile.position.z);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
            }
            else if (a > .2f && a <= .3f)
            {

                transform.position = new Vector3(onTile.position.x + 100, onTile.position.y, onTile.position.z - 100);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
            }
            else if (a > .3f && a <= .4f)
            {

                transform.position = new Vector3(onTile.position.x, onTile.position.y, onTile.position.z - 100);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
            }
            else if (a > .4f && a <= .5f)
            {
                transform.position = new Vector3(onTile.position.x - 100, onTile.position.y, onTile.position.z - 100);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
            }
            else if (a > .5f && a <= .6f)
            {
                transform.position = new Vector3(onTile.position.x - 100, onTile.position.y, onTile.position.z);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
            }
            else if (a > .6f && a <= .7f)
            {
                transform.position = new Vector3(onTile.position.x - 100, onTile.position.y, onTile.position.z + 100);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
            }
            else if (a > .7f && a <= .8f)
            {
                transform.position = new Vector3(onTile.position.x, onTile.position.y, onTile.position.z + 100);
                RayCastGroundCheck();
                if (collideCount == 0)
                {
                    SpawnTile(.05f);
                }
            }
            else if (a > .8f)
            {
                oldTile = onTile;
                a = 0f;
               
            }
        }
    }
   
    void SpawnTile(float interval)
    {
        if (collideCount == 0)
        {
            checkTime += Time.deltaTime;
        }
        if (checkTime >= interval && collideCount == 0)
        {

            randomRot = Random.Range(0, 3);
            randomSafeZone = Random.Range(0, 8);
            randomRotation.eulerAngles = new Vector3(0, randomRot * 90, 0);
            random = Random.Range(0, tile.Length);
            chosenTile = tile[random];

            GameObject currentTile = SCR_Pool.GetFreeObject(chosenTile);
            currentTile.transform.parent = map.transform;
            currentTile.GetComponent<SpawnScript>().Spawn(transform.position, randomRotation);
            if (randomSafeZone == 1 && zoneCheck != 1)
            {
                GameObject safeZoneObj = SCR_Pool.GetFreeObject(safeZone);
                safeZoneObj.transform.parent = map.transform;
                safeZoneObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
            }
            checkTime = 0;
        }

    }
   
    void RayCastGroundCheck()
    {
        RaycastHit hit2;
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y -20, transform.position.z);
        Ray groundRay = new Ray(newPos, transform.up * 20);
        if (Physics.Raycast(groundRay, out hit2, 80))
           
        {
            Transform objectHit = hit2.transform;

            if (objectHit != null)
            {

                if (objectHit.gameObject.layer == 9 || objectHit.gameObject.CompareTag("Ground"))
                {
                    collideCount = 0f;
                }

                if (objectHit.gameObject.layer == 19 || objectHit.CompareTag("Tile") || objectHit.gameObject.CompareTag("Safe Zone"))
                {
                    collideCount = 1;

                }
               

            } else if (objectHit == null)
            {
                collideCount = 0f;
            }
           
        }
    }
}
