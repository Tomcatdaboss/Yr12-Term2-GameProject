using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    public GameObject item_info;
    public GameObject item_icon;
    public Sprite wood_icon_sprite;
    public Sprite stone_icon_sprite;
    public Sprite fiber_icon_sprite;
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
            if(item_name == "Fiber"){
                item_icon.GetComponent<Image>().sprite = fiber_icon_sprite;
            }
        }
        if (quantity == 0){
            item_name = "empty";
            item_info.GetComponent<Text>().text = "Empty Slot";
            item_icon.GetComponent<Image>().sprite = null_sprite;
        }
    }
}
