using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 6.0f;

    //This will define how the player will move at any instant
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    // This is a layer mask which the ray from camera will hit
    int floorMask;
    float cameraRayLength = 100.0f;

    //Awake function is enabled regardless of whether script is enabled or not
    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        //Getting reference to the animator
        anim = GetComponent<Animator>();
        //Getting reference to the Player Rigidbody
        playerRigidbody = GetComponent<Rigidbody>();
    }

    //FixedUpdate updates for every change in physics (Update updates for every render change)
    void FixedUpdate()
    {
        //GetAxis obtains any floating value between -1 and 1. GetAxisRaw gives either -1, 0, or 1. It is used here to snap the velocity of the player to full speed without slow acceleration
        //Horizontal maps from A, D, Left and Right keys
        float horiz = Input.GetAxisRaw("Horizontal");
        //Vertical maps from W, S, Up and Down keys
        float vert = Input.GetAxisRaw("Vertical");
        Move(horiz, vert);
        Turning();
        Animating(horiz, vert);
    }

    //Controls default movement of Player
    void Move(float horiz, float vert)
    {
        movement.Set(horiz, 0f, vert);
        //Normalizing movement so that Player moves at the same speed even in the diagonal vector
        //Multiply by Time.deltaTime so that movement is only once per second instead of per FixedUpdate
        movement = movement.normalized * speed * Time.deltaTime;
        //Moves the Player to the current position + the desired movement
        playerRigidbody.MovePosition(transform.position + movement);
    }

    //Controls orientation of Player
    void Turning()
    {
        //Ray from camera to Floor Quad to determine which direction the Player has to face, controlled by Mouse
        //ScreenPointToRay takes a point on the screen and casts a ray onto the scene
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit variable contains information about the Raycast
        RaycastHit floorHit;
        //Check whether cameraRay hit something on only the floorMask layer, output info to floorHit, checked for a length of cameraRayLength
        if(Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            //Vector between the Player and the hit position
            Vector3 playerToMousePosition = floorHit.point - transform.position;
            //To make sure Player's y rotation remains 0
            playerToMousePosition.y = 0f;
            //Use Quaternions to store rotation directions, instead of using Vector3
            //LookRotation changes the z-axis as forward axis, to playerToMousePosition vector's axis
            Quaternion newRotation = Quaternion.LookRotation(playerToMousePosition);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    //Controls Player animations
    void Animating(float horiz, float vert)
    {
        //isWalking is set to true, if value of horiz or vert parameters are not 0
        bool isWalking = horiz != 0f || vert != 0f;
        //Set the boolean parameter in Animator Controller to be equal to the isWalking bool
        anim.SetBool("IsWalking", isWalking);
    }

}
