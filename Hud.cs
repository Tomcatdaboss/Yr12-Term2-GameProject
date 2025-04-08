using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public GameObject XP_text;
    public GameObject DeathScene;
    public GameObject WinScene;
    public GameObject Win_button;
    public GameObject savemanager;
    public GameObject menu_sprite;
    Animator animator;
    private float lerpTimer;
    private bool tutorialcomplete = false;
    private int tutorialstep = 1;
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
    public GameObject start_sprite;
    public float XP_level;
    private Text xp_txt;
    private float lasthealth = 100;
    public float is_winded_start_time = 0;
    public float is_winded_end_time = 0;
    public float is_winded_time_elapsed = 0;
    public float is_hurt_start_time = 0;
    public float is_hurt_end_time = 0;
    public float is_hurt_time_elapsed = 0;
    public GameObject menu_cam;


    // Start is called before the first frame update
    public void Start()
    { // initialising values of the stats
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
        start_sprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        is_winded_end_time = Time.time; // every frame this code updates the current time
        is_hurt_end_time = Time.time; // every frame this code updates the current time

        xp_txt.text = XP_level.ToString();

        UpdateStatsUI(); // constantly triggers the check to change the UI to reflect current stat levels

        LoseHunger(0.001f);// loses hunger and thirst every frame.
        LoseThirst(0.001f);
        if (hunger <= 1){ // loses health if starving or thirsty
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
        if (is_winded_time_elapsed >= 5 && is_sprinting_bool == false && stamina <= 100) // if the player hasn't sprinted for a period of time, regain stamina.
        {
            LoseStamina(-0.1f);
        }
        if (Input.GetKey(KeyCode.LeftShift) && is_winded_start_time == 0) // if sprinting, set is_winded_start time to Time.time
        {
            is_winded_start_time = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) // if the player is no longer holding shift, stop sprinting.
        {
            is_sprinting_bool = false;
        }

        if (is_winded_start_time != 0) // if the player has sprinted and stopped, compare the time between the current time and the time they stopped sprinting until 5 seconds have passed.
        {
            is_winded_time_elapsed = is_winded_end_time - is_winded_start_time;
            if (is_winded_time_elapsed >= 5)
            {
                is_winded_end_time = 0;
                is_winded_start_time = 0;
            }
        }
       
        if (xp >= maxXP) // add XP levels when the xp approaches the required amount.
        {
            XP_level += 1;
            xp = 0;
            UpdateStatsUI();
        }
        if (health <= 0) //if the character loses all health, teleport the player to the death box and start the death UI.
        {
            health = 100;
            DeathScene.SetActive(true);
            transform.position = DeathSceneTransform.position;
        }
        if (health != lasthealth) // if the player was recently hurt, set is_hurt_start time to Time.time
        {
            is_hurt_start_time = Time.time;
        }
        if (is_hurt_start_time != 0) // if the player has been damaged and stopped taking damage, compare the time between the current time and the time they stopped taking damage until 5 seconds have passed.
        { 
            is_hurt_time_elapsed = is_hurt_end_time - is_hurt_start_time;
            if (is_hurt_time_elapsed >= 5)
            {
                is_hurt_end_time = 0;
                is_hurt_start_time = 0;
            }
        }
        if (is_hurt_time_elapsed >= 5 && health <= 100) // gain health after 5 seconds have passed after taking damage.
        {
            GainHealth(0.01f);
        }
        lasthealth = health; // updates every frame to check if damage has been taken.

        if(Input.GetKeyDown(KeyCode.S) && Win_button.activeSelf == true){ // if the boat has been crafted and the player is in the boat and pressed S, trigger the win animation.
            WinGame();
        }

        if(Input.GetKeyDown(KeyCode.I)){ // open inventory
            inventory_sprite.SetActive(true);
            crafting_sprite.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Escape)){ // close all menus
            inventory_sprite.SetActive(false);
            crafting_sprite.SetActive(false);
            help_sprite.SetActive(false);
            if(tutorialcomplete == false && tutorialstep == 1){
                start_sprite.GetComponentInChildren<Text>().text = "Press W to move forward, A and D to move left and right, and S to move backwards."; 
                start_sprite.GetComponent<RectTransform>().anchoredPosition = new Vector2(-447, 150);
                start_sprite.GetComponent<RectTransform>().sizeDelta = new Vector2(250,70);
                tutorialstep += 1;
            } else if (tutorialcomplete){
                start_sprite.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1f);
            }       
        }
        if(gameObject.GetComponent<PlayerMovement>().isMoving && tutorialcomplete == false && tutorialstep == 2){
            start_sprite.GetComponentInChildren<Text>().text = "Press Space to jump. But don't jump from heights, because you'll get hurt!"; 
            tutorialstep += 1;
        }
        if(Input.GetKeyDown(KeyCode.Space) && tutorialcomplete == false && tutorialstep == 3){
            start_sprite.GetComponentInChildren<Text>().text = "Press I to look at your Inventory. It's empty right now, but you can use your tools to get materials."; 
            tutorialstep += 1;
        }
        if(Input.GetKeyDown(KeyCode.I) && tutorialcomplete == false && tutorialstep == 4){
            start_sprite.GetComponentInChildren<Text>().text = "Press H to look at a list of controls that might help you on your journey."; 
            tutorialstep += 1;
        }
        if(Input.GetKeyDown(KeyCode.H) && tutorialcomplete == false && tutorialstep == 5){
            start_sprite.GetComponentInChildren<Text>().text = "Now, equip your Axe by pressing 1, and get 10 wood from a nearby tree."; 
            tutorialstep += 1;
        }
        try{
            if(gameObject.GetComponent<Inventory>().FindSlot("Wood", gameObject.GetComponent<Inventory>().InventSlots, false).GetComponent<Slot>().quantity >= 10 && tutorialcomplete == false && tutorialstep == 6){
                start_sprite.GetComponentInChildren<Text>().text = "Great! Your XP level will grow as you harvest materials, allowing you to learn how to make new things. Try to get your level to 1."; 
                tutorialstep += 1;
            }
        } catch {

        }
        if(XP_level >= 1 && tutorialcomplete == false && tutorialstep == 7){
            start_sprite.GetComponentInChildren<Text>().text = "Great! Now, you can craft new gear! Look around for a rock to mine from, and get 10 stone!"; 
            tutorialstep += 1;
        }
        try{
            if(gameObject.GetComponent<Inventory>().FindSlot("Stone", gameObject.GetComponent<Inventory>().InventSlots, false).GetComponent<Slot>().quantity >= 10 && tutorialcomplete == false && tutorialstep == 8){
                start_sprite.GetComponentInChildren<Text>().text = "Your hunger and thirst bars are on the bottom left. To quench your thirst, stand in water. To eat, kill an animal with a Spear and harvest its meat with it. Remember: if in doubt, always go upward. Good luck! Press escape to close this tutorial."; 
                start_sprite.GetComponentInChildren<Text>().fontSize = 10;
                tutorialstep += 1;
                tutorialcomplete = true;
            }
        } catch{}
        if(Input.GetKeyDown(KeyCode.H)){ // open the help menu
            help_sprite.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R)){ // triggers the respawn process and wipes the inventory due to death
            savemanager.GetComponent<PlayerDataManager>().DeathResetGame();
            savemanager.GetComponent<PlayerDataManager>().SaveGame();
            DeathScene.SetActive(false);
            WinScene.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.E)){ // check if there is any cooked meat in the inventory, and if so, eat one and add 20 hunger to the hunger stat.
            GameObject prospective_meat_slot = gameObject.GetComponent<Inventory>().FindSlot("Cooked Meat", gameObject.GetComponent<Inventory>().InventSlots, false);
            if (prospective_meat_slot != null) {
                if ((gameObject.GetComponent<Inventory>().FindSlot("Cooked Meat", gameObject.GetComponent<Inventory>().InventSlots, false).GetComponent<Slot>().quantity - 1) >= 0){
                    gameObject.GetComponent<Inventory>().InsertSlot("Cooked Meat", -1, true);
                    LoseHunger(-20);
                }
            }
        }

    }

    public void UpdateStatsUI() 
    { // every frame, this updates the on-screen stat bars to match their integer fractions of the maximum.
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

        if (fillB > hFraction) // if the health has decreased do this
        {
            frontHealthBar.color = Color.red; // adds a red tint to indicate that the health is decreasing
            frontHealthBar.fillAmount = hFraction; // next 5 lines slowly decrease the health bar on screen
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            frontHealthBar.color = Color.white; // return to normal colour
        }
        if (fillF < hFraction) // if the health has increased do this
        {
            frontHealthBar.color = Color.green; // adds a green tint to indicate that the health is increasing
            backHealthBar.fillAmount = hFraction; // next 5 lines slowly increase the health bar on screen
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
            frontHealthBar.color = Color.white; // return to normal colour
        }
        if (fillX < xpFraction) // if the xp has increased do this
        {
            xpBarFiller.fillAmount = xpFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            xpBarFiller.fillAmount = Mathf.Lerp(fillX, xpBarFiller.fillAmount, percentComplete);
        }
        if (fillX > xpFraction) // if the xp has decreased do this
        {
            xpBarFiller.fillAmount = xpFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            xpBarFiller.fillAmount = Mathf.Lerp(fillX, xpFraction, percentComplete);
        }
        if (fillS < staminaFraction) // if the stamina has increased do this
        {
            staminaBarFiller.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            staminaBarFiller.fillAmount = Mathf.Lerp(fillS, staminaBarFiller.fillAmount, percentComplete);
        }
        if (fillS > staminaFraction) // if the stamina has decreased do this
        {
            staminaBarFiller.fillAmount = staminaFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            staminaBarFiller.fillAmount = Mathf.Lerp(fillS, staminaFraction, percentComplete);
        }
        if (fillH < hungerFraction) // if the hunger has increased do this
        {
            hungerBarFiller.fillAmount = hungerFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            hungerBarFiller.fillAmount = Mathf.Lerp(fillH, hungerBarFiller.fillAmount, percentComplete);
        }
        if (fillH > hungerFraction) // if the hunger has decreased do this
        {
            hungerBarFiller.fillAmount = hungerFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            hungerBarFiller.fillAmount = Mathf.Lerp(fillH, hungerFraction, percentComplete);
        }
        if (fillT < thirstFraction) // if the thirst has increased do this
        {
            thirstBarFiller.fillAmount = thirstFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            thirstBarFiller.fillAmount = Mathf.Lerp(fillT, thirstBarFiller.fillAmount, percentComplete);
        }
        if (fillT > thirstFraction) // if the thirst has decreased do this
        {
            thirstBarFiller.fillAmount = thirstFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            thirstBarFiller.fillAmount = Mathf.Lerp(fillT, thirstFraction, percentComplete);
        }

    }

    public void OnCollisionStay(Collision collider) { // if the player is near enough to the boat, reveal the win button
        if(collider.gameObject.tag == "Boat"){
            Win_button.SetActive(true);
        }
    }
    public void OnCollisionExit(Collision collider) { // if the player is out of range of the boat, close the win button
        if(collider.gameObject.tag == "Boat"){
            Win_button.SetActive(false);
        }
    }

    public void GainHealth(float statAmount) // increases the health by the statAmount. Can be negative, to decrease health.
    {
        health += statAmount;
        lerpTimer = 0f;
    }
    public void LoseHunger(float statAmount) // decreases the hunger by the statAmount. Can be negative, to increase hunger.
    {
        hunger -= statAmount;
    }
    public void LoseThirst(float statAmount) // decreases the thirst by the statAmount. Can be negative, to increase thirst.
    {
        thirst -= statAmount;
    }
    public void GainXp(float statAmount) // increases the xp by the statAmount. Can be negative, to decrease xp.
    {
        xp += statAmount;
        lerpTimer = 0f;
    }
    public void LoseStamina(float staminaAmount) // decreases the stamina by the statAmount. Can be negative, to increase stamina.
    {
        stamina -= staminaAmount;
        is_winded_start_time = 0;
    }
    public void WinGame(){ // if the player presses the S key and wins, trigger the win UI.
        health = 100;
        WinScene.SetActive(true);
        savemanager.GetComponent<PlayerDataManager>().SaveGame();
        transform.position = DeathSceneTransform.position;
    }
}
