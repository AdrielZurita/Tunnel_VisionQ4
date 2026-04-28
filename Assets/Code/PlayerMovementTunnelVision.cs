using System.Collections;
using UnityEngine;

public class PlayerMovementTunnelVision : MonoBehaviour
{
    [Header("Shmovin")]
    public float maxWalkSpeed = 20f;             // Base movement speed on the ground.
    public float groundDrag = 3.5f;           // Drag applied when grounded.
    public float jumpForce = 40f;             // Impulse force for a normal jump.
    public float jumpCoolDown = 0.5f;         // Optional cooldown between jumps.
    public float currentSpeed = 0f;                  // Runtime speed, changes when sprinting.
    public float speedCap = 2f;                // Max percentage of speed allowed.
    public float walkAccelerationRate = 0.6f;      // How quickly the player reaches max speed while walking.
    public float sprintAccelerationRate = 0.3f;      // How quickly the player reaches max speed while sprinting.
    public float sprintSpeedMultiplier = 1.5f;    // Sprint speed multiplier.
    private float currentSprintAcceleration = 1f;

    [Header("Yumpin")]
    public float jumpGravity = -1f;                 // Downwards force applied while in air.
    private bool readyToJump = true;                  // Tracks whether jump input is allowed.
    public float airControl = 0.8f;            // Movement control while airborne.
    public float jumpFloat = 0.6f;             // Small upward force while holding jump.
    public float AirDrag = 0.1f;               // Drag applied while in air.
    [SerializeField] private bool flutterJump;         // Tracks whether extra jump float is active.

    [Header("Ground-Checkinin")]
    public float playerHeight = 2f;                 // Half height used for ground raycast.
    public LayerMask whatIsGround;             // Layers counted as ground.
    public float heightCheckOffset = 0.2f;     // Extra raycast length for ground check.
    [SerializeField] private bool grounded;           // Is the player currently standing on the ground?
    public float CoyoteTime = 0.2f;           // Time window after leaving ground to still jump.
    private bool isRunningCoroutine;                  // Prevents multiple coyote coroutines from running.

    [Header("Orientationifyinin")]
    public Transform orientation;              // Direction reference for movement input.
    private float horizontalInput;                    // Raw input from A/D or left/right keys.
    private float verticalInput;                      // Raw input from W/S or up/down keys.
    private Vector3 moveDirection;                    // Combined movement direction.
    public Rigidbody rb;                             // Player physics body.

    [Header("Tunnel Vision Specific Mechanics")]
    public float zoomMaxMultiplier = 2;          // speed multiplier when fully zoomed in.
    public float zoomMinMultiplier = 0.5f;    // speed multiplier when fully zoomed out.
    public float zoomSensitivity = 0.05f;              // how quickly the speed changes when zooming.
    public float currentZoom = 1f;                    // current zoom level

    [Header("DevMode")]
    public bool DevMode = false;                       // Toggle debugging shortcuts.

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
        if (DevMode)
        {
            if (Input.GetKey(KeyCode.F1))
            {
                print("grounded: " + grounded);                   // Debug: print grounded state.
                print("velocity: " + rb.velocity);                // Debug: print current velocity
                print("currentSpeed: " + currentSpeed);              // Debug: print current speed variable.
                print("currentZoom: " + currentZoom);             // Debug: print current zoom level.
            }
            if (Input.GetKey(KeyCode.F2))
            {
                rb.velocity =  new Vector3(rb.velocity.x, 40f, rb.velocity.z);           // Debug: Fly
            }
        }

        currentZoom += Input.mouseScrollDelta.y * zoomSensitivity;
        currentZoom = Mathf.Clamp(currentZoom, zoomMinMultiplier, zoomMaxMultiplier);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0) // if pressing WASD
        {    
            if (currentSpeed < 1f)
            {
                currentSpeed = 1f; // eases acceleration from 0 to prevent slow start when beginning to move.
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (currentSprintAcceleration < 1f)
                {
                    currentSprintAcceleration = 1f; // Reset acceleration to 1 when below.
                }
                
                if (currentSpeed < maxWalkSpeed * currentSprintAcceleration * currentZoom)
                {       
                    currentSpeed += maxWalkSpeed * currentSprintAcceleration * currentZoom * Time.deltaTime; // increases speed by the accelerated value.
                }
            
                currentSprintAcceleration += sprintAccelerationRate * sprintSpeedMultiplier * Time.deltaTime; // accelerates player while sprinmting.

                if (currentSpeed > maxWalkSpeed * speedCap * currentZoom)
                {
                    currentSpeed = maxWalkSpeed * speedCap * currentZoom; // Cap sprint speed.

                    currentSprintAcceleration = speedCap/maxWalkSpeed; // Cap acceleration to match speed cap.
                }
            }
            else
            {
                if (currentSpeed < maxWalkSpeed * currentZoom)
                {       
                    currentSpeed += maxWalkSpeed * currentZoom * walkAccelerationRate * Time.deltaTime; // Gradually increase speed when walking.
                }

                if (currentSprintAcceleration >= 1f)
                {
                    currentSprintAcceleration -= 0.8f * currentSprintAcceleration * Time.deltaTime; // Slows down player (Reference 1)
                }
                else if (currentSprintAcceleration < 1f)
                {
                    currentSprintAcceleration = 1f; // Prevents slowing to below 1 and slowing base walk speed
                }

                if (currentSpeed > maxWalkSpeed * currentSprintAcceleration * currentZoom)
                {
                    currentSpeed = maxWalkSpeed * currentSprintAcceleration * currentZoom; // Applies slowdown to player from Ref. 1
                }
            }
        }
        else // if not pressing WASD
        {

            if (currentSpeed > 0f)
            {
                currentSpeed -= ( 1.5f * currentSpeed + 2f ) * Time.deltaTime; // slows player when not moving
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
        if (horizontalInput != 0 || verticalInput != 0) // if pressing WASD
        {  
            // Convert input into movement direction relative to orientation.
            moveDirection = verticalInput * orientation.forward + horizontalInput * orientation.right * 0.5f;
        }

        if (grounded)
        {
            // rb.AddForce(moveDirection * currentSpeed * 10f, ForceMode.Force);
            // rb.velocity = moveDirection * currentSpeed * 10f + new Vector3(0f, rb.velocity.y, 0f); // Preserve vertical velocity when grounded.
            rb.velocity += moveDirection * currentSpeed * 10f;
        }
        else
        {
            // rb.AddForce(moveDirection * currentSpeed * 10f * airControl, ForceMode.Force);
            // rb.velocity = moveDirection * currentSpeed * 10f * airControl + new Vector3(0f, rb.velocity.y, 0f); // Preserve vertical velocity when in the air.
            rb.velocity += moveDirection * currentSpeed * 10f * airControl;
        }

        if (flutterJump) 
        {
            // rb.AddForce(transform.up * jumpFloat, ForceMode.Impulse);
            rb.velocity += new Vector3(0f, jumpFloat, 0f); // Apply consistent float boost while holding jump.
        }

        rb.velocity += new Vector3(0f, jumpGravity, 0f);
        
    }

    private void SpeedControl()
    {
        // Only limit horizontal velocity, preserve current vertical velocity.
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentSpeed * speedCap ;
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
        readyToJump = false;                   // Allow jump again after coyote time.
    }
}
