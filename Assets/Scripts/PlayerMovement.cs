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

    private Vector2 moveDir;
    public float friction = 10f;
    public float maxSpeed = 20f;
    public float acceleration = 5f;

    public float jumpHeight = 1.5f;
    public float gravity = 9.81f;

    //only used to see in debug because the character controller doesn't show this value in the editor
    bool isGrounded;
    public Transform groundChecker;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

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
        Debug.Log("jump");
        jumping = true;
        velocityY = Mathf.Sqrt(jumpHeight * gravity);
    }

    //get movement axis from input system
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>().normalized;
    }

    private void ProcessMovement()
    {
        velocity2d = new Vector2(characterController.velocity.x, characterController.velocity.z)*Time.deltaTime;
        velocity2d += moveDir * acceleration * Time.deltaTime;
        if (velocity2d.magnitude > maxSpeed)
        {
            Debug.Log("max speed");
            velocity2d = velocity2d.normalized * maxSpeed * Time.deltaTime;
        }

        Vector2 prevVel = velocity2d.normalized;
        velocity2d -= velocity2d.normalized * friction * Time.deltaTime;
        if (Vector2.Dot(velocity2d.normalized, prevVel.normalized) < 0f)
        {
            Debug.Log("stopped");
            velocity2d = Vector2.zero;
        }
        currentSpeed = velocity2d.magnitude;

        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer);
        if (isGrounded && jumping && velocityY < 0)
        {
            Debug.Log("landed");
            jumping = false;
            velocityY = 0;
        }
        else if(!isGrounded) velocityY -= gravity * Time.deltaTime;
        characterController.Move(new Vector3(velocity2d.x, velocityY, velocity2d.y));
        //Debug.Log("Velocity: " + velocity2d + velocity2d.sqrMagnitude);
    }
}
 