using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float xRotation = 0;
    public Transform playerPosition;
    private float mouseSens = 400f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerPosition.Rotate(Vector3.up * mouseX);
    }
}
