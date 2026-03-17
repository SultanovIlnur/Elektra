using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    private float walkingSpeed = 7.5f;
    private float runningSpeed = 11.5f;
    private float jumpingSpeed = 8.0f;
    private float gravity = 20.0f;
    private float lookSpeed = 2.0f;
    private float lookXLimit = 85.0f;
    private float rotationX = 0;

    private float maxBuildAndDestroyDistance = 4.0f;
    private float maxPickupDistance = 3.0f;

    private bool canMove = true;



    //private GameObject currentItem;
    private int currentItemID;
    private int currentItemCount;

    private Vector3 moveDirection = Vector3.zero;
    [SerializeField]
    private Camera playerCamera;
    private CharacterController characterController;

    [SerializeField]
    private GameObject cubes;
    [SerializeField]
    private GameObject inventory;
    private Inventory inventoryScript;
    private Cubes cubesScript;

    /*public RaycastHit RayHit
    {
        get { return _rayHit; }
    }
    public Ray MainRay
    {
        get { return _ray; }
    }*/


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        inventoryScript = inventory != null ? inventory.GetComponent<Inventory>() : null;
        cubesScript = cubes != null ? cubes.GetComponent<Cubes>() : null;
        //Блокируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Мы на земле, поэтому пересчитаем направление движения, основанное на осях
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //Левая кнопка шифт для бега
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        //Движение
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        //обновляем направление движения по Y
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpingSpeed;

        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Передвигаем персонажа
        characterController.Move(moveDirection * Time.deltaTime);

        //Поворот игрока и камеры
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }


        MakeRay();
    }

    void MakeRay()
    {
        if (inventoryScript == null)
        {
            return;
        }

        Vector3 point = new Vector3(playerCamera.pixelWidth / 2, playerCamera.pixelHeight / 2, 0);
        Ray ray = playerCamera.ScreenPointToRay(point);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction);
        //при нажатии левой кнопки мыши ставим блок
        if (Input.GetMouseButtonDown(1))
        {
            InventoryItem currentInventoryItem = inventoryScript.inventoryItemArray[inventoryScript.ActivePanel];
            if (currentInventoryItem != null)
            {
                currentItemCount = currentInventoryItem.ItemCount;
                if (currentItemCount > 0)
                {
                    currentItemID = currentInventoryItem.ItemID;
                    Debug.Log($"Current item ID is equal {currentItemID}");
                    if (Physics.Raycast(ray, out hit, maxBuildAndDestroyDistance))
                    {
                        Debug.Log($"Yes, RMB {hit.collider} {hit.distance} {hit.point} {hit.transform} {hit.normal}");
                        Debug.Log(hit.transform.tag);
                        GameObject currentRayItem = hit.transform.gameObject;
                        Item currentRayItemScript = currentRayItem.GetComponent<Item>();
                        GameObject currentItemPrefab = Resources.Load<GameObject>($"Items/{currentItemID}");
                        Item currentItemScript = currentItemPrefab != null ? currentItemPrefab.GetComponent<Item>() : null;
                        if (currentRayItemScript != null && currentItemScript != null)
                        {
                            if (currentRayItemScript.ItemType == 0 && currentItemScript.ItemType == 0 && cubesScript != null)
                            {
                                Vector3 hitTransformPlace = hit.transform.position;
                                Vector3 hitNormal = hit.normal;
                                if (cubesScript.PlaceItem(hitTransformPlace + hitNormal, currentItemID))
                                {
                                    inventoryScript.DeleteItem(inventoryScript.ActivePanel, 1);
                                }
                            }
                        }
                    }
                    currentItemID = -1;
                }
            }
        }

        //при нажатии левой кнопки мыши убираем блок
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, maxBuildAndDestroyDistance))
            {
                GameObject currentRayItem = hit.transform.gameObject;
                Item currentRayItemScript = currentRayItem.GetComponent<Item>();
                if (currentRayItemScript != null)
                {
                    if (currentRayItemScript.ItemType == 0)
                    {
                        if (inventoryScript.TryAddItem(currentRayItemScript.ItemID, 1))
                        {
                            GameObject hitTransform = hit.transform.gameObject;
                            Destroy(hitTransform);
                        }

                    }
                }



            }

        }
        else
        {
            //проверка на выкинутые объекты
            if (Physics.Raycast(ray, out hit, maxPickupDistance))
            {
                GameObject currentRayItem = hit.transform.gameObject;
                if (currentRayItem.CompareTag("Thrown"))
                {
                    ThrownItem thrownItemScript = currentRayItem.GetComponent<ThrownItem>();
                    if (thrownItemScript != null)
                    {
                        inventoryScript.OutputPickupText(thrownItemScript.currentItemID, thrownItemScript.currentItemName, thrownItemScript.currentItemCount);

                        //если мы нажали f на выкинутом объекте
                        if (Input.GetKeyDown("f"))
                        {
                            inventoryScript.PickupItem(thrownItemScript.currentItemID, currentRayItem);
                        }
                    }
                    else
                    {
                        inventoryScript.ClearPickupText();
                    }
                }
                else
                {
                    inventoryScript.ClearPickupText();
                }
            }
            else
            {
                inventoryScript.ClearPickupText();
            }



        }



    }
}
