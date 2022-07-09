using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float xMove;
    float zMove;

    public Transform playerOrientation;
    public CharacterController controller;

    [SerializeField] float speed = 1f;
    [SerializeField] float sprintSpeed = 15f;
    public float gravity = -9.8f;
    Vector3 velocity;

    public float jumpheight = .5f;
    

    public Transform ground;
    public float groundDistance = .4f;
    public LayerMask groundMask;
    public bool isGrounded;
      

    // Update is called once per frame
    void Update()
    { 
        xMove = Input.GetAxisRaw("Horizontal");
        zMove = Input.GetAxisRaw("Vertical");
        isGrounded = Physics.CheckSphere(ground.position, groundDistance, groundMask);
        controller.Move(velocity * Time.deltaTime);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            playerJump();
        }

        velocity.y += gravity * Time.deltaTime;

    }

    private void FixedUpdate()
    {
        
        playerMove();         
       
    }

    private void playerMove()
    {
        Vector3 direction;
        
        direction = playerOrientation.forward * zMove + playerOrientation.right * xMove;
      
        if(Input.GetKey(KeyCode.LeftShift) && zMove != -1)
        {
            controller.Move(direction * sprintSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
        
    }

    private void playerJump()
    {     
        if(Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }
    }

    

    
}
