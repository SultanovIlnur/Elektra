using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    private int[] ItemIDArray;
    [SerializeField]
    private int[] ItemCountNeedArray;
    [SerializeField]
    private int[] ItemCountStoredArray;
    
    private int actionType = 0;

    private int itemsCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        itemsCount = ItemIDArray.Length;
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
    bool CheckIsFilled(int itemSlotNumber)
    {
        if (itemSlotNumber < ItemCountStoredArray.Length)
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
