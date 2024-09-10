using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{
    [SerializeField] public GameObject cam;
    private float distance = 2f;
    public bool flashActive = false;
    [SerializeField] public GameObject crosshair;
    [SerializeField] public GameObject flashLight;
    [SerializeField] public ItemChange ItemScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            takeItem();
        }
    }

    void takeItem()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
        {
            if (hit.collider.CompareTag("InteractiveItem/CUBE"))
            {
                Debug.Log("Take item!");
                Destroy(hit.collider.gameObject);
            }
        }
        if(Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, distance))
        {
            if (hit.collider.CompareTag("InteractiveItem/FLASHLIGHT"))
            {
                flashActive = true;
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
