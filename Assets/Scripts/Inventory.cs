
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    Color selectedColor = Color.white;
    Color unselectedColor = Color.gray;
    private int activePanel;
    public int ActivePanel
    {
        get
        {
            return activePanel;
        }
        set
        {
            activePanel = value;
        }
    }
    bool isPanelEmpty;
    GameObject[] panels = new GameObject[9];
    Image[] panelsColor = new Image[9];
    Image[] panelImages = new Image[9];
    Text itemNameText;
    Text itemCountText;
    Text pickupText;
    GameObject pickupTextPanel;
    [SerializeField]
    GameObject fpsCamera;
    public InventoryItem[] inventoryItemArray = new InventoryItem[9];


    void Start()
    {
        itemNameText = GameObject.Find("GuiCanvas/InfoPanel/ItemNameText").GetComponent<Text>();
        itemCountText = GameObject.Find("GuiCanvas/InfoPanel/ItemCountText").GetComponent<Text>();
        pickupText = GameObject.Find("GuiCanvas/PickupText").GetComponent<Text>();
        pickupTextPanel = GameObject.Find("GuiCanvas/PickupTextPanel");
        FillPanels();

        TryAddItem(100, 1, 4);
        TryAddItem(3, 128, 5);
        TryAddItem(4, 128, 7);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPanelButtonsDown();
        SetGUICurrentInfo();
    }

    void FillPanels()
    {
        for (int i = 0; i < 9; i++)
        {
        }

        activePanel = 0;
        for (int i = 0; i < 9; i++)
        {
            panels[i] = GameObject.Find($"GuiCanvas/InvPanel/{i + 1}");
            panelsColor[i] = panels[i].GetComponent<Image>();
            panelImages[i] = GameObject.Find($"GuiCanvas/InvPanel/{i + 1}/Image").GetComponent<Image>();
        }

        SetActivePanel(0);
    }

    void SetActivePanel(int newActivePanel)
    {
        activePanel = newActivePanel;

        for (int i = 0; i < panelsColor.Length; i++)
        {
            panelsColor[i].color = i == activePanel ? selectedColor : unselectedColor;
        }
    }

    void CheckForPanelButtonsDown()
    {


        //отслеживание нажатий клавиатуры изменения активной панели инвентаря
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown($"{i + 1}"))
            {
                SetActivePanel(i);
                break;
            }
        }

        //отслеживание поворота колесика мыши для изменения активной панели инвентаря
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            int newActivePanel = activePanel - 1;
            if (newActivePanel < 0)
            {
                newActivePanel = 8;
            }
            SetActivePanel(newActivePanel);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            int newActivePanel = activePanel + 1;
            if (newActivePanel > 8)
            {
                newActivePanel = 0;
            }
            SetActivePanel(newActivePanel);
        }

        //кнопка для выкидывания объекта из руки
        if (Input.GetKeyDown("q"))
        {
            if (inventoryItemArray[activePanel] != null)
            {
                if (ThrowOnGround(inventoryItemArray[activePanel].ItemID, 1))
                {
                    DeleteItem(activePanel, 1);
                }

            }

        }

    }

    void CheckForItems()
    {
        for (int i = 0; i < 9; i++)
        {
            if (inventoryItemArray[i]?.ItemCount <= 0)
            {
                inventoryItemArray[i] = null;
            }
            else
            {

            }
        }
    }

    int GetItemMaxStack(int itemID)
    {
        GameObject itemPrefab = Resources.Load<GameObject>($"Items/{itemID}");
        if (itemPrefab != null && itemPrefab.GetComponent<Item>() != null)
        {
            return Mathf.Max(1, itemPrefab.GetComponent<Item>().MaxStack);
        }

        return 1;
    }

    public int AddItem(int itemID, int panelNumber, int itemCount)
    {
        if (itemCount <= 0)
        {
            return 0;
        }

        int maxStack = GetItemMaxStack(itemID);
        if (inventoryItemArray[panelNumber] != null)
        {
            if (inventoryItemArray[panelNumber].ItemID != itemID)
            {
                return itemCount;
            }

            int freeSpace = maxStack - inventoryItemArray[panelNumber].ItemCount;
            if (freeSpace <= 0)
            {
                return itemCount;
            }

            int addedCount = Mathf.Min(itemCount, freeSpace);
            inventoryItemArray[panelNumber].ItemCount += addedCount;
            return itemCount - addedCount;
        }

        inventoryItemArray[panelNumber] = (InventoryItem)ScriptableObject.CreateInstance("InventoryItem");
        inventoryItemArray[panelNumber].ItemID = itemID;

        int newStackCount = Mathf.Min(itemCount, maxStack);
        inventoryItemArray[panelNumber].ItemCount = newStackCount;
        SetImageOnPanel(panelNumber, itemID);

        return itemCount - newStackCount;
    }

    public bool TryAddItem(int itemID, int itemCount)
    {
        return TryAddItem(itemID, itemCount, activePanel);
    }

    public bool TryAddItem(int itemID, int itemCount, int preferredPanel)
    {
        if (itemCount <= 0)
        {
            return true;
        }

        if (preferredPanel < 0 || preferredPanel >= inventoryItemArray.Length)
        {
            preferredPanel = activePanel;
        }

        int remainingCount = AddItem(itemID, preferredPanel, itemCount);
        if (remainingCount <= 0)
        {
            return true;
        }

        for (int i = 1; i < inventoryItemArray.Length; i++)
        {
            int panelIndex = (preferredPanel + i) % inventoryItemArray.Length;
            if (inventoryItemArray[panelIndex] != null && inventoryItemArray[panelIndex].ItemID == itemID)
            {
                remainingCount = AddItem(itemID, panelIndex, remainingCount);
                if (remainingCount <= 0)
                {
                    return true;
                }
            }
        }

        for (int i = 1; i < inventoryItemArray.Length; i++)
        {
            int panelIndex = (preferredPanel + i) % inventoryItemArray.Length;
            if (inventoryItemArray[panelIndex] == null)
            {
                remainingCount = AddItem(itemID, panelIndex, remainingCount);
                if (remainingCount <= 0)
                {
                    return true;
                }
            }
        }

        return remainingCount <= 0;
    }

    public void DeleteItem(int panelNumber, int itemCount)
    {
        if (inventoryItemArray[panelNumber] != null)
        {
            if ((inventoryItemArray[panelNumber].ItemCount - itemCount) >= 1)
            {
                inventoryItemArray[panelNumber].ItemCount -= itemCount;
            }
            else if ((inventoryItemArray[panelNumber].ItemCount - itemCount) < 1)
            {
                inventoryItemArray[panelNumber] = null;
                ClearImageOnPanel(panelNumber);
            }
        }

    }
    //выкинуть объект на землю
    bool ThrowOnGround(int currentID, int currentCount)
    {
        Vector3 fpsCameraPosition = fpsCamera.transform.position;
        GameObject thrownItemPrefab = Resources.Load<GameObject>("ThrownItem");
        GameObject itemPrefab = Resources.Load<GameObject>($"Items/{currentID}");
        if (thrownItemPrefab == null || itemPrefab == null)
        {
            return false;
        }

        Item itemScript = itemPrefab.GetComponent<Item>();
        if (itemScript == null || itemScript.Model == null)
        {
            return false;
        }

        GameObject thrownItemInstanced = Instantiate<GameObject>(thrownItemPrefab, fpsCameraPosition + transform.forward * 0.3f, Quaternion.identity);
        ThrownItem thrownItemScript = thrownItemInstanced.GetComponent<ThrownItem>();
        if (thrownItemScript == null)
        {
            Destroy(thrownItemInstanced);
            return false;
        }

        thrownItemScript.ImportItemStats(currentID, currentCount);

        GameObject modelInstanced = Instantiate<GameObject>(itemScript.Model, thrownItemInstanced.transform);
        modelInstanced.transform.localPosition = Vector3.zero;
        modelInstanced.transform.localRotation = Quaternion.identity;
        modelInstanced.transform.localScale *= 0.3f;
        return true;
    }

    void SetGUICurrentInfo()
    {
        if (inventoryItemArray[ActivePanel] != null)
        {
            itemNameText.text = Resources.Load<GameObject>($"Items/{inventoryItemArray[activePanel].ItemID}").GetComponent<Item>().ItemName;
            itemCountText.text = inventoryItemArray[activePanel].ItemCount.ToString();
        }
        else
        {
            itemNameText.text = "";
            itemCountText.text = "";
        }

    }

    void SetImageOnPanel(int currentPanel, int currentItemID)
    {
        panelImages[currentPanel].sprite = Resources.Load<GameObject>($"Items/{currentItemID}").GetComponent<Item>().Preview;
    }

    void ClearImageOnPanel(int currentPanel)
    {
        panelImages[currentPanel].sprite = Resources.Load<Sprite>($"Materials/White");
    }

    public void OutputPickupText(int itemID, string itemName, int itemCount)
    {
        pickupText.text = $"Press F to pick up {itemName} {itemCount}x!";
        pickupTextPanel.SetActive(true);
    }

    public void ClearPickupText()
    {
        pickupText.text = "";
        pickupTextPanel.SetActive(false);
    }

    public void PickupItem(int itemID, GameObject throwItem)
    {
        int itemCount = 1;
        ThrownItem thrownItem = throwItem.GetComponent<ThrownItem>();
        if (thrownItem != null)
        {
            itemCount = thrownItem.currentItemCount;
        }

        if (TryAddItem(itemID, itemCount))
        {
            Destroy(throwItem.gameObject);
        }

    }
}
