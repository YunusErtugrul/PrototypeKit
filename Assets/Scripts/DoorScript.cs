using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] public GameObject CameraObject;
    [SerializeField] GameObject text;
    //[SerializeField] GameObject keyObject;
    //[SerializeField] GameObject lockObject;
    [Header("Sound")]

    [Header("Animation")]
    public Animator Animator;
    public bool inReach;
    public bool DoorOpen;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
    }

    // Update is called once per frame
   public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Doors();
        }
    }

    public void Doors()
    {
        RaycastHit hit;
        if(Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out hit, 5))
        {
            if (hit.collider.CompareTag("Doors/Door"))
            {
                inReach = true;
            }
            else
            {
                inReach = false;
            }
            if(inReach && hit.collider.CompareTag("Doors/Door"))
            {
                Animator.SetTrigger("open");
                Debug.Log("Door Open!");
                DoorOpen = true;
            }
            if(!inReach && hit.collider.CompareTag("Doors/Door"))
            {
                Animator.SetTrigger("close");
            }
        }
    }
}
