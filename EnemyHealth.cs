using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    //Speed at which enemy sinks through the floor after they die
    public float sinkSpeed = 2.5f;
    //Score given to Player when this enemy is killed
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudioSource;
    //To refer to the Particle System attached to the GameObject
    ParticleSystem hitParticles;
    //Reference to Enemy Collider
    CapsuleCollider enemyCollider;
    bool isDead;
    //After Death animation is played, state changes to isSinking
    bool isSinking;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudioSource = GetComponent<AudioSource>();
        //Since the particle system is attached to a child
        //This goes through all the children, and uses the first Particle System that it finds
        hitParticles = GetComponentInChildren<ParticleSystem>();
        enemyCollider = GetComponent<CapsuleCollider>();
        currentHealth = startingHealth;
    }

    void Update()
    {
        if (isSinking)
        {
            //Translate the Enemy downward at speed sinkSpeed
            //Vector3.up is in-built
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    //It is a public function because it will be referenced from another script
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
        {
            return;
        }
        enemyAudioSource.Play();
        currentHealth -= amount;
        //Particle system will activate wherever the hitPoint is positioned
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        //After the Enemy is dead, the capsule collider is converted into a trigger, so that Player can walk through it
        enemyCollider.isTrigger = true;
        anim.SetTrigger("Die");
        enemyAudioSource.clip = deathClip;
        enemyAudioSource.Play();
    }

    public void StartSinking()
    {
        //To stop the enemy from following Player
        GetComponent<NavMeshAgent>().enabled = false;
        //To stop calculation of new geometry when Enemy starts sinking
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        //Add the score for killing enemy to the static variable score from ScoreManager script
        ScoreManager.score += scoreValue;
        //After it starts sinking, destroy it in 2 seconds
        Destroy(gameObject, 2.0f);
    }
}