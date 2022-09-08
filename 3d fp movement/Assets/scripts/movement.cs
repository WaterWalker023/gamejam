using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 12f;
    public float gravity = -10f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayerMask;
    public float jump = 3f;

    Vector3 Velocity;
    public bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);
        
        if (isGrounded && Velocity.y < 0)
            Velocity.y = -2;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 20f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            speed = 5;
        }
        else
        {
            speed = 12f;
        }
        

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
            Velocity.y = Mathf.Sqrt(jump * -2f * gravity);



        Velocity.y += gravity * Time.deltaTime;
        characterController.Move(Velocity * Time.deltaTime);
    }
}
