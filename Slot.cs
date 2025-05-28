using System.Collections.Generic;
using UnityEditor;
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
    public Sprite hide_icon_sprite;
    public Sprite net_icon_sprite;
    public Sprite hull_icon_sprite;
    public Sprite flag_icon_sprite;
    public Sprite compass_icon_sprite;
    public Sprite sail_icon_sprite;
    public Sprite null_sprite;
    public int quantity = 0;
    public string item_name = "empty";
    public bool isInvent;
    public GameObject nextSlotup;
    // Update is called once per frame
    void Update()
    {
        if (nextSlotup.GetComponent<Slot>().quantity == 0 && quantity != 0) // if the slot before it in the list is empty, this slot transfers its contents to the slot before it.
        {
            nextSlotup.GetComponent<Slot>().quantity = quantity;
            nextSlotup.GetComponent<Slot>().item_name = item_name;
            quantity = 0;
        }
        if (item_name == "empty")
        { // if there is no item in the inventory, then call it an Empty slot
            item_info.GetComponent<Text>().text = "Empty Slot";
            item_icon.GetComponent<Image>().sprite = null_sprite;
        }
        else if (quantity == 0)
        {
            item_name = "empty";
            item_info.GetComponent<Text>().text = "Empty Slot";
            item_icon.GetComponent<Image>().sprite = null_sprite;
        }
        else
        {
            if (isInvent)
            { // defines correct method of stating the item name and quantity depending on whether the slot is an equip or invent slot
                item_info.GetComponent<Text>().text = item_name + " X " + quantity;
            }
            else
            {
                item_info.GetComponent<Text>().text = item_name;
            }
            if (item_name == "Wood")
            { // essentially all these if statements do is check if the item in the slot is a specific type of item, and if so, change the item icon to the correct sprite.
                item_icon.GetComponent<Image>().sprite = wood_icon_sprite;
            }
            if (item_name == "Stone")
            {
                item_icon.GetComponent<Image>().sprite = stone_icon_sprite;
            }
            if (item_name == "Raw Meat")
            {
                item_icon.GetComponent<Image>().sprite = rawmeat_icon_sprite;
            }
            if (item_name == "Ore")
            {
                item_icon.GetComponent<Image>().sprite = ore_icon_sprite;
            }
            if (item_name == "Cooked Meat")
            {
                item_icon.GetComponent<Image>().sprite = cookedmeat_icon_sprite;
            }
            if (item_name == "Fiber")
            {
                item_icon.GetComponent<Image>().sprite = fibre_icon_sprite;
            }
            if (item_name == "Rope")
            {
                item_icon.GetComponent<Image>().sprite = rope_icon_sprite;
            }
            if (item_name == "Hide")
            {
                item_icon.GetComponent<Image>().sprite = hide_icon_sprite;
            }
            if (item_name == "Axe")
            {
                item_icon.GetComponent<Image>().sprite = axe_icon_sprite;
            }
            if (item_name == "Pickaxe")
            {
                item_icon.GetComponent<Image>().sprite = pickaxe_icon_sprite;
            }
            if (item_name == "Spear")
            {
                item_icon.GetComponent<Image>().sprite = spear_icon_sprite;
            }
            if (item_name == "Sickle")
            {
                item_icon.GetComponent<Image>().sprite = sickle_icon_sprite;
            }
            if (item_name == "Net")
            {
                item_icon.GetComponent<Image>().sprite = net_icon_sprite;
            }
            if (item_name == "Hull")
            {
                item_icon.GetComponent<Image>().sprite = hull_icon_sprite;
            }
            if (item_name == "Sail")
            {
                item_icon.GetComponent<Image>().sprite = sail_icon_sprite;
            }
            if (item_name == "Flag")
            {
                item_icon.GetComponent<Image>().sprite = flag_icon_sprite;
            }
            if (item_name == "Compass")
            {
                item_icon.GetComponent<Image>().sprite = compass_icon_sprite;
            }
        }
    }
}

