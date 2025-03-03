using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public GameObject item_info;
    public GameObject item_icon;
    public Sprite axe_icon_sprite;
    public Sprite pickaxe_icon_sprite;
    public Sprite spear_icon_sprite;
    public Sprite sickle_icon_sprite;
    public Sprite wood_icon_sprite;
    public Sprite stone_icon_sprite;
    public Sprite rawmeat_icon_sprite;
    public Sprite ore_icon_sprite;
    public Sprite fibre_icon_sprite;
    public Sprite cookedmeat_icon_sprite;
    public Sprite rope_icon_sprite;
    public Sprite null_sprite;
    public int quantity = 0;
    public string item_name = "empty";
    public bool isInvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (item_name == "empty"){
            item_info.GetComponent<Text>().text = "Empty Slot";
            item_icon.GetComponent<Image>().sprite = null_sprite;
        } else {
            if (isInvent){
                item_info.GetComponent<Text>().text = item_name + " X " + quantity;
            } else {
                item_info.GetComponent<Text>().text = item_name;
            }            
            if(item_name == "Wood"){
                item_icon.GetComponent<Image>().sprite = wood_icon_sprite;
            }
            if(item_name == "Stone"){
                item_icon.GetComponent<Image>().sprite = stone_icon_sprite;
            }
            if(item_name == "Raw Meat"){
                item_icon.GetComponent<Image>().sprite = rawmeat_icon_sprite;
            }
            if(item_name == "Ore"){
                item_icon.GetComponent<Image>().sprite = ore_icon_sprite;
            }
            if(item_name == "Cooked Meat"){
                item_icon.GetComponent<Image>().sprite = cookedmeat_icon_sprite;
            }
            if(item_name == "Fiber"){
                item_icon.GetComponent<Image>().sprite = fibre_icon_sprite;
            }
            if(item_name == "Rope"){
                item_icon.GetComponent<Image>().sprite = rope_icon_sprite;
            }
            if(item_name == "Axe"){
                item_icon.GetComponent<Image>().sprite = axe_icon_sprite;
            }
            if(item_name == "Pickaxe"){
                item_icon.GetComponent<Image>().sprite = pickaxe_icon_sprite;
            }
            if(item_name == "Spear"){
                item_icon.GetComponent<Image>().sprite = spear_icon_sprite;
            }
            if(item_name == "Sickle"){
                item_icon.GetComponent<Image>().sprite = sickle_icon_sprite;
            }
        }
        if (quantity == 0){
            item_name = "empty";
            item_info.GetComponent<Text>().text = "Empty Slot";
            item_icon.GetComponent<Image>().sprite = null_sprite;
        }
    }
}
