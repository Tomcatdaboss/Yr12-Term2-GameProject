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
    private bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
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
    public float maxFallDistance = -30;
    public Transform respawnPoint;
    Animator animatorp;
    public bool isMoving = false;
    public GameObject Camera;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        readyToJump = true;
        otherScript = gameObject.GetComponent<Hud>();
        animatorp = gameObject.GetComponent<Animator>();
        y_at_jump = transform.position.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        if (moveDirection.magnitude > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        animatorp.SetBool("IsMoving", isMoving);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        //actually getting input from WASD
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void StateHandler()
    {
        // Mode - Sprinting
        if (grounded && Input.GetKey(sprintKey) && otherScript.is_sprinting_bool)
        {
            moveSpeed = sprintSpeed;
            animatorp.speed = 2;
        }
        // Mode - Walking
        else if (grounded)
        {
            moveSpeed = walkSpeed;
            animatorp.speed = 1;
        }
        // Mode - Falling
        else
        {
            animatorp.speed = 1;
            rb.drag = 0;
        }
    }
    private void MovePlayer()
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

    private void Jump()
    {
        exitingSlope = true;
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    private void OnCollisionEnter(Collision collision) // allows the player to jump again upon hitting the grounds
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            y_after_jump = gameObject.transform.position.y;
            FallDmgCalc(y_at_jump - y_after_jump);
        }
        if(collision.gameObject.name == "UnderLandBarrier"){ // if the player falls through the terrain, tp them back to spawn.
            transform.position = respawnPoint.position;
        }
        if (transform.position.y < maxFallDistance) // if the player goes below the ocean floor tp back to spawn
        {
            transform.position = respawnPoint.position;
        }
    }
    private void OnCollisionExit(Collision collision){
        if (collision.gameObject.CompareTag("Ground"))
        {
            y_at_jump = gameObject.transform.position.y;
        }
    }
    private void FallDmgCalc(float y_change){
        float resultant_health_loss = 0;
        if (y_change > 10){
            resultant_health_loss = y_change / 2;
        }
        otherScript.GainHealth(-resultant_health_loss);
    }
}
