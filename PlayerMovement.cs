//using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 5;
    public Hud otherScript;
    public bool isOnGround = true;
    private float horizontalInput;
    public float real_speed;
    private float is_sprinting = 1;
    private float forwardInput;
    public float y_at_jump;
    public float y_after_jump;
    private Rigidbody playerRb;
    public float rotateSpeed = 100f;
    public float maxFallDistance = -30;
    public Transform respawnPoint;
    private Vector3 PlayerMovementInput;
    private Vector3 MoveVector;
    Animator animatorp;
    public bool isMoving = false;
    public GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
        otherScript = gameObject.GetComponent<Hud>();
        playerRb = GetComponent<Rigidbody>();
        animatorp = gameObject.GetComponent<Animator>();
        y_at_jump = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
        {
            isMoving = false;
        } else
        {
            isMoving = true;
        }
        // get player input
        real_speed = speed * is_sprinting;

        // player moving around
        
        PlayerMovementInput = new Vector3 (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MoveVector = transform.TransformDirection(PlayerMovementInput) * real_speed;
        playerRb.velocity = new Vector3 (MoveVector.x, playerRb.velocity.y, MoveVector.z);

        //player jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            y_at_jump = gameObject.transform.position.y; // records current y value to calculate fall dmg

        }
        //player sprint
        if (otherScript.is_sprinting_bool == true)
        {
            is_sprinting = 2;
            animatorp.speed = 2;
        }
        if (otherScript.is_sprinting_bool == false)
        {
            is_sprinting = 1;
            animatorp.speed = 1;
        }
        animatorp.SetBool("IsMoving", isMoving);
    }

    private void OnCollisionEnter(Collision collision) // allows the player to jump again upon hitting the grounds
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
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
