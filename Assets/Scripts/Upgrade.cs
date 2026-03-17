using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    private int[] ItemIDArray = new int[0];
    [SerializeField]
    private int[] ItemCountNeedArray = new int[0];
    [SerializeField]
    private int[] ItemCountStoredArray = new int[0];
    
    private int actionType = 0;

    private int itemsCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        EnsureUpgradeArrays();
        itemsCount = Mathf.Min(ItemIDArray.Length, ItemCountNeedArray.Length);
        itemsCount = Mathf.Min(itemsCount, ItemCountStoredArray.Length);
    }

    // Update is called once per frame
    void Update()
    {
        /*for (int i = 0; i < ItemIDArray.Length; i++)
        {
            if (CheckIsFilled(i))
            {

            }
            else
            {

            }
        }*/
    }

    void EnsureUpgradeArrays()
    {
        if (ItemIDArray == null)
        {
            ItemIDArray = new int[0];
        }

        if (ItemCountNeedArray == null)
        {
            ItemCountNeedArray = new int[0];
        }

        if (ItemCountStoredArray == null)
        {
            ItemCountStoredArray = new int[0];
        }
    }

    bool CheckIsFilled(int itemSlotNumber)
    {
        EnsureUpgradeArrays();

        if (itemSlotNumber >= 0 && itemSlotNumber < itemsCount)
        {
            if (ItemCountStoredArray[itemSlotNumber] == ItemCountNeedArray[itemSlotNumber])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }
}
