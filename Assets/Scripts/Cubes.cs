using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{
    [SerializeField]
    private GameObject fpsController;
    //private RaycastHit _rayHit;
    //private Ray _ray;
    // Start is called before the first frame update
    void Start()
    {
        //_rayHit = _fpsController.GetComponent<FPSController>().RayHit;
        //_ray = _fpsController.GetComponent<FPSController>().MainRay;
        PlaceItem(new Vector3(0, 0, 0), 0);
        for (int i = 0; i < 50; i++)
        {
            for (int k = 0; k < 50; k++)
            {
                PlaceItem(new Vector3(k, -1, i), 0);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        //transform.position += transform.TransformDirection(transform.position);
    }
    public void PlaceItem(Vector3 coords, int itemID)
    {
        GameObject currentItem = Resources.Load<GameObject>($"Items/{itemID}");
        Instantiate(currentItem, coords, Quaternion.identity, transform);
    }


}
