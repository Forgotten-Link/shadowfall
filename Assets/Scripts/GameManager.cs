using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;

public class GameManager : MonoBehaviour
{
    //initializes the player's health and score
    public float health = 100f;
    public int score = 0;
    public float mana = 1f;
    public float maxStamina = 1f;
    public float stamina = 1f;

    public int keyCount = 0;

    public Image healthBar;
    public Image staminaBar;
    public Image manaBar;
    public GameObject keyUI;

    [SerializeField] TMP_Text scoreLabel;
    [SerializeField] TMP_Text healthLabel;
    [SerializeField] TMP_Text manaLabel;
    [SerializeField] TMP_Text staminaLabel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = $"Score: {score}";
        healthLabel.text = $"Health: {health}";
        if(health <= 0) {
            SceneManager.LoadScene("Level");
        }
        manaLabel.text = $"MP: {mana}";
        staminaLabel.text = $"SP: {stamina}";
        //healthBar.fillAmount = health / 100f;
        Debug.Log(stamina);
        if (stamina < 1){
            stamina += .08f * Time.deltaTime;
        }
        if (mana < 1){
            mana += .065f * Time.deltaTime;
        }
        staminaBar.fillAmount = stamina;
        manaBar.fillAmount = mana;
        if (keyCount > 0)
        {
            keyUI.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / 100f;
    }
    public void Heal(float healingAmount)
    {
        health += healingAmount;
        health = Mathf.Clamp(health, 0, 100);
        healthBar.fillAmount = health / 100f;
    }

    public void useStamina(float staminaCost)
    {
        stamina -=staminaCost;
        
    }

    public void useMana(float manaCost)
    {
        mana -= manaCost;
        
    }

}
