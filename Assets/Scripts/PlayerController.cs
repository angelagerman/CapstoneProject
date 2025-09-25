using System;
using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // How fast the player moves
    public float jumpForce = 5f; // How high the player jumps
    public float gravityScale = 2f; // Controls how strong gravity is on the player

    private Rigidbody rb;
    private bool isGrounded;
    
    private Vector3 moveDirection;
    private Vector3 currentMoveDirection;
    public Transform cameraRig;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (rb != null)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            
            if (moveDirection.magnitude >= 0.1f)
            {
                Vector3 camForward = cameraRig.forward;
                camForward.y = 0f;
                camForward.Normalize();

                Vector3 camRight = cameraRig.right;
                camRight.y = 0f;
                camRight.Normalize();

                // Move direction is relative to camera
                Vector3 targetDirection = camForward * vertical + camRight * horizontal;
                targetDirection.Normalize();

                // Store for movement
                currentMoveDirection = targetDirection;
            }
            else
            {
                currentMoveDirection = Vector3.zero;
            }

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        float currentSpeed = moveDirection.magnitude;
        animator.SetFloat("Speed", currentSpeed);
    }
    
    void FixedUpdate()
    {
        
        if (currentMoveDirection.magnitude >= 0.1f)
        {
            //actual moving
            Vector3 move = currentMoveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
            
            //spinnnnn
            Quaternion toRotation = Quaternion.LookRotation(currentMoveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.fixedDeltaTime);
        }
        
        ApplyGravity();
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        
        animator.SetBool("isJumping", true);
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;

        animator.SetBool("isJumping", false);
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    
    
}