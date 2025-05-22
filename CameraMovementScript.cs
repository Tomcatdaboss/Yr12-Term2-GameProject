using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 2f;
    public bool isMoving = false;
    public GameObject inventory_sprite;
    public GameObject menusprite;
    public GameObject help_obj;
    public GameObject menubtnsprite;
    public GameObject campfireUI;
    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // the if statement checks if inventory or the help screen is active, then if no, locks the cursor.
        if (inventory_sprite.activeSelf == false && help_obj.activeSelf == false && menusprite.activeSelf == false && menubtnsprite.activeSelf == false && campfireUI.activeSelf == false)
        { // testing that they are not in inventory
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Collect Mouse Input

            float inputX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
            float inputY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

            yRotation += inputX;
            xRotation -= inputY;
            xRotation = Mathf.Clamp(xRotation, -90f, 50f);
            player.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
