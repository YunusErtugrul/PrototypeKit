using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    public GameObject cam;
    private float distance = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            takeItem();
        }
    }

    void takeItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
        {
            if (hit.collider.CompareTag("AvailableItem"))
            {
                Debug.Log("Take item!");
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
