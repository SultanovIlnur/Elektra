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

    public void ImportItemStats(int importID, int importCount = 1)
    {
        currentItemID = importID;
        currentItemCount = Mathf.Max(1, importCount);

        if (ItemResources.TryGetItemDefinition(currentItemID, out Item itemDefinition))
        {
            currentItemName = itemDefinition.ItemName;
            currentItemType = itemDefinition.ItemType;
        }
        else
        {
            currentItemName = "";
            currentItemType = 0;
        }

    }
    void PickupItem()
    {

    }
}
