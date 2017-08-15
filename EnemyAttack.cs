using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject player;
    //Reference to PlayerHealth and EnemyHealth scripts
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    //Whether Player is close enough for the Enemy to attack
    bool isPlayerInRange;
    //Synchronisation Timer
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Used to reference from another GameObject
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    //When the Enemy trigger collides with other Collider
    void OnTriggerEnter(Collider other)
    {
        //Check if the other Collider is player game object
        if(other.gameObject == player)
        {
            //When triggered, Player is near enough to attack
            isPlayerInRange = true;
        }
    }

    //When the Enemy trigger exits the other Collider
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            //When triggered, Player is out of range of attack
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Attack Player after every timeBetweenAttacks time, if Player is in range
        if(timer >= timeBetweenAttacks && isPlayerInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
        }
        if(playerHealth.currentHealth <= 0)
        {
            //Change Enemy animation to Idle after Player dies
            anim.SetTrigger("PlayerDie");
        }
    }

    void Attack()
    {
        //Reset the timer
        timer = 0f;
        //If Player has health, do attackDamage amount of damage
        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

}
