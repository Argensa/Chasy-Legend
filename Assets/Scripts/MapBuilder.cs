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

    [SerializeField]
    float collideCount;
    float checkTime;
    // Start is called before the first frame update
    void Awake()
        
    {
        map = GameObject.FindGameObjectWithTag("Map");
        for (int b = 0; b <= 17; b++)
        {
            Physics.IgnoreLayerCollision(18, b);
        }

        
        transform.position = new Vector3(0, -1, 0);
        for (int i = 0; i <= maxSize - 1; i++)
        {
            for (int j = 0; j <= maxSize - 1; j++)
            {
                random = Random.Range(0, tile.Length);
                zoneCheck = randomSafeZone;
                randomRot = Random.Range(0, 3);
                randomSafeZone = Random.Range(0,8);

                chosenTile = tile[random];
                
                randomRotation.eulerAngles = new Vector3(0, randomRot * 90, 0);
                transform.position = new Vector3(-130 + i * 80, -1, -130 + j * 80);
                GameObject currentTile = SCR_Pool.GetFreeObject(chosenTile);
                currentTile.transform.parent = map.transform;
                currentTile.GetComponent<SpawnScript>().Spawn(transform.position, randomRotation);
                if (randomSafeZone == 1 && zoneCheck != 1)
                {
                    GameObject safeZoneObj = SCR_Pool.GetFreeObject(safeZone);
                    safeZoneObj.transform.parent = map.transform;
                    safeZoneObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                }
            }

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (onTile != null )
        {
            a += Time.deltaTime;


            if (a >= 0 && a <= .1f)
            {

                transform.position = new Vector3(onTile.position.x + 80, onTile.position.y, onTile.position.z + 80);
                SpawnTile();
            }
            else if (a > .1f && a <= .2f)
            {
                transform.position = new Vector3(onTile.position.x + 80, onTile.position.y, onTile.position.z);

                SpawnTile();
            }
            else if (a > .2f && a <= .3f)
            {
                transform.position = new Vector3(onTile.position.x + 80, onTile.position.y, onTile.position.z - 80);
                SpawnTile();
            }
            else if (a > .3f && a <= .4f)
            {
                transform.position = new Vector3(onTile.position.x, onTile.position.y, onTile.position.z - 80);
                SpawnTile();
            }
            else if (a > .4f && a <= .5f)
            {
                transform.position = new Vector3(onTile.position.x - 80, onTile.position.y, onTile.position.z - 80);

                SpawnTile();
            }
            else if (a > .5f && a <= .6f)
            {
                transform.position = new Vector3(onTile.position.x - 80, onTile.position.y, onTile.position.z);
                SpawnTile();
            }
            else if (a > .6f && a <= .7f)
            {
                transform.position = new Vector3(onTile.position.x - 80, onTile.position.y, onTile.position.z + 80);
                SpawnTile();
            }
            else if (a > .7f && a <= .8f)
            {
                
                transform.position = new Vector3(onTile.position.x, onTile.position.y, onTile.position.z + 80);
                SpawnTile();
            }
            else if (a > .8f)
            {
                a = 0f;
            }
        }
        
       

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tile"))
        {
            checkTime = 0f;
            collideCount = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tile"))
        {
            checkTime = 0f;
            collideCount = 1;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tile"))
        {
            checkTime = 0f;
            collideCount = 1;
        }
    }
    void SpawnTile()
    {

        if (collideCount < 1)
        {
            checkTime += Time.deltaTime;
        } 
        if (checkTime >= .005f)
        {
            randomRot = Random.Range(0, 3);
            randomSafeZone = Random.Range(0, 8);
            randomRotation.eulerAngles = new Vector3(0, randomRot * 90, 0);
            random = Random.Range(0, tile.Length);
            chosenTile = tile[random];
           // GameObject currentTile = SCR_Pool.GetFreeObject(chosenTile);
            Instantiate(chosenTile, transform.position, randomRotation, map.transform);
            //currentTile.transform.parent = map.transform;
            // randomRotation.eulerAngles = new Vector3(0, randomRot * 90, 0);
            // currentTile.GetComponent<SpawnScript>().Spawn(transform.position, randomRotation);
            if (randomSafeZone == 1 && zoneCheck != 1)
            {
                GameObject safeZoneObj = SCR_Pool.GetFreeObject(safeZone);
                safeZoneObj.transform.parent = map.transform;
                safeZoneObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
            }
            checkTime = 0f;
        }
        
    }

}
