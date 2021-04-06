
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
    int ignorePanel;
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

        AddItem(100, 4, 30);
        AddItem(3, 5, 128);
        AddItem(4, 6, 128);
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
        ignorePanel = 0;
        for (int i = 0; i < 9; i++)
        {
            panels[i] = GameObject.Find($"GuiCanvas/InvPanel/{i + 1}");
            panelsColor[i] = panels[i].GetComponent<Image>();
            panelImages[i] = GameObject.Find($"GuiCanvas/InvPanel/{i + 1}/Image").GetComponent<Image>();
        }
    }

    void CheckForPanelButtonsDown()
    {


        //отслеживание нажатий клавиатуры изменения активной панели инвентаря
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown($"{i + 1}"))
            {
                //GameObject invPanel = GameObject.Find($"GuiCanvas/InvPanel/{i}");
                //invPanel.GetComponent<Image>().color = selectedColor;
                panelsColor[i].color = selectedColor;
                activePanel = i;
                ignorePanel = i;
            }
            else
            {
                if (i != ignorePanel)
                {
                    //GameObject invPanel = GameObject.Find($GuiCanvas/InvPanel/{i}");
                    //invPanel.GetComponent<Image>().color = unselectedColor;
                    panelsColor[i].color = unselectedColor;

                }
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
            panelsColor[newActivePanel].color = selectedColor;
            activePanel = newActivePanel;
            ignorePanel = newActivePanel;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            int newActivePanel = activePanel + 1;
            if (newActivePanel > 8)
            {
                newActivePanel = 0;
            }
            panelsColor[newActivePanel].color = selectedColor;
            activePanel = newActivePanel;
            ignorePanel = newActivePanel;
        }

        //кнопка для выкидывания объекта из руки
        if (Input.GetKeyDown("q"))
        {
            if (inventoryItemArray[activePanel] != null)
            {
                ThrowOnGround(inventoryItemArray[activePanel].ItemID);
                DeleteItem(activePanel, 1);

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

    public void AddItem(int itemID, int panelNumber, int itemCount)
    {
        Debug.Log($"{itemID} {panelNumber} {itemCount} {inventoryItemArray}");
        if (inventoryItemArray[panelNumber] != null)
        {
            Debug.Log("1");
            if (inventoryItemArray[panelNumber].ItemCount >= 1 && inventoryItemArray[panelNumber].ItemID == itemID)
            {
                Debug.Log("2");
                inventoryItemArray[panelNumber].ItemCount += itemCount;
            }
        }
        else
        {
            Debug.Log("3");
            inventoryItemArray[panelNumber] = (InventoryItem)ScriptableObject.CreateInstance("InventoryItem");
            inventoryItemArray[panelNumber].ItemCount += itemCount;
            inventoryItemArray[panelNumber].ItemID = itemID;
            SetImageOnPanel(panelNumber, itemID);
        }




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
    void ThrowOnGround(int currentID)
    {
        Vector3 fpsCameraPosition = fpsCamera.transform.position;
        GameObject thrownItem = Resources.Load<GameObject>("ThrownItem");
        thrownItem.GetComponent<ThrownItem>().ImportItemStats(currentID);
        GameObject model = Resources.Load<GameObject>($"Items/{currentID}").GetComponent<Item>().Model;
        GameObject modelInstanced = Instantiate<GameObject>(model, Vector3.zero, Quaternion.identity);
        GameObject thrownItemInstanced = Instantiate<GameObject>(thrownItem, fpsCameraPosition + transform.forward * 0.3f, Quaternion.identity);
        modelInstanced.transform.parent = thrownItemInstanced.transform;
        modelInstanced.transform.localPosition = Vector3.zero;
        modelInstanced.transform.localScale *= 0.3f;
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
        if (inventoryItemArray[activePanel] == null)
        {
            AddItem(itemID, activePanel, 1);
            Destroy(throwItem.gameObject);
        }
        else if (inventoryItemArray[activePanel].ItemID == itemID)
        {
            AddItem(itemID, activePanel, 1);
            Destroy(throwItem.gameObject);
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                if (inventoryItemArray[i] == null)
                {
                    AddItem(itemID, i, 1);
                    Destroy(throwItem.gameObject);
                    break;
                }
                else if (inventoryItemArray[i].ItemID == itemID)
                {
                    AddItem(itemID, i, 1);
                    Destroy(throwItem.gameObject);
                    break;
                }
            }
        }
        
    }
}
