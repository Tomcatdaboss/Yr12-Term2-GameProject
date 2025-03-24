using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipController : MonoBehaviour
{
    public GameObject axe_obj;
    public GameObject pickaxe_obj;
    public GameObject spear_obj;
    public GameObject sickle_obj;
    public string current_obj_selected;
    Animator axe_animator;
    Animator pick_animator;
    Animator spear_animator;
    Animator sickle_animator;
    private bool is_mining = false;
    // Start is called before the first frame update
    void Start()
    {
        axe_animator = axe_obj.GetComponent<Animator>();
        pick_animator = pickaxe_obj.GetComponent<Animator>();
        spear_animator = spear_obj.GetComponent<Animator>();
        sickle_animator = sickle_obj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
            if(Input.GetKey(KeyCode.Mouse0)){
                is_mining = true; 
                pick_animator.SetBool("Mining", is_mining);// sets mining animation on, which triggers "Inventory" script's mining code
            } else{
                is_mining = false; // turns the animation off.
                pick_animator.SetBool("Mining", is_mining);
            }
        }   
        if (current_obj_selected == "Axe"){ // see 'pickaxe' version of this
            spear_obj.SetActive(false);
            axe_obj.SetActive(true);
            pickaxe_obj.SetActive(false);
            sickle_obj.SetActive(false);
            if(Input.GetKey(KeyCode.Mouse0)){
                is_mining = true;
                axe_animator.SetBool("Mining", is_mining);
            } else {
                is_mining = false;
                axe_animator.SetBool("Mining", is_mining);
            }
        }
        if (current_obj_selected == "Spear"){ // see 'pickaxe' version of this
            spear_obj.SetActive(true);
            axe_obj.SetActive(false);
            pickaxe_obj.SetActive(false);
            sickle_obj.SetActive(false);
            if(Input.GetKey(KeyCode.Mouse0)){
                is_mining = true;
                spear_animator.SetBool("Is_Mining", is_mining);
            } else {
                is_mining = false;
                spear_animator.SetBool("Is_Mining", is_mining);
            }
        }
        if (current_obj_selected == "Sickle"){ // see 'pickaxe' version of this
            spear_obj.SetActive(false);
            axe_obj.SetActive(false);
            pickaxe_obj.SetActive(false);
            sickle_obj.SetActive(true);
            if(Input.GetKey(KeyCode.Mouse0)){
                is_mining = true;
                sickle_animator.SetBool("Is_Mining", is_mining);
            } else {
                is_mining = false;
                sickle_animator.SetBool("Is_Mining", is_mining);
            }
        }
    }
}

