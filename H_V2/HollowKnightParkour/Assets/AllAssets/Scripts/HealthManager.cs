using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance; // Singleton
    public int maxHealth = 10;
    public int currentHealth;
    private Vector3 lastCheckpoint; // Stores the last checkpoint position

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        currentHealth = maxHealth; 

    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

    public void TakeDamage()
    {
        currentHealth--;

        if (currentHealth > 0)
        {
            RespawnAtCheckpoint();
        }
        else
        {
            Die();
        }
    }
    public void TakeDamageWithoutCheckPoint()
    {
        currentHealth--;

        if (currentHealth > 0)
        {
        }
        else
        {
            Die();
        }
    }
    private void RespawnAtCheckpoint()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = lastCheckpoint;
        }
    }

    private void Die()
    {
        Debug.Log("Game Over! Restart or show UI here.");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart current scene
    }
}
