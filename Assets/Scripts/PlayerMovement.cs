using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private bool jumping = false;
    public CharacterController characterController;

    private Vector2 velocity2d = Vector2.zero;

    private float velocityY = 0;
    private float currentSpeed;

    private Vector2 move;
    public float friction = 10f;
    public float maxSpeed = 20f;
    public float acceleration = 5f;

    public float jumpHeight = 1.5f;
    public float gravity = 9.81f;

    //only used to see in debug because the character controller doesn't show this value in the editor
    bool isGrounded; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        ProcessMovement();
    }

    //event triggered by Input System when jump button pressed
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started || jumping) return;
        jumping = true;
        //velocityY += Mathf.Sqrt(jumpHeight * gravity);
    }

    //get movement axis from input system
    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log(context.action);
        move = context.ReadValue<Vector2>().normalized;
        //Debug.Log(move);
        //velocity.x += move.x * acceleration * Time.deltaTime;
        //velocity.z += move.y * acceleration * Time.deltaTime;
    }

    private void ProcessMovement()
    {
        //Debug.Log("moveDir" + move);
        //velocity2d = new Vector2(characterController.velocity.x, characterController.velocity.z);
        velocity2d += move * acceleration;
        if (velocity2d.magnitude > maxSpeed)
        {
            Debug.Log("max speed");
            velocity2d = velocity2d.normalized * maxSpeed;
        }

        Vector2 prevVel = velocity2d.normalized;
        velocity2d -= velocity2d.normalized * friction;
        if (Vector2.Dot(velocity2d.normalized, prevVel.normalized) < 0f)
        {
            Debug.Log("stopped");
            velocity2d = Vector2.zero;
        }
        currentSpeed = velocity2d.sqrMagnitude;

        //float xSign = Mathf.Sign(velocity2d.x);
        //float zSign = Mathf.Sign(velocity2d.z);
        //velocity2d.x -= xSign * friction * Time.deltaTime;
        //velocity2d.z -= zSign * friction * Time.deltaTime;
        //if (Mathf.Sign(velocity2d.x) != xSign) velocity2d.x = 0;
        //if (Mathf.Sign(velocity2d.z) != zSign) velocity2d.z = 0;


        //if (characterController.isGrounded)
        //{
        //    jumping = false;
        //    velocityY = 0;
        //}
        //else velocityY -= gravity;
        characterController.Move(new Vector3(velocity2d.x, velocityY, velocity2d.y) * Time.deltaTime);
        //Debug.Log("Velocity: " + velocity2d + velocity2d.sqrMagnitude);
    }
}
 