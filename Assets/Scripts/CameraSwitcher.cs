using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour, IShowable
{
    private CharacterController characterController;

    public GameObject crosshair;
    public Camera firstCamera;
    public Camera secondCamera;
    private string currentValue = "Laptop";

    private bool isPlayerInTrigger = false;

    public string value { get => currentValue; }

    private void Start()
    {
        Cursor.visible = false;
        characterController = GameObject.Find("Player").GetComponent<CharacterController>();
    }
    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            crosshair.SetActive(false);
            Cursor.visible = true;
            Debug.Log("Switching Cameras");
            firstCamera.gameObject.SetActive(false);
            secondCamera.gameObject.SetActive(true);
            characterController.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q))
        {
            crosshair.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            firstCamera.gameObject.SetActive(true);
            secondCamera.gameObject.SetActive(false);
            characterController.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enter");
            isPlayerInTrigger = true;
            currentValue = "Laptop";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            currentValue = "";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            currentValue = "Laptop";
        }
    }
}