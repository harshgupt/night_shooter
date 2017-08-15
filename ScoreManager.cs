using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    //static used to make it global
    public static int score;
    //Reference to the Text, so it can be modified
    Text scoreText;

    void Awake()
    {
        scoreText = GetComponent<Text>();
        score = 0;
    }

    void Update()
    {
        //+ is used to concatenate
        scoreText.text = "Score: " + score;
    }

}
