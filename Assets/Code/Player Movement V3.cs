using System.Collections;
using UnityEngine;

public class PlayerMovementV3 : MonoBehaviour
{
    [Header("Shmovin")]
    public float moveSpeed = 20f;             // Base movement speed on the ground.
    public float groundDrag = 3.5f;           // Drag applied when grounded.
    public float jumpForce = 10f;             // Impulse force for a normal jump.
    public float jumpCoolDown = 0.5f;         // Optional cooldown between jumps.
    public float tempSpeed;                  // Runtime speed, changes when sprinting.
    public float speedCap = 1;                // Max percentage of speed allowed.
    public float accelerationSpeed = 1f;      // How quickly the player reaches max speed.
    float tempAcceleration = 1f;

    [Header("Yumpin")]
    bool readyToJump = true;                  // Tracks whether jump input is allowed.
    public float jumpGravity;                 // Downwards force applied while in air.
    public float sprintSpeedMultiplier = 1.2f;// Sprint speed multiplier.
    public float airControl = 0.8f;            // Movement control while airborne.
    public float jumpFloat = 0.2f;             // Small upward force while holding jump.
    public float AirDrag = 0.1f;               // Drag applied while in air.
    [SerializeField] bool flutterJump;         // Tracks whether extra jump float is active.

    [Header("Ground-Checkinin")]
    public float playerHeight;                 // Half height used for ground raycast.
    public LayerMask whatIsGround;             // Layers counted as ground.
    public float heightCheckOffset = 0.2f;     // Extra raycast length for ground check.
    [SerializeField] bool grounded;           // Is the player currently standing on the ground?
    public float CoyoteTime = 0.15f;           // Time window after leaving ground to still jump.
    bool isRunningCoroutine;                  // Prevents multiple coyote coroutines from running.

    [Header("Orientationifyinin")]
    public Transform orientation;              // Direction reference for movement input.
    float horizontalInput;                    // Raw input from A/D or left/right keys.
    float verticalInput;                      // Raw input from W/S or up/down keys.
    Vector3 moveDirection;                    // Combined movement direction.
    Rigidbody rb;                             // Player physics body.

    [Header("DevMode")]
    public bool DevMode;                       // Toggle debugging shortcuts.

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;              // Prevent physics from rotating the player.

        grounded = true;
        isRunningCoroutine = false;
        readyToJump = true;
        
    }

    void Update()
    {
        // Read player input and decide movement state.
        MyInput();

        // Keep horizontal speed below the configured cap.
        SpeedControl();

        // Apply different drag depending on grounded state.
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = AirDrag;
        }
    }

    void FixedUpdate()
    {
        // Apply movement forces in the physics update.
        MovePlayer();
    }

    void LateUpdate()
    {
        // Cast downward to determine whether the player is grounded.
        if (Physics.Raycast(transform.localPosition, -transform.up, playerHeight * 0.5f + heightCheckOffset, whatIsGround))
        {
            grounded = true;
            readyToJump = true;                  // Reset jump ability immediately when on the ground.
        }
        else if (!isRunningCoroutine)
        {
            // Start coyote time if the player has just left the ground.
            StartCoroutine(CoyoteJump());
            isRunningCoroutine = true;
        }
    }

    private void MyInput()
    {
        if (DevMode && Input.GetKey(KeyCode.F1))
        {
            print(grounded);                   // Debug: print grounded state.
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (readyToJump && grounded)
            {
                flutterJump = false;            // Reset float support before jumping.
                Jump();
                readyToJump = false;
            }
        }

        // Hold space after jumping to apply a small float boost.
        flutterJump = Input.GetKey(KeyCode.Space);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            tempSpeed = moveSpeed * sprintSpeedMultiplier * tempAcceleration; 
            
            tempAcceleration += accelerationSpeed * Time.deltaTime; // Gradually increase speed while sprinting.
        }
        else
        {
            tempSpeed = moveSpeed;
            tempAcceleration = 1f; // Reset acceleration when not sprinting.
        }
    }

    private void MovePlayer()
    {
        // Convert input into movement direction relative to orientation.
        moveDirection = verticalInput * orientation.forward + horizontalInput * orientation.right;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * tempSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * tempSpeed * 10f * airControl, ForceMode.Force);
        }

        if (flutterJump) 
        {
            rb.AddForce(transform.up * jumpFloat, ForceMode.Impulse);
        }

        rb.AddForce(transform.up * jumpGravity, ForceMode.Impulse);
        
    }

    private void SpeedControl()
    {
        // Only limit horizontal velocity, preserve current vertical velocity.
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > tempSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * tempSpeed * speedCap;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        if (tempSpeed > moveSpeed * sprintSpeedMultiplier * speedCap)
        {
            tempSpeed = moveSpeed * sprintSpeedMultiplier * speedCap; // Cap sprint speed.
        }
    }

    private void Jump()
    {
        // Reset vertical velocity before jumping for consistent height.
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(CoyoteTime);
        grounded = false;
        isRunningCoroutine = false;
        readyToJump = true;                   // Allow jump again after coyote time.
    }
}
