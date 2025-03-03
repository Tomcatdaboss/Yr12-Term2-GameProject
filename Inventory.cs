using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool already_mined = false;
    public GameObject SaveManager;
    public List<GameObject> InventSlots = new List<GameObject>();
    public List<GameObject> EquipSlots = new List<GameObject>();
    public List<string> CraftingList = new List<string>();
    private GameObject used_slot = null;
    public GameObject equip_controller;
    public GameObject campUI;
    public GameObject campbutton;
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
    }
    public void AddToList(string add_candidate){
      CraftingList.Add(add_candidate);
    }
    public void Run_Crafting_Func(string output_name){
      bool enough_material = true;
      foreach(var x in CraftingList){
        if (FindSlot(x, InventSlots, true).GetComponent<Slot>().quantity - 1 >= 0){
          InsertSlot(x, -1, true);
        }
        else {
          enough_material = false;
        }   
      }
      CraftingList.Clear();
      if (enough_material){
        if(output_name == "Campfire")
        {
          Debug.Log("Campfire");
        } else if (output_name != "Axe" && output_name != "Spear" && output_name != "Pickaxe" && output_name != "Sickle"){
          InsertSlot(output_name, 1, true);
          Debug.Log(output_name);
        } else{
          InsertSlot(output_name, 1, false);
          Debug.Log("EquipSlots");
        }
      }
      else{
        Debug.Log(enough_material);
      }
    }
    public GameObject FindSlot(string given_name, List<GameObject> TypeSlots, bool adding)
    {
      if (TypeSlots == InventSlots){
          int i = 0;
          while(i < InventSlots.Count){
            if (InventSlots[i].GetComponent<Slot>().item_name == given_name)
            {
              return InventSlots[i];
            } 
            else if (InventSlots[i].GetComponent<Slot>().item_name == "empty" && adding == true)
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
        used_slot = FindSlot(new_item_name, InventSlots, true);
      }else{
        used_slot = FindSlot(new_item_name, EquipSlots, true);
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
      if(other.gameObject.layer == 4){
        gameObject.GetComponent<Hud>().thirst = 100;
      }
      if (equip_controller.GetComponent<EquipController>().current_obj_selected == "Pickaxe"){
        if(other.gameObject.layer == 8 && equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Pickaxe_mining_anim") && already_mined == false){
            InsertSlot("Stone", 1, true);
            gameObject.GetComponent<Hud>().xp += 2;
            already_mined = true;
        }
        if(other.gameObject.layer == 9 && equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Pickaxe_mining_anim") && already_mined == false){
            InsertSlot("Ore", 1, true);
            gameObject.GetComponent<Hud>().xp += 2;
            already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        }
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Axe"){
        if(other.gameObject.layer == 7 && equip_controller.GetComponent<EquipController>().axe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AxeMining_Anim") && already_mined == false){
          InsertSlot("Wood", 1, true);
          gameObject.GetComponent<Hud>().xp += 2;
          already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().axe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        } 
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Spear"){
        Debug.Log(other.gameObject.layer);
        if(other.gameObject.layer == 10 && equip_controller.GetComponent<EquipController>().spear_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spearing_Anim") && already_mined == false){
          InsertSlot("Raw Meat", 1, true);
          gameObject.GetComponent<Hud>().xp += 2;
          already_mined = true;
        }
        if(other.gameObject.layer == 11 && equip_controller.GetComponent<EquipController>().spear_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spearing_Anim") && already_mined == false){
          other.gameObject.GetComponentInParent<EnemyMovement>().health -= 10;
          gameObject.GetComponent<Hud>().xp += 3;
          already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().spear_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        } 
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Sickle"){
        if(other.gameObject.layer == 12 && equip_controller.GetComponent<EquipController>().sickle_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SickleMining_Anim") && already_mined == false){
          InsertSlot("Fiber", 1, true);
          gameObject.GetComponent<Hud>().xp += 2;
          already_mined = true;
        }
        if(other.gameObject.layer == 10 && equip_controller.GetComponent<EquipController>().sickle_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SickleMining_Anim") && already_mined == false){
          InsertSlot("Hide", 1, true);
          gameObject.GetComponent<Hud>().xp += 2;
          already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().sickle_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        } 
      }
    }
    public void StartSpawnProcess(GameObject itemPrefab){
      Vector3 playerposition = new Vector3(gameObject.transform.position.x + 2, gameObject.transform.position.y + 2, gameObject.transform.position.z + 2);
      SpawnConstruct(itemPrefab, playerposition);
    }
    public void SpawnConstruct(GameObject itemPrefab, Vector3 playerposition) // for spawning constructs - unfinished!
    {
        if (itemPrefab.name != "Boat_Prefab") {
            GameObject construct = Instantiate(itemPrefab, playerposition, Quaternion.identity);
            SaveManager.GetComponent<PlayerDataManager>().ConstructList.Add(construct);
            if(construct.tag == "Campfire"){
              construct.GetComponent<CampfireController>().UI = campUI;
              construct.GetComponent<CampfireController>().button = campbutton;
              construct.GetComponent<CampfireController>().player = gameObject;
            }
            Debug.Log("Spawned");
        } else if (itemPrefab.name == "Boat_Prefab" && gameObject.GetComponent<Hud>().thirst >= 99.5){
            Instantiate(itemPrefab, playerposition, Quaternion.identity);
        } else {
            Debug.Log("You can't construct the ship on land! Stand in the water.");
        }
    }
    
}
