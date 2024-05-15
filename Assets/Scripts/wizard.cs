using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.TextCore.Text;

public class wizard : MonoBehaviour
{
    public float speed = 4.5f;
    public float runMultiplier = 1.5f;
    public float jumpforce = 12.0f;
    public int jumpCount = 1;
    public float direction = 1;
    //code for dashing
    public float dashforce = 60.0f;
    public int dashCount = 1;

    public int health = 4;

    public float characterWidth = .5f;

    public bool flipDirection;

    public float pivotAdjustmentDistance = 0.5f;
    public Transform characterSprite;

    public GameManager gameManager;
    public GameObject pauseMenu;
    AudioManager audioManager;

    //gets reference to the desired componenent of the player character
    private Rigidbody2D body;
    private Animator anim;
    private CapsuleCollider2D box;
    private SpriteRenderer sprite;

    public GameObject character;

    //variables for stamina manipulation
    public float runningStaminaDrain;
    public float stmainaRechargeRate;
    public Coroutine recharge;

    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        //stores reference to the desired component
        body = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        box = GetComponentInChildren<CapsuleCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        
    }

    void Update()
    {
        //running
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift) && gameManager.stamina > 0f){
            currentSpeed *= runMultiplier;
            gameManager.stamina -= .15f * Time.deltaTime;
        }
        anim.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
        //moves the character
        float moveInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(moveInput * currentSpeed, body.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(moveInput));
        //float deltaX = Input.GetAxis("Horizontal") * speed;
        //Vector2 movement = new Vector2(deltaX, body.velocity.y);
        //body.velocity = movement;
        if (!Mathf.Approximately(moveInput, 0f))
        {
            FlipCharacter(moveInput);
        }

        //gets the bounds of the box collider and only allows the player to jump if it is colliding or overlapping with another box
        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        anim.SetBool("isJumping", true);
        if (hit!= null) {
            grounded = true;
            anim.SetBool("isJumping", false);
        }
        //sets the speed parameter in the animator to the character's speed
        //anim.SetFloat("Speed", Mathf.Abs(deltaX));
        //  transform.localScale = new Vector3(Mathf.Sign(deltaX)* .5f, .5f, .5f);

         
        
        //allows the player to jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded && gameManager.stamina >= .05f) {
            audioManager.PlaySFX(audioManager.jump);
            body.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            gameManager.useStamina(.05f);
        }
        //DoubleJump
        //if (Input.GetKeyDown(KeyCode.Space) && !grounded && jumpCount > 0) {
          //  body.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            //jumpCount-=1;
        //}

        //OpenPauseMenu
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log(pauseMenu.activeSelf);
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf == true){
                Time.timeScale = 0f;
            }
            else {
                Time.timeScale = 1f;
            }
        }

        //resets doublejump and dashing
        if (grounded) {
            dashCount = 1;
            jumpCount =1;
        }

        //anim.SetBool("attack1", Input.GetKey(KeyCode.Mouse0));
        //anim.SetBool("attack2", Input.GetKey(KeyCode.Mouse1));
        //anim.SetBool("flameJet", Input.GetKey(KeyCode.Q));
        //anim.SetBool("fireball", Input.GetKey(KeyCode.F));

        if(gameManager.stamina >= 0.05f && Input.GetKeyDown(KeyCode.Mouse0)) {
            //anim.SetBool("attack1", Input.GetKey(KeyCode.Mouse0));
            anim.Play("attack_1");
            audioManager.PlaySFX(audioManager.swordSlash);
            gameManager.useStamina(.05f);
        }
        if(Input.GetKeyDown(KeyCode.Mouse1) && gameManager.stamina >= 0.05f) {
            anim.Play("attack_2");
            audioManager.PlaySFX(audioManager.swordStab);
            gameManager.useStamina(.05f);
        }
        if(Input.GetKeyDown(KeyCode.Q) && gameManager.mana >= 0.25f) {
            anim.Play("flame_jet");
            audioManager.PlaySFX(audioManager.fireBall);
            gameManager.useMana(.25f);
        }
        if(Input.GetKeyDown(KeyCode.F) && gameManager.mana >= 0.05f) {
            anim.Play("fireball");
            audioManager.PlaySFX(audioManager.fireStream);
            gameManager.useMana(.05f);
        }



    }

    void FlipCharacter(float deltaX)
    {
        // Assuming your character moves and flips here
        Vector3 scale = characterSprite.localScale;
        if (deltaX > 0)
        {
            scale.x = Mathf.Abs(scale.x);
            characterSprite.localPosition = new Vector3(pivotAdjustmentDistance, characterSprite.localPosition.y, characterSprite.localPosition.z);
        }
        else if (deltaX < 0)
        {
            scale.x = -Mathf.Abs(scale.x);
            characterSprite.localPosition = new Vector3(-pivotAdjustmentDistance, characterSprite.localPosition.y, characterSprite.localPosition.z);
        }
        characterSprite.localScale = scale;
    }
    //checks if the player has entered a trigger and executes the corresponding effect
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Point")){
            audioManager.PlaySFX(audioManager.chomp);
            Destroy(other.gameObject);
            gameManager.score++;
            gameManager.Heal(5);
        }
        if(other.gameObject.CompareTag("Pineapple")){
            Destroy(other.gameObject);
            audioManager.PlaySFX(audioManager.chomp);
            gameManager.score+=2;
            gameManager.Heal(10);
        }
        if(other.gameObject.CompareTag("Spike")) {
            audioManager.PlayEnemySFX(audioManager.hurt);
            gameManager.TakeDamage(10f);
        }
        if(other.gameObject.CompareTag("Kill")) {
            audioManager.PlayEnemySFX(audioManager.hurt);
            gameManager.health -= 10000;
            gameManager.score -= 10000;
        }
        if(other.gameObject.CompareTag("Enemy")) {
            audioManager.PlayEnemySFX(audioManager.hurt);
            gameManager.TakeDamage(1f);
        }
        if(other.gameObject.CompareTag("Key")) {
            audioManager.PlayEnemySFX(audioManager.key);
            Destroy(other.gameObject);
            gameManager.keyCount++;
        }

    }
}
