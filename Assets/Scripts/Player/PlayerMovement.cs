using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CreatureMovement creature;
    

    float xMove;
    float zMove;

    public Transform playerOrientation;
    public CharacterController controller;
    public Collider creatureSword;

    float currentspeed;
    float walkSpeed = 7f;
    float sprintSpeed = 13f;
    float strafeSprintSpeed = 9f;
    float sidestepSpeed = 5f;
    float backstepSpeed = 3f;
    public bool isSprinting;

    public float gravity = -9.8f;
    Vector3 velocity;

    public float jumpheight = .5f;
    

    public Transform ground;
    public float groundDistance = .4f;
    public LayerMask groundMask;
    public bool isGrounded;

    public bool wasHit;

    private float DistancefromTurret;
      

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
      
        if (Input.GetKey(KeyCode.LeftShift) && zMove == 1 && xMove == 0)
        {
            movementSpeed(direction, sprintSpeed);
            isSprinting = true;
        }
        else if(Input.GetKey(KeyCode.LeftShift) && zMove == 1 && xMove != 0)
        {
            movementSpeed(direction, strafeSprintSpeed);
            isSprinting = true;
        }
        else if(zMove == -1)
        {
            movementSpeed(direction, backstepSpeed);
            isSprinting = false;
        }
        else if(xMove != 0)
        {
            movementSpeed(direction, sidestepSpeed);
            isSprinting = false;
        }
        else
        {
            movementSpeed(direction, walkSpeed);
            isSprinting = false;
        }
    }

    private void playerJump()
    {     
        if(Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }
    }

    private void movementSpeed(Vector3 direction,float speed)
    {
        controller.Move(direction * speed * Time.deltaTime);
        currentspeed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other = creatureSword;
        if (creature.isAttacking)
        {
            wasHit = true;
        }
    }
}
