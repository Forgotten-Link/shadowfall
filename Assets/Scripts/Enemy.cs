using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    //public Transform[] patrolPoints;
    //private int currentPointIndex = 0;

    public int damage = 1; // Damage dealt to the player

    AudioManager audioManager;


    public int hp;
    public Vector3 finishPos = Vector2.zero;
    public Vector3 startPos;
    public float trackPercent = 0;
    private int direction = -1;

    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start() {
        startPos = transform.position;
        Flip();
    }

    void Update()
    {
        //Patrol();
        if(hp <= 0){
            audioManager.PlayEnemySFX(audioManager.enemyDeath);
            Destroy(this.gameObject);
        }
        //moving code
        trackPercent += direction * speed * Time.deltaTime;
        float x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
        float y = (finishPos.y - startPos.y) * trackPercent + startPos.y;
        transform.position = new Vector3(x, y, startPos.z);

        //Reverses direction when it has finshed moving
        if ((direction == 1 && trackPercent > 0.9f) || (direction == -1 && trackPercent < 0.1f)) {
            direction *= -1;
            Flip();
        }
    }
/*
    void Patrol()
    {
        
        Transform targetPoint = patrolPoints[currentPointIndex];

        // Move towards the next patrol point
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Check if the enemy has reached the patrol point
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Flip to face the next direction and move to the next point
            Flip();
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
        
        //interpolate new location between start and finish positions

    }
*/
    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Slash"))
        {
            // Assuming the player has a script with a method like TakeDamage(int amount)
            //collision.GetComponent<Player>().TakeDamage(damage);
            hp-=1;
        }
        if (other.gameObject.CompareTag("Stab"))
        {
            // Assuming the player has a script with a method like TakeDamage(int amount)
            //collision.GetComponent<Player>().TakeDamage(damage);
            hp-=1;
        }
        if (other.gameObject.CompareTag("FlameJet"))
        {
            // Assuming the player has a script with a method like TakeDamage(int amount)
            //collision.GetComponent<Player>().TakeDamage(damage);
            hp-=2;
        }
        //if (other.gameObject.CompareTag("Fireball"))
        //{
            // Assuming the player has a script with a method like TakeDamage(int amount)
            //collision.GetComponent<Player>().TakeDamage(damage);
            //hp-=2;
        //}
    }
}
