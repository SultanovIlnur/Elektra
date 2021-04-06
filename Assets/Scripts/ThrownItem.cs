using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownItem : MonoBehaviour
{
    public int currentItemID = 0;
    public string currentItemName = "";
    public int currentItemType = 0;
    public int currentItemCount = 1;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ImportItemStats(int importID)
    {
        currentItemID = importID;
        Debug.Log($"Current item ID {currentItemID}");
        if (Resources.Load<GameObject>($"Items/{currentItemID}") != null)
        {
            GameObject importItem = Resources.Load<GameObject>($"Items/{currentItemID}");
            string importItemName = importItem.GetComponent<Item>().ItemName;
            int importItemType = importItem.GetComponent<Item>().ItemType;

            currentItemName = importItemName;
            currentItemType = importItemType;
        }

    }
    void PickupItem()
    {

    }
}
