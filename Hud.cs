using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject XP_text;
    public GameObject DeathScene;
    public GameObject savemanager;
    Animator animator;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float maxHunger = 100f;
    public float maxThirst = 100f;
    public float maxXP = 100f;
    public float stamina;
    public float health;
    public float hunger;
    public float thirst;
    public Transform DeathSceneTransform;
    public Transform RespawnPointTransform;
    public bool is_sprinting_bool = false;
    public float xp = 0f;
    private float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Image xpBarFiller;
    public Image staminaBarFiller;
    public Image hungerBarFiller;
    public Image thirstBarFiller;
    public GameObject inventory_sprite;
    public GameObject crafting_sprite;
    public GameObject help_sprite;
    public float XP_level;
    private Text xp_txt;
    private float lasthealth = 100;
    public float is_winded_start_time = 0;
    public float is_winded_end_time = 0;
    public float is_winded_time_elapsed = 0;
    public float is_hurt_start_time = 0;
    public float is_hurt_end_time = 0;
    public float is_hurt_time_elapsed = 0;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
        stamina = maxStamina;
        xp_txt = XP_text.GetComponent<Text>();
        XP_level = 0;
        UpdateStatsUI();
        inventory_sprite.SetActive(false);
        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        hunger = Mathf.Clamp(hunger, 0, maxHunger);
        thirst = Mathf.Clamp(thirst, 0, maxThirst);
    }

    // Update is called once per frame
    void Update()
    {
        is_winded_end_time = Time.time;
        is_hurt_end_time = Time.time; 

        xp_txt.text = XP_level.ToString();

        UpdateStatsUI(); // constantly triggers the check to change the UI to reflect current stat levels

        LoseHunger(0.001f);// loses hunger and thirst every frame.
        LoseThirst(0.001f);
        if (hunger <= 1){
            GainHealth(-0.01f);
        }else if (thirst <= 1){
            GainHealth(-0.01f);
        }

        if (Input.GetKeyDown(KeyCode.M)) // these if statements are testing functions to artificially trigger stat change
        {
            GainHealth(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            GainHealth(Random.Range(-5, -10));
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GainXp(Random.Range(5, 10));
        }

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0.5) // sprinting code. 
        {
            is_sprinting_bool = true; // triggers faster movement
            LoseStamina(0.05f); // stat change
        }
        if (stamina >= 30) // these two if statements handle UI change in color of stamina bar based on stamina levels
        {
            staminaBarFiller.color = Color.white;
        }
        if (stamina <= 30)
        {
            staminaBarFiller.color = Color.red;
        }
        if (stamina <= 0.5) // turns off sprint if stamina is low.
        {
            is_sprinting_bool = false;
        }

        if (is_winded_start_time != 0)
        {
            is_winded_time_elapsed = is_winded_end_time - is_winded_start_time;
            if (is_winded_time_elapsed >= 5)
            {
                is_winded_end_time = 0;
                is_winded_start_time = 0;
            }
        }
        if (is_winded_time_elapsed >= 5 && is_sprinting_bool == false && stamina <= 100)
        {
            LoseStamina(-0.1f);
        }
        if (Input.GetKey(KeyCode.LeftShift) && is_winded_start_time == 0)
        {
            is_winded_start_time = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            is_sprinting_bool = false;
        }
        if (xp >= maxXP)
        {
            XP_level += 1;
            xp = 0;
            UpdateStatsUI();
        }
        if (health <= 0)
        {
            health = 100;
            DeathScene.SetActive(true);
            transform.position = DeathSceneTransform.position;
        }
        if (health != lasthealth)
        {
            is_hurt_start_time = Time.time;
        }
        if (is_hurt_start_time != 0)
        { 
            is_hurt_time_elapsed = is_hurt_end_time - is_hurt_start_time;
            if (is_hurt_time_elapsed >= 5)
            {
                is_hurt_end_time = 0;
                is_hurt_start_time = 0;
            }
        }
        if (is_hurt_time_elapsed >= 5 && health <= 100)
        {
            GainHealth(0.01f);
        }
        lasthealth = health;
        if(Input.GetKeyDown(KeyCode.I)){
            inventory_sprite.SetActive(true);
            crafting_sprite.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            inventory_sprite.SetActive(false);
            crafting_sprite.SetActive(false);
            help_sprite.SetActive(false);         
        }
        if(Input.GetKeyDown(KeyCode.H)){
            help_sprite.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R)){
            transform.position = RespawnPointTransform.position;
            savemanager.GetComponent<PlayerDataManager>().DeathResetGame();
            savemanager.GetComponent<PlayerDataManager>().SaveGame();
            DeathScene.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.E)){
            Debug.Log("registering key");
            Debug.Log(gameObject.GetComponent<Inventory>().FindSlot("Cooked Meat", gameObject.GetComponent<Inventory>().InventSlots, false).GetComponent<Slot>().quantity);
            if ((gameObject.GetComponent<Inventory>().FindSlot("Cooked Meat", gameObject.GetComponent<Inventory>().InventSlots, false).GetComponent<Slot>().quantity - 1) >= 0){
                gameObject.GetComponent<Inventory>().InsertSlot("Cooked Meat", -1, true);
                Debug.Log("RightPath");
                LoseHunger(-20);
            } else {
                Debug.Log("Wrong Path");
            }
        }
    }

    public void UpdateStatsUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float fillX = xpBarFiller.fillAmount;
        float fillS = staminaBarFiller.fillAmount;
        float fillH = hungerBarFiller.fillAmount;
        float fillT = thirstBarFiller.fillAmount;
        float hFraction = health / maxHealth;
        float xpFraction = xp / maxXP;
        float staminaFraction = stamina / maxStamina;
        float hungerFraction = hunger / maxHunger;
        float thirstFraction = thirst / maxThirst;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            frontHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            frontHealthBar.color = Color.white;
        }
        if (fillF < hFraction)
        {
            frontHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
            frontHealthBar.color = Color.white;
        }
        if (fillX < xpFraction)
        {
            xpBarFiller.fillAmount = xpFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            xpBarFiller.fillAmount = Mathf.Lerp(fillX, xpBarFiller.fillAmount, percentComplete);
        }
        if (fillX > xpFraction)
        {
            xpBarFiller.fillAmount = xpFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            xpBarFiller.fillAmount = Mathf.Lerp(fillX, xpFraction, percentComplete);
        }
        if (fillS < staminaFraction)
        {
            staminaBarFiller.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            staminaBarFiller.fillAmount = Mathf.Lerp(fillS, staminaBarFiller.fillAmount, percentComplete);
        }
        if (fillS > staminaFraction)
        {
            staminaBarFiller.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            staminaBarFiller.fillAmount = Mathf.Lerp(fillS, staminaFraction, percentComplete);
        }
        if (fillH < hungerFraction)
        {
            hungerBarFiller.fillAmount = hungerFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            hungerBarFiller.fillAmount = Mathf.Lerp(fillH, hungerBarFiller.fillAmount, percentComplete);
        }
        if (fillH > hungerFraction)
        {
            hungerBarFiller.fillAmount = hungerFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            hungerBarFiller.fillAmount = Mathf.Lerp(fillH, hungerFraction, percentComplete);
        }
        if (fillT < thirstFraction)
        {
            thirstBarFiller.fillAmount = thirstFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            thirstBarFiller.fillAmount = Mathf.Lerp(fillT, thirstBarFiller.fillAmount, percentComplete);
        }
        if (fillT > thirstFraction)
        {
            thirstBarFiller.fillAmount = thirstFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            thirstBarFiller.fillAmount = Mathf.Lerp(fillT, thirstFraction, percentComplete);
        }

    }

    public void GainHealth(float statAmount)
    {
        health += statAmount;
        lerpTimer = 0f;
    }
    public void LoseHunger(float statAmount)
    {
        hunger -= statAmount;
    }
    public void LoseThirst(float statAmount)
    {
        thirst -= statAmount;
    }
    public void GainXp(float statAmount)
    {
        xp += statAmount;
        lerpTimer = 0f;
    }
    public void LoseStamina(float staminaAmount)
    {
        stamina -= staminaAmount;
        is_winded_start_time = 0;
    }
}
