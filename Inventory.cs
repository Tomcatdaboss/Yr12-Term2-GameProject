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
    public GameObject boat_obj;
    // Start is called before the first frame update
    void Start()
    {   
    }
    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Alpha1)){ // if the 1 key is pressed, select the object in equip slot 1.
        GameObject obj_in_slot = EquipSlots[0];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
      if(Input.GetKeyDown(KeyCode.Alpha2)){ // if the 2 key is pressed, select the object in equip slot 2.
        GameObject obj_in_slot = EquipSlots[1];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
      if(Input.GetKeyDown(KeyCode.Alpha3)){ // if the 3 key is pressed, select the object in equip slot 3.
        GameObject obj_in_slot = EquipSlots[2];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
      if(Input.GetKeyDown(KeyCode.Alpha4)){ // if the 4 key is pressed, select the object in equip slot 4.
        GameObject obj_in_slot = EquipSlots[3];
        equip_controller.GetComponent<EquipController>().current_obj_selected = obj_in_slot.GetComponent<Slot>().item_name;
      }
    }
    public void AddToList(string add_candidate){ // function to add a particular item to the list of crafting requirements to prepare for the execution of RunCraftingFunc()
      CraftingList.Add(add_candidate);
    }
    public void Run_Crafting_Func(string output_name){ // takes all the required items as input and outputs an item to theinventory or equip slots
      bool enough_material = true;
      foreach(var x in CraftingList){ // checks that all items in the crafting list are present in the inventory
        if (FindSlot(x, InventSlots, true).GetComponent<Slot>().quantity - 1 >= 0){
          InsertSlot(x, -1, true);
        }
        else {
          enough_material = false;
        }   
      }
      CraftingList.Clear(); // wipes crafting list
      if (enough_material){
        if(output_name == "Campfire") // checks that the item isnt a object or 'construct' like the campfire or boat.
        {
          Debug.Log("Campfire");
        }
        else if (output_name == "Ship"){
          Debug.Log("ship");
        } else if (output_name != "Axe" && output_name != "Spear" && output_name != "Pickaxe" && output_name != "Sickle"){ // if the output is an equipable item, sends it to the equip slots. If not, sends it to the inventory.
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
    public GameObject FindSlot(string given_name, List<GameObject> TypeSlots, bool adding) // this is an iteration function used to find items in the inventory, suitable slots to place items in, etc.
    {
      if (TypeSlots == InventSlots){ 
          int i = 0;
          while(i < InventSlots.Count){ // goes through all the different inventory slots for one that has the name of the given item, then checks for empty slots
            if (InventSlots[i].GetComponent<Slot>().item_name == given_name)
            {
              return InventSlots[i];
            } 
            else{
              i += 1;
            }
          }
          i = 0;
          while(i < InventSlots.Count){
            if (InventSlots[i].GetComponent<Slot>().item_name == "empty" && adding == true)
            {
              return InventSlots[i];
            }
            else{
              i += 1;
            }
          }
          return null;
      }else if (TypeSlots == EquipSlots){ // goes through all the different equip slots for one that is empty
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
    public void InsertSlot(string new_item_name, int quantity, bool isInvent) // this function takes the output of FindSlot and places an item inside the given slot.
    {
      if(isInvent){ // checks which list to search - whether the item is equippable or not.
        used_slot = FindSlot(new_item_name, InventSlots, true);
      }else{
        used_slot = FindSlot(new_item_name, EquipSlots, true);
        Debug.Log("EquipPathTaken");
      }
       if (used_slot == null){ // if no slot is found by the FindSlot function, do nothing
       } else { // place the item in the given slot.
        used_slot.GetComponent<Slot>().quantity += quantity;
        used_slot.GetComponent<Slot>().item_name = new_item_name;
       }
    }
    private void OnTriggerStay(Collider other)
    {
      if(other.gameObject.layer == 4){ // if the player comes in contact with water, they get their thirst bar refilled.
        gameObject.GetComponent<Hud>().thirst = 100;
      }
      if (equip_controller.GetComponent<EquipController>().current_obj_selected == "Pickaxe"){
        if(other.gameObject.layer == 8 && equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Pickaxe_mining_anim") && already_mined == false){
            InsertSlot("Stone", 1, true);
            gameObject.GetComponent<Hud>().xp += 6;
            already_mined = true;
        }
        if(other.gameObject.layer == 9 && equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Pickaxe_mining_anim") && already_mined == false){
            InsertSlot("Ore", 1, true);
            gameObject.GetComponent<Hud>().xp += 6;
            already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().pickaxe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        }
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Axe"){
        if(other.gameObject.layer == 7 && equip_controller.GetComponent<EquipController>().axe_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AxeMining_Anim") && already_mined == false){
          InsertSlot("Wood", 1, true);
          gameObject.GetComponent<Hud>().xp += 6;
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
          gameObject.GetComponent<Hud>().xp += 6;
          already_mined = true;
        }
        if(other.gameObject.layer == 11 && equip_controller.GetComponent<EquipController>().spear_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spearing_Anim") && already_mined == false){
          other.gameObject.GetComponentInParent<EnemyMovement>().health -= 10;
          gameObject.GetComponent<Hud>().xp += 9;
          already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().spear_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        } 
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Sickle"){
        if(other.gameObject.layer == 12 && equip_controller.GetComponent<EquipController>().sickle_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SickleMining_Anim") && already_mined == false){
          InsertSlot("Fiber", 1, true);
          gameObject.GetComponent<Hud>().xp += 6;
          already_mined = true;
        }
        if(other.gameObject.layer == 10 && equip_controller.GetComponent<EquipController>().sickle_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SickleMining_Anim") && already_mined == false){
          InsertSlot("Hide", 1, true);
          gameObject.GetComponent<Hud>().xp += 6;
          already_mined = true;
        }
        if(equip_controller.GetComponent<EquipController>().sickle_obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle")){
          already_mined = false;
        } 
      }
    }
    public void StartSpawnProcess(GameObject itemPrefab){ // this function is for spawning constructs - it defines the position that the construct will spawn at, then triggers the next part of the function. This is because Unity buttons only accept functions with one paramater.
      Vector3 playerposition = new Vector3(gameObject.transform.position.x + 2, gameObject.transform.position.y + 2, gameObject.transform.position.z + 2);
      SpawnConstruct(itemPrefab, playerposition);
    }
    public void SpawnConstruct(GameObject itemPrefab, Vector3 playerposition) // for spawning constructs - takes the position, spawns it, and adds it to the list of constructs in the PlayerDataManager.
    {
        if (itemPrefab.tag != "Boat") {
            GameObject construct = Instantiate(itemPrefab, playerposition, Quaternion.identity);
            SaveManager.GetComponent<PlayerDataManager>().ConstructList.Add(construct);
            if(construct.tag == "Campfire"){ // initialises the variables for the campire script of the construct.
              construct.GetComponent<CampfireController>().UI = campUI;
              construct.GetComponent<CampfireController>().button = campbutton;
              construct.GetComponent<CampfireController>().player = gameObject;
            }
            Debug.Log("Spawned");
        } else if (itemPrefab.tag == "Boat"){ // activates the boat object and saves the game - you dont want to lose this milestone to a save issue!
            boat_obj.SetActive(true);
            SaveManager.GetComponent<PlayerDataManager>().SaveGame();
        }
    }
    
}
