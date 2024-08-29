using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 10f;
    private float gravity = -19.65f;
    Vector3 velocity;
    public CharacterController player;
    private float jumpForce = 2;

    bool isGround;
    private float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.Find("Player").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        player.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        player.Move(velocity * Time.deltaTime);
    }
}
