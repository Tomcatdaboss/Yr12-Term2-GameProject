using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

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
    private Rigidbody playerRb;
    public float rotateSpeed = 100f;
    public float maxFallDistance = -30;
    public Transform respawnPoint;
    Animator animator;
    private bool isMoving = false;
    public GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
        otherScript = gameObject.GetComponent<Hud>();
        playerRb = GetComponent<Rigidbody>();
        animator = Camera.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsMoving", isMoving);
        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
        {
            isMoving = false;
        } else
        {
            isMoving = true;
        }
        if (isOnGround == true)
        {
            // get player input
            horizontalInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");
            real_speed = speed * is_sprinting;
        }

        // player moving around
        transform.Translate(UnityEngine.Vector3.forward * Time.deltaTime * real_speed * forwardInput);
        transform.Translate(UnityEngine.Vector3.right * Time.deltaTime * speed * horizontalInput);
        
        // this prevents the character from clipping through the terrain
        int naturalYDistance = 1;
        float playerPositionCalculatedY = transform.position.y - Terrain.activeTerrain.SampleHeight(this.transform.position);
        if (playerPositionCalculatedY < naturalYDistance)
        {
            float pushHeight = 1 - playerPositionCalculatedY;
            transform.position += new UnityEngine.Vector3(0, pushHeight, 0);
        }

        //player jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;

        }
        if (transform.position.y < maxFallDistance) // if the player goes below the ocean floor tp back to spawn
        {
            transform.position = respawnPoint.position;
        }
        //player sprint
        if (otherScript.is_sprinting_bool == true)
        {
            is_sprinting = 2;
        }
        if (otherScript.is_sprinting_bool == false)
        {
            is_sprinting = 1;
        }
        animator.SetBool("IsMoving", isMoving);
    }

    private void OnCollisionEnter(Collision collision) // allows the player to jump again upon hitting the grounds
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        if(collision.gameObject.name == "UnderLandBarrier"){ // if the player falls through the terrain, tp them back to spawn.
            transform.position = respawnPoint.position;
        }
    }
}
