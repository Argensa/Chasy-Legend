using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChangeItem : MonoBehaviour
{
    int itemID;
    public GameObject[] itemList;
    GameObject currentItem;
    private void Start()
    {
        currentItem = itemList[0];
        itemID = 0;
    }
    public void ShopChangeRight()
    {
        currentItem.SetActive(false);
        itemID++;
        if (itemID > itemList.Length - 1)
        {
            itemID = 0;
        }
        itemList[itemID].SetActive(true);
        currentItem = itemList[itemID];
    }
    public void ShopChangeLeft()
    {
        currentItem.SetActive(false);
        itemID--;
        if (itemID < 0)
        {
            itemID = itemList.Length - 1;
        }
        itemList[itemID].SetActive(true);
        currentItem = itemList[itemID];
    }
}
