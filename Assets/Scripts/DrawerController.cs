using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawerController : MonoBehaviour, IInteractable, IShowable
{
    private bool isOpen;
    [SerializeField] private Transform endPosition;
    private float lerp = 2;
    public GameObject drawer;
    private string curruentValue = "Drawer";
    public AudioSource drawerAudio;
    public AudioClip openDrawer;
    public AudioClip closeDrawer;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = drawer.transform.position;
    }

    public void Drawer()
    {
        if(!isOpen)
        {
            isOpen = true;
            curruentValue = "Drawer";
            drawer.transform.DOMove(endPosition.position, lerp);
            drawerAudio.PlayOneShot(openDrawer);
        }
        else
        {
            isOpen = false;
            curruentValue = "Drawer";
            drawer.transform.DOMove(startPosition, lerp);
            drawerAudio.PlayOneShot(closeDrawer);
        }
    }


    public string value { get => curruentValue; }

    public void Interact()
    {
        Drawer();
    }
}
