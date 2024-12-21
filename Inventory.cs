using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int wood = 0;
    public int stone = 0;
    private bool already_mined = false;
    public List<GameObject> InventSlots = new List<GameObject>();
    public List<GameObject> EquipSlots = new List<GameObject>();
    public List<string> CraftingList = new List<string>();
    private GameObject used_slot = null;
    public GameObject equip_controller;
    // Start is called before the first frame update
    void Start()
    {   
    }
    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Alpha1)){
        GameObject obj_in_slot = EquipSlots[0];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
      if(Input.GetKeyDown(KeyCode.Alpha2)){
        GameObject obj_in_slot = EquipSlots[1];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
      if(Input.GetKeyDown(KeyCode.Alpha3)){
        GameObject obj_in_slot = EquipSlots[2];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
      if(Input.GetKeyDown(KeyCode.Alpha4)){
        GameObject obj_in_slot = EquipSlots[3];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
      wood = FindSlot("Wood", InventSlots).GetComponent<Slot>().quantity;
      stone = FindSlot("Stone", InventSlots).GetComponent<Slot>().quantity;
    }
    public void AddToList(string add_candidate){
      CraftingList.Add(add_candidate);
    }
    public void Run_Crafting_Func(string output_name){
      bool enough_material = true;
      foreach(var x in CraftingList){
        if (FindSlot(x, InventSlots).GetComponent<Slot>().quantity - 1 >= 0){
          InsertSlot(x, -1, true);
        }
        else {
          enough_material = false;
        }   
      }
      CraftingList.Clear();
      if (enough_material){
        if (output_name != "Axe" && output_name != "Spear" && output_name != "Pickaxe"){
          InsertSlot(output_name, 1, true);
          Debug.Log(output_name);
        } 
        else{
          InsertSlot(output_name, 1, false);
          Debug.Log("EquipSlots");
        }
      }
      else{
        Debug.Log(enough_material);
      }
    }
    public GameObject FindSlot(string given_name, List<GameObject> TypeSlots)
    {
      if (TypeSlots == InventSlots){
          int i = 0;
          while(i < InventSlots.Count){
            if (InventSlots[i].GetComponent<Slot>().item_name == given_name)
            {
              return InventSlots[i];
            } 
            else if (InventSlots[i].GetComponent<Slot>().item_name == "empty")
            {
              return InventSlots[i];
            }
            else{
              i += 1;
            }
          }
          return null;
      }else if (TypeSlots == EquipSlots){
          Debug.Log("Equip Path in FindSlot");     
          int i = 0;
          while (i < EquipSlots.Count){
            if (EquipSlots[i].GetComponent<Slot>().item_name == "empty")
            {
              return EquipSlots[i];
            }
            else{
              i += 1;
            }
          }
          return null;
      } else {
        return null;
      }
    }
    public void InsertSlot(string new_item_name, int quantity, bool isInvent)
    {
      if(isInvent){
        used_slot = FindSlot(new_item_name, InventSlots);
      }else{
        used_slot = FindSlot(new_item_name, EquipSlots);
        Debug.Log("EquipPathTaken");
      }
       if (used_slot == null){
       } else {
        used_slot.GetComponent<Slot>().quantity += quantity;
        used_slot.GetComponent<Slot>().item_name = new_item_name;
       }
    }
    private void OnTriggerStay(Collider other)
    {
        if (equip_controller.GetComponent<EquipController>().current_obj_selected == "Pickaxe"){
        if(other.gameObject.layer == 8 && equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Pickaxe_mining_anim") && already_mined == false){
            InsertSlot("Stone", 1, true);
            already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        }
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Axe"){
        if(other.gameObject.layer == 7 && equip_controller.GetComponent<EquipController>().axe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AxeMining_Anim") && already_mined == false){
          InsertSlot("Wood", 1, true);
          already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().axe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        } 
      }
    }
    
}