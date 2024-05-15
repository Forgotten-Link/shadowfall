using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreLabel;
    public int score = 0;

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = $"Score: {score}";
    }
}
