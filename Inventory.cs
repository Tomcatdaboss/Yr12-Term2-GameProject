using System.Collections.Generic;
using System.Linq;
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
    public float player_dmg;
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
        try{
          int numofthing = 0;
          int i = 0;
          while (i < CraftingList.Count){
            if(CraftingList[i] == x){
              numofthing += 1;
            }
            i += 1;
          }
          if (FindSlot(x, InventSlots, false).GetComponent<Slot>().quantity - (1 * numofthing) < 0){
            enough_material = false;
          }   
        } catch {
          enough_material = false;
        }
      }
      if(enough_material){
        foreach(var x in CraftingList){ // checks that all items in the crafting list are present in the inventory
          InsertSlot(x, -1, true);
        }
      }
      
      CraftingList.Clear(); // wipes crafting list
      if (enough_material){
        SoundManager.instance.PlaySound(SoundManager.instance.craft, SoundManager.instance.volume);
        if(output_name == "Campfire") // checks that the item isnt a object or 'construct' like the campfire or boat.
        {
        }
        else if (output_name == "Ship"){
        } else if (output_name != "Axe" && output_name != "Spear" && output_name != "Pickaxe" && output_name != "Sickle"){ // if the output is an equipable item, sends it to the equip slots. If not, sends it to the inventory.
          InsertSlot(output_name, 1, true);
        } else if (FindSlot(output_name, EquipSlots, false) == null){ // makes sure that you can't craft two of the same tool
          InsertSlot(output_name, 1, false);
        }
      }
      else{
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
        if(other.gameObject.layer == 8 && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PlayerMinePickorAx") && already_mined == false){
            InsertSlot("Stone", 1, true);
            gameObject.GetComponent<Hud>().xp += 6;
            SoundManager.instance.PlaySound(SoundManager.instance.woodnstonemine, SoundManager.instance.volume);
            already_mined = true;
        }
        if(other.gameObject.layer == 9 && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PlayerMinePickorAx") && already_mined == false){
            InsertSlot("Ore", 1, true);
            gameObject.GetComponent<Hud>().xp += 6;
            SoundManager.instance.PlaySound(SoundManager.instance.woodnstonemine, SoundManager.instance.volume);
            already_mined = true;
        }
        if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("Idle")){
          already_mined = false;
        }
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Axe"){
        if(other.gameObject.layer == 7 && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PlayerMinePickorAx") && already_mined == false){
          InsertSlot("Wood", 1, true);
          SoundManager.instance.PlaySound(SoundManager.instance.woodnstonemine, SoundManager.instance.volume);
          gameObject.GetComponent<Hud>().xp += 6;
          already_mined = true;
        }
        if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("Idle")){
          already_mined = false;
        } 
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Spear"){
        if(other.gameObject.layer == 10 && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PlayerMineSpear") && already_mined == false){
          InsertSlot("Raw Meat", 1, true);
          gameObject.GetComponent<Hud>().xp += 6;
          already_mined = true;
        }
        if(other.gameObject.layer == 11 && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PlayerMineSpear") && already_mined == false){
          other.gameObject.GetComponentInParent<EnemyMovement>().health -= player_dmg;
          already_mined = true;
        }
        if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("Idle")){
          already_mined = false;
        } 
      }
      if(equip_controller.GetComponent<EquipController>().current_obj_selected == "Sickle"){
        if(other.gameObject.layer == 12 && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PlayerSickleMine") && already_mined == false){
          InsertSlot("Fiber", 1, true);
          gameObject.GetComponent<Hud>().xp += 6;
          SoundManager.instance.PlaySound(SoundManager.instance.fibermine, SoundManager.instance.volume);
          already_mined = true;
        }
        if(other.gameObject.layer == 10 && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("PlayerSickleMine") && already_mined == false){
          InsertSlot("Hide", 1, true);
          gameObject.GetComponent<Hud>().xp += 6;
          already_mined = true;
        }
        if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("Idle")){
          already_mined = false;
        } 
      }
    }
    public void StartSpawnProcess(GameObject itemPrefab){ // this function is for spawning constructs - it defines the position that the construct will spawn at, then triggers the next part of the function. This is because Unity buttons only accept functions with one paramater.
      UnityEngine.Vector3 playerposition = new UnityEngine.Vector3(gameObject.transform.position.x + 2, gameObject.transform.position.y + 2, gameObject.transform.position.z + 2);
      SpawnConstruct(itemPrefab, playerposition);
    }
    public void SpawnConstruct(GameObject itemPrefab, UnityEngine.Vector3 playerposition) // for spawning constructs - takes the position, spawns it, and adds it to the list of constructs in the PlayerDataManager.
    {
        if (itemPrefab.tag != "Boat") {
            GameObject construct = Instantiate(itemPrefab, playerposition, UnityEngine.Quaternion.identity);
            SaveManager.GetComponent<PlayerDataManager>().ConstructList.Add(construct);
            if(construct.tag == "Campfire"){ // initialises the variables for the campire script of the construct.
              construct.GetComponent<CampfireController>().UI = campUI;
              construct.GetComponent<CampfireController>().button = campbutton;
              construct.GetComponent<CampfireController>().player = gameObject;
            }
        } else if (itemPrefab.tag == "Boat"){ // activates the boat object and saves the game - you dont want to lose this milestone to a save issue!
            boat_obj.GetComponent<Transform>().localScale = new UnityEngine.Vector3((float)0.7, (float)0.7, (float)0.7);
            SaveManager.GetComponent<PlayerDataManager>().SaveGame();
        }
    }
    
}
