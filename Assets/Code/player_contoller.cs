using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    

    private Vector2 moveInput;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Rigidbody2D rb;
    private Animator animator;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

  
    public void FixedUpdate()
    {
        if (!IsOwner) return;
        // rb.MovePosition(rb.position + (moveInput * moveSpeed * Time.fixedDeltaTime));

        if (moveInput != Vector2.zero)
        {
            // Try to move player in input direction, followed by left right and up down input if failed
            bool success = MovePlayer(moveInput);

            if (!success)
            {
                // Try Left / Right
                success = MovePlayer(new Vector2(moveInput.x, 0));

                if (!success)
                {
                    success = MovePlayer(new Vector2(0, moveInput.y));
                }
            }
            if (!IsOwner) return;

            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
        } 


    }

    // Tries to move the player in a direction by casting in that direction by the amount
    // moved plus an offset. If no collisions are found, it moves the players
    // Returns true or false depending on if a move was executed
    public bool MovePlayer(Vector2 direction)
    {
        if (!IsOwner) return false;
        // Check for potential collisions
        int count = rb.Cast(
            direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
            castCollisions, // List of collisions to store the found collisions into after the Cast is finished
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

        if (count == 0)
        {
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;

            // No collisions
            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            // Print collisions
            foreach (RaycastHit2D hit in castCollisions)
            {
                print(hit.ToString());  
            }

            return false;
        }

       
    }

    public void OnMove(InputValue value)
    {
        if (!IsOwner) return;
        moveInput = value.Get<Vector2>();

        // Only set the animation direction if the player is trying to move
        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("moveX", moveInput.x);
            animator.SetFloat("moveY", moveInput.y);
        }
    }


    public void OnFire()
    {
        print("Shots fired");
    }
}

