using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    //How far the bullet can go
    public float range = 100.0f;

    //To synchronise everything
    float timer;
    Ray shootRay;
    //To determine what is hit by the ray
    RaycastHit shootHit;
    //To ensure that ray will hit only these  objects
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudioSource;
    Light gunLight;
    //How long the effects are viewable before they diappear
    float effectsDisplayTime = 0.2f;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        gunAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Fire1 maps to Left Ctrl and Left Mouse Button
        if(Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            Shoot();
        }
        //Since timer has been reset after shooting, we need small enough time to disable effects, so it does not overlap with next Shoot
        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        //Particles and Audio not included here, since the sources are always enabled, and they play once and then stop
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f;
        gunAudioSource.Play();
        gunLight.enabled = true;
        //The particles are stopped and then played to make visuals smoother
        gunParticles.Stop();
        gunParticles.Play();
        gunLine.enabled = true;
        //Set position of 0th vertex to position of GameObject
        gunLine.SetPosition(0, transform.position);
        //Start of Ray is at current GameObject
        shootRay.origin = transform.position;
        //transfrom.forward corresponds to z-axis by default
        //In the scene, z-axis of GunBarrelEnd points away from the Player
        shootRay.direction = transform.forward;
        //Ray shootRay will output hit information into shootHit, if it hits something in shootableMask layer, within range distance
        if(Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            //Get reference to the EnemyHealth script attached to the GameObject with collider at shootHit
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
            //Since we are able to hit objects like the wall and toys lying about, we have to also check whether they contain an EnemyHealth script component
            if(enemyHealth != null)
            {
                //If it is an Enemy object, call its TakeDamage function from the script
                //Enemy takes damagePerShot amount of damage, and is hit at shootHit.point position for particle effects
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }
            //If bullet hits something, the end point of the line should be hit position
            gunLine.SetPosition(1, shootHit.point);
        }
        //Render the line even if no hit, for range length
        else
        {
            //Render the second vertex of the line at the end of range (position + vector = position)
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

}
