using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float jumpForce;
    private float jumpCooldown = 1;
    private float airMultiplier = 0.4f;
    public bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    float horizontalInput;
    float verticalInput;
    private Vector3 moveDirection;
    Rigidbody rb;
    public Hud otherScript;
    public float y_at_jump;
    public float y_after_jump;
    public float rotateSpeed = 100f;
    private float maxFallDistance = -30;
    public Transform respawnPoint;
    Animator animatorp;
    public bool isMoving = false;
    public float velocity;
    private void Start()
    {
        // assigns variables
        rb = GetComponent<Rigidbody>();
        readyToJump = true;
        otherScript = Hud.instance;
        animatorp = gameObject.GetComponent<Animator>();
        y_at_jump = transform.position.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        velocity = rb.velocity.magnitude;
        //gets input & speed
        MyInput();
        SpeedControl();
        StateHandler();
        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        if (moveDirection.magnitude > 0) // handles animations
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        animatorp.SetBool("IsMoving", isMoving);
    }

    private void FixedUpdate() // calls this function not every frame but at a specified time. Inbuilt function used for unity physics calcs
    {
        MovePlayer();
    }

    private void MyInput() // actually gets the input from the player
    {
        //actually getting input from WASD
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // resets jump at a specified time
        }
    }

    private void StateHandler() // checks the various states the player may be in and assigns certain variables to match
    {
        // Sprinting
        if (grounded && Input.GetKey(sprintKey) && otherScript.is_sprinting_bool)
        {
            moveSpeed = sprintSpeed;
            animatorp.speed = 2;
        }
        // Walking
        else if (grounded)
        {
            moveSpeed = walkSpeed;
            animatorp.speed = 1;
        }
        // Falling
        else
        {
            animatorp.speed = 1;
            rb.drag = 0;
        }
    }
    private void MovePlayer() // calculates movement direction depending on whether or not the player is falling, on a slope or just walking
    {
        // calculate movement direction
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    private void Jump() // the function that makes the player jump upwards
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset y velocity
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump() // pretty simple, just resets the jump code so you can jump again
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope() // checks if the player is on slope or not
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection() // changes the force vector to become parallel to the slope
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void OnCollisionEnter(Collision collision) // allows the player to jump again upon hitting the grounds
    {
        if (collision.gameObject.CompareTag("Ground")) // finds difference in y positions after jump and calculates the fall damage
        {
            grounded = true;
            y_after_jump = gameObject.transform.position.y;
            FallDmgCalc(y_at_jump - y_after_jump);
        }
        // note: the following is probably not necessary anymore but I'm keeping it in the code just in case.
        if (collision.gameObject.name == "UnderLandBarrier")
        { // if the player falls through the terrain, tp them back to spawn.
            transform.position = respawnPoint.position;
        }
        if (transform.position.y < maxFallDistance) // if the player goes below the ocean floor tp back to spawn
        {
            transform.position = respawnPoint.position;
        }
    }
    private void OnCollisionExit(Collision collision)
    { // called when the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            y_at_jump = gameObject.transform.position.y; // checks the starting position of the player when they jump
        }
    }
    private void FallDmgCalc(float y_change){ // calculates fall damage
        float resultant_health_loss = 0;
        if (y_change > 10){
            resultant_health_loss = y_change / 2;
        }
        otherScript.GainHealth(-resultant_health_loss);
    }
}
