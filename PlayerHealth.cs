using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    //Image to be flashed on screen when taking damage
    public Image damageImage;
    //Audio to be played when dying
    public AudioClip deathClip;
    //How quickly damageImage will flash on the screen
    public float flashSpeed = 5.0f;
    //Red color, with 0.1 Opacity (Alpha)
    public Color flashColor = new Color(1.0f, 0f, 0f, 0.1f);
    Animator anim;
    AudioSource playerAudioSource;
    //Reference to the PlayerMovement script
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    //Every time Player is hit by Enemy
    bool isDamaged;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = startingHealth;
        playerShooting = GetComponentInChildren<PlayerShooting>();
    }

    void Update()
    {
        //If damaged, flash the color on screen
        if (isDamaged)
        {
            damageImage.color = flashColor;
        }
        //Otherwise fade the color back to clear
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        isDamaged = false;
    }

    //Set the new health in value and in slider after taking damage
    public void TakeDamage(int amount)
    {
        isDamaged = true;
        currentHealth -= amount;
        //Change value of slifer to match remaining health
        healthSlider.value = currentHealth;
        //Play the current audio clip, which is Player Hurt clip by default
        playerAudioSource.Play();
        //If the health has been depleted, call the function
        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        //Play dying animation
        anim.SetTrigger("Die");
        //Change audio clip and play it
        playerAudioSource.clip = deathClip;
        playerAudioSource.Play();
        //Disable the script, so Player cannot be controlled after it dies
        playerMovement.enabled = false;
        //Disable all efects, if active, after dying
        playerShooting.DisableEffects();
        //Disable the script, so Player cannot shoot after it dies
        playerShooting.enabled = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
