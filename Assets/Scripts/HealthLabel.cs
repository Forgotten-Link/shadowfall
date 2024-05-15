using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthLabel : MonoBehaviour
{
    [SerializeField] TMP_Text healthLabel;
    public int health = 4;

    // Update is called once per frame
    void Update()
    {
        healthLabel.text = $"Health: {health}";
        if(health <= 0) {
            SceneManager.LoadScene("PrjectScene");
        }
    }
}

