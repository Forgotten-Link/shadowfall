using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformerPlayer : MonoBehaviour
{
    public float speed = 4.5f;
    public float jumpforce = 12.0f;
    public int jumpCount = 1;
    public float direction = 1;

    public float dashforce = 60.0f;
    public int dashCount = 1;

    public int health = 4;

    public ScoreManager scoreBoard;
    public HealthLabel healthLabel;

    //gets reference to the desired componenent of the player character
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D box;
    private SpriteRenderer sprite;

    private void Start()
    {
        //stores reference to the desired component
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //moves the character
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;

        //gets the bounds of the box collider and only allows the player to jump if it is colliding or overlapping with another box
        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;
        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;
        if (hit!= null) {
            grounded = true;
        }
        //sets the speed parameter in the animator to the character's speed
        anim.SetFloat("Speed", Mathf.Abs(deltaX));

        //flips the character based on the direction that they are moving
        if (!Mathf.Approximately(deltaX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
            
        }
        //allows the player to jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            body.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !grounded && jumpCount > 0) {
            body.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            jumpCount-=1;
        }

        //dashing
        if (Input.GetKeyDown(KeyCode.Mouse1) && dashCount > 0) {
            direction = transform.localScale.x;
            body.AddForce(Vector2.right * dashforce * direction, ForceMode2D.Impulse);
            dashCount-=1;
        } 
        //resets doublejump and dashing
        if (grounded) {
            dashCount = 1;
            jumpCount =1;
        }

        //attaches the player to the platform as a child when the player has jumped on it
        MovingPlatform platform = null;
        if (hit!= null) {
            platform = hit.GetComponent<MovingPlatform>();
        }
        if (platform != null) { 
            transform.parent = platform.transform;
        } 
        //removes the player as a child if they jump off the platform
        else {
             transform.parent = null;
        }
        Vector3 playerScale = Vector3.one;
        if (platform != null) {
            playerScale = platform.transform.localScale;
        }
        if (!Mathf.Approximately(deltaX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / playerScale.x, 1 / playerScale.y, 1);
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Point")){
            Destroy(other.gameObject);
            scoreBoard.score++;
            healthLabel.health++;
            //scoreBoard.scoreUpdate();
        }
        if(other.gameObject.CompareTag("Pineapple")){
            Destroy(other.gameObject);
            scoreBoard.score+=2;
            healthLabel.health+=2;
        }
        if(other.gameObject.CompareTag("Spike")) {
            healthLabel.health--;
            scoreBoard.score--;
        }

    }

}
