using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreMechanics : MonoBehaviour
{
     public int score;
     public int highScore;


    private void OnEnable()
    {
        EventManager.OnEnemyKilled += UpdateScore;
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
    }

    void UpdateScore()
    {
        score++;
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
