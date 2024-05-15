using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject winScreen;
    AudioManager audioManager;

    private void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Slash") && gameManager.keyCount > 0){
            //anim to open chest
            gameManager.score += 100;
            gameManager.keyCount--;
            winScreen.SetActive(true);
            Time.timeScale = 0f;
            audioManager.PlaySFX(audioManager.victory);
        }
    }
}
