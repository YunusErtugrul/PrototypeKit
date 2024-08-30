using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private CharacterController characterController;

    public GameObject crosshair;
    public Camera firstCamera;
    public Camera secondCamera;

    private bool isPlayerInTrigger = false;

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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }
}