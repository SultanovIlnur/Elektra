using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //item name to be displayed on ui
    [SerializeField]
    private string itemName = "";
    public string ItemName
    {
        get
        {
            return itemName;
        }
        set
        {
            itemName = value;
        }

    }
    //item ID to spawn this item
    [SerializeField]
    private int itemID = 0;
    public int ItemID
    {
        get
        {
            return itemID;
        }
        set
        {
            itemID = value;
        }
    }
    //preview sprite for use inventory and ui
    [SerializeField]
    private Sprite preview;
    public Sprite Preview
    {
        get
        {
            return preview;
        }
        set
        {
            preview = value;
        }
    }
    //cube types
    //0 block - just a square block with a material on it and can be placed (eg. wall and floor objects)
    //1 instrument - it'a unplaceable Item (eg. pistol and pickaxe)
    //2 resource - just a resource that can't be
    [SerializeField]
    private int itemType = 0;
    public int ItemType
    {
        get
        {
            return itemType;
        }
        set
        {
            itemType = value;
        }
    }
    [SerializeField]
    private int maxStack = 1;
    public int MaxStack
    {
        get
        {
            return maxStack;
        }
        set
        {
            maxStack = value;
        }
    }
    [SerializeField]
    private GameObject model;
    public GameObject Model
    {
        get
        {
            return model;
        }
        set
        {
            model = value;
        }
    }
    //can object be destroyed
    [SerializeField]
    private bool destroyable = true;
    public bool Destroyable
    {
        get
        {
            return destroyable;
        }
        set
        {
            destroyable = value;
        }
    }
    [SerializeField]
    //can object burn in fire and take damage from it
    private bool fireable = false;
    public bool Fireable
    {
        get
        {
            return fireable;
        }
        set
        {
            fireable = value;
        }
    }
    //health of this object
    [SerializeField]
    private int hp = 100;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }
    //the damage that item can harm to the player or other items
    [SerializeField]
    private int damage = 1;
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }
    [SerializeField]
    private bool screwdriver = false;
    [SerializeField]
    private bool crowbar = false;
    [SerializeField]
    private bool welder = false;
    //using special tools!
    [SerializeField]
    private bool upgradeable = false;
    //degradable means it can be dissasembled faster by using special instruments
    [SerializeField]
    private bool degradable = false;


    void Start()
    {
    }


    void Update()
    {
        if (Hp <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
