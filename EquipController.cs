using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipController : MonoBehaviour
{
    public GameObject axe_obj;
    public GameObject pickaxe_obj;
    public string current_obj_selected;
    Animator axe_animator;
    Animator pick_animator;
    private bool is_mining = false;
    // Start is called before the first frame update
    void Start()
    {
        axe_animator = axe_obj.GetComponent<Animator>();
        pick_animator = pickaxe_obj.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (current_obj_selected == "empty"){
            pickaxe_obj.SetActive(false);
            axe_obj.SetActive(false);
        }
        if (current_obj_selected == "Pickaxe"){
            pickaxe_obj.SetActive(true);
            axe_obj.SetActive(false);
            if(Input.GetKey(KeyCode.Mouse0)){
                is_mining = true;
                pick_animator.SetBool("Mining", is_mining);
            } else{
                is_mining = false;
                pick_animator.SetBool("Mining", is_mining);
            }
        }   
        if (current_obj_selected == "Axe"){
            axe_obj.SetActive(true);
            pickaxe_obj.SetActive(false);
            if(Input.GetKey(KeyCode.Mouse0)){
                is_mining = true;
                axe_animator.SetBool("Mining", is_mining);
            } else {
                is_mining = false;
                axe_animator.SetBool("Mining", is_mining);
            }
        }
    }
}
