using System.Collections;
using UnityEngine;

public class PlayerMovementV3 : MonoBehaviour
{
    [Header("Shmovin")]
    public float maxWalkSpeed = 20f;             // Base movement speed on the ground.
    public float groundDrag = 3.5f;           // Drag applied when grounded.
    public float jumpForce = 40f;             // Impulse force for a normal jump.
    public float jumpCoolDown = 0.5f;         // Optional cooldown between jumps.
    public float currentSpeed;                  // Runtime speed, changes when sprinting.
    public float speedCap = 2f;                // Max percentage of speed allowed.
    public float walkAccelerationRate = 0.6f;      // How quickly the player reaches max speed while walking.
    public float sprintAccelerationRate = 0.3f;      // How quickly the player reaches max speed while sprinting.
    float currentSprintAcceleration = 1f;

    [Header("Yumpin")]
    public float jumpGravity;                 // Downwards force applied while in air.
    bool readyToJump = true;                  // Tracks whether jump input is allowed.
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
            rb.drag = groundDrag; // Apply ground drag to slow down when on the ground.
        }
        else
        {
            rb.drag = AirDrag; // Apply air drag to slow down when in the air.
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
            print(rb.velocity);                // Debug: print current velocity.
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0) // if pressing WASD
        {    
            if (currentSpeed < 4f)
            {
                currentSpeed = 4f; // eases acceleration from 0 to prevent slow start when beginning to move.
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (currentSprintAcceleration < 1f)
                {
                    currentSprintAcceleration = 1f; // Reset acceleration to 1 when below.
                }
                
                if (currentSpeed < maxWalkSpeed * currentSprintAcceleration)
                {       
                    currentSpeed += maxWalkSpeed * currentSprintAcceleration * Time.deltaTime; // increases speed by the accelerated value.
                }
            
                currentSprintAcceleration += sprintAccelerationRate * sprintSpeedMultiplier * Time.deltaTime; // accelerates player while sprinmting.

                if (currentSpeed > maxWalkSpeed * speedCap)
                {
                    currentSpeed = maxWalkSpeed * speedCap; // Cap sprint speed.

                    currentSprintAcceleration = speedCap/maxWalkSpeed; // Cap acceleration to match speed cap.
                }
            }
            else
            {
                if (currentSpeed < maxWalkSpeed)
                {       
                    currentSpeed += maxWalkSpeed * walkAccelerationRate * Time.deltaTime; // Gradually increase speed when walking.
                }

                if (currentSprintAcceleration >= 1f)
                {
                    currentSprintAcceleration -= 0.5f * currentSprintAcceleration * Time.deltaTime; // Slows down player (Reference 1)
                }
                else if (currentSprintAcceleration < 1f)
                {
                    currentSprintAcceleration = 1f; // Prevents slowing to below 1 and slowing base walk speed
                }

                if (currentSpeed > maxWalkSpeed * currentSprintAcceleration)
                {
                    currentSpeed = maxWalkSpeed * currentSprintAcceleration; // Applies slowdown to player from Ref. 1
                }
            }
        }
        else // if not pressing WASD
        {

            if (currentSpeed >= 0f)
            {
                currentSpeed -= ( 0.9f * currentSpeed + 2f ) * Time.deltaTime; // slows player when not moving
            }
            else if (currentSpeed < 0.1f)
            {
                currentSpeed = 0f; // Prevents speed from being negative
            }
            //Debug.Log(currentSpeed);
        }

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

        
    }

    private void MovePlayer()
    {
        // Convert input into movement direction relative to orientation.
        moveDirection = verticalInput * orientation.forward + horizontalInput * orientation.right;

        if (grounded)
        {
            rb.AddForce(moveDirection * currentSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection * currentSpeed * 10f * airControl, ForceMode.Force);
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

        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed * speedCap;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
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
