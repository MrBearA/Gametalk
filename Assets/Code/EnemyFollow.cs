using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{

    Rigidbody rb;

    public GameObject player;
    public float speed;
    private float distance;
    public float distanceBetween;
    Animator animator;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //makes the enemy follow the player with smooth movement 
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //make it so it has a distance for it to follow the player 
        if (distance < distanceBetween)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            animator.SetFloat("moveX", distanceBetween);
        }
        else
        {
            animator.SetFloat("moveY", distanceBetween);
        }

        


    }
}
