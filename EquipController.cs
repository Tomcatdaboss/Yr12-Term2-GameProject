using UnityEngine;

public class EquipController : MonoBehaviour
{
    public GameObject axe_obj;
    public GameObject pickaxe_obj;
    public GameObject spear_obj;
    public GameObject sickle_obj;
    public GameObject player;
    public string current_obj_selected;
    Animator player_animator;
    private bool is_mining = false;
    private bool can_mine = true;
    // Start is called before the first frame update
    void Start()
    {
        player_animator = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Hud.instance.inventory_sprite.activeSelf != true && Hud.instance.help_sprite.activeSelf != true && Hud.instance.menu_button_UI.activeSelf != true && Hud.instance.menu_sprite.activeSelf != true)
        {
            can_mine = true;
        } else {
            can_mine = false;
        }
        // the following if statements handle the switching of active equip objects, and triggers the mining animation and variable that triggers resource gathering.
        // the code is essentially the same for each of the four tools, so I will only detail the first one.
        if (current_obj_selected == "empty"){ // sets all tools to deactivated.
            pickaxe_obj.SetActive(false);
            axe_obj.SetActive(false);
            spear_obj.SetActive(false);
            sickle_obj.SetActive(false);
        }
        if (current_obj_selected == "Pickaxe"){
            spear_obj.SetActive(false); // sets all tools but pickaxe to deactivated
            pickaxe_obj.SetActive(true);
            axe_obj.SetActive(false);
            sickle_obj.SetActive(false);
            if(Input.GetKey(KeyCode.Mouse0) && can_mine){
                is_mining = true; // sets mining animation on, which triggers "Inventory" script's mining code
                player_animator.SetFloat("CurrentObj", 2);
            } else{
                is_mining = false; // turns the animation off.
            }
        }   
        if (current_obj_selected == "Axe"){ // see 'pickaxe' version of this
            spear_obj.SetActive(false);
            axe_obj.SetActive(true);
            pickaxe_obj.SetActive(false);
            sickle_obj.SetActive(false);
            if(Input.GetKey(KeyCode.Mouse0) && can_mine){
                is_mining = true;
                player_animator.SetFloat("CurrentObj", 2);
            } else {
                is_mining = false;
            }
        }
        if (current_obj_selected == "Spear"){ // see 'pickaxe' version of this
            spear_obj.SetActive(true);
            axe_obj.SetActive(false);
            pickaxe_obj.SetActive(false);
            sickle_obj.SetActive(false);
            if(Input.GetKey(KeyCode.Mouse0) && can_mine){
                is_mining = true;
                player_animator.SetFloat("CurrentObj", 3);
            } else {
                is_mining = false;
            }
        }
        if (current_obj_selected == "Sickle"){ // see 'pickaxe' version of this
            spear_obj.SetActive(false);
            axe_obj.SetActive(false);
            pickaxe_obj.SetActive(false);
            sickle_obj.SetActive(true);
            if(Input.GetKey(KeyCode.Mouse0) && can_mine){
                is_mining = true;
                player_animator.SetFloat("CurrentObj", 1);
            } else {
                is_mining = false;
            }
        }
        player_animator.SetBool("Mining", is_mining);
    }
}

