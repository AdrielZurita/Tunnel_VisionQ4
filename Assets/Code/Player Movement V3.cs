using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV3 : MonoBehaviour
{
    [Header("Shmovin")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCoolDown;
    private float tempSpeed;

    [Header("Yumpin")]
    bool readyToJump = true;
    public float jumpGravity;
    public float sprintSpeedMultiplier = 1.2f;
    public float airControl = 0.8f;
    public float jumpFloat = 0.2f;
    float tempGravity;

    [Header("Ground-Checkinin")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float heightCheckOffset = 0.2f;
    bool grounded;
    public float CoyoteTime = 0.15f;
    bool isRunningCoroutine;

    [Header("Orientationifyinin")]
    public Transform orientation; 
    float horizontalInput;
    float verticalInput; 
    Vector3 moveDirection; 
    Rigidbody rb;

    [Header("DevMode")]
    public bool DevMode;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        grounded = true;
        isRunningCoroutine = false;
        readyToJump = true;
    }

    void Update()
    {
        //tracks WASD Space and LShift
        MyInput();

        //tracks 
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void LateUpdate()
    {
        if (Physics.Raycast(transform.localPosition, -transform.up, playerHeight * 0.5f + heightCheckOffset, whatIsGround))
        {
            grounded = true;
            tempGravity = 0;
        }
        else if (!isRunningCoroutine)
        {
            StartCoroutine(CoyoteJump());
            isRunningCoroutine = true;
        }
    }

    private void MyInput()
    {
        if(DevMode && Input.GetKey(KeyCode.F1))
        {
            print(grounded);
        }
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("pressing Space");
            if(readyToJump && grounded)
            {
                tempGravity = jumpGravity * (1 - jumpFloat);
                Jump(); 
                readyToJump = false;
                //Invoke(nameof(ResetJump), jumpCoolDown);
                print("yumped");
            }
            
            if (!grounded)
            {
                tempGravity = jumpGravity * (1 - jumpFloat);
            }
        }
        else
        {
            tempGravity = jumpGravity;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            tempSpeed = moveSpeed * sprintSpeedMultiplier;
            
        }
        else
        {
            tempSpeed = moveSpeed;
        }
    }

    private void MovePlayer()
    {
        moveDirection = verticalInput * orientation.forward + horizontalInput * orientation.right;
        
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * tempSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(transform.up * tempGravity, ForceMode.Impulse);
            rb.AddForce(moveDirection.normalized * tempSpeed * 10f * airControl, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > tempSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * tempSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /*private void ResetJump()
    {
        readyToJump = true;
    }*/

    /*void OnCollisionStay(Collision other)
    {
    
        if (other.gameObject.tag == "Ground")
        {
            CanJump = true;
        }
        else
        {
            
        }  
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            CoyoteJump();
        }
    }*/

    IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(CoyoteTime);
        grounded = false;
        isRunningCoroutine = false;
        readyToJump = true;
    }
}
