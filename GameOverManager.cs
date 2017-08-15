﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

    public PlayerHealth playerHealth;
    public float restartDelay = 5.0f;

    Animator anim;
    float restartTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(playerHealth.currentHealth <= 0 )
        {
            anim.SetTrigger("GameOver");
            if (Input.GetButton("Fire1"))
            {
                playerHealth.RestartLevel();
            }
        }
    }
}