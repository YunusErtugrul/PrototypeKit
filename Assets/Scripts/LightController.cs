using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightController : MonoBehaviour, IInteractable, IShowable
{
    [SerializeField] private GameObject lightObject;
    [SerializeField] private AudioSource light_open;
    private string curruentValue = "Light";
    private bool isOpen;
    void OpenLight()
    {
        if (!isOpen)
        {
            lightObject.SetActive(true);
            isOpen = true;
            curruentValue = "Light";
            light_open.Play();
        }
        else
        {
            isOpen = false;
            lightObject.SetActive(false);
            curruentValue = "Light";
            light_open.Play();
        }
    }


    public string value { get => curruentValue; }

    public void Interact()
    {
        OpenLight();
    }
}
