using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombearSpawner : MonoBehaviour {

    //To ensure that Enemies spawn only if health > 0
    public PlayerHealth playerHealth;
    //To refer to the Prefabs of the Enemies
    public GameObject enemy;
    public float spawnTime = 10.0f;
    //Array of different positions (transforms) where Enemies can spawn
    public Transform[] spawnPoints;

    float timer;

    void Update()
    {
        //Repeatedly calls specified function starting after spawnTime seconds, and repeated every spawnTime seconds
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            timer = 0;
            Spawn();
        }
        //Changes Spawn rate, based on score
        if (ScoreManager.score >= 100 && ScoreManager.score < 150)
        {
            spawnTime = 5.0f;
        }
        if (ScoreManager.score >= 150 && ScoreManager.score < 200)
        {
            spawnTime = 2.0f;
        }
        if (ScoreManager.score >= 200 && ScoreManager.score < 250)
        {
            spawnTime = 1.0f;
        }
    }

    void Spawn()
    {
        if (playerHealth.currentHealth <= 0)
        {
            return;
        }
        //Index of the spawnPoints[] array
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        //Instantiate creates a new GameObject with specified position and specified rotation
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
