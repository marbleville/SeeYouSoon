using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    public Slider healthSlider;
    public int startingHealth = 100;
    private int currentHealth;
    private bool isAlive;
    
    void Start()
    {
        currentHealth = startingHealth;
        isAlive = true;
        UpdateHealthSlider();
    }

    void Update()
    {
        UpdateHealthSlider();
        if (currentHealth <= 0)
        {
            isAlive = false;
        }

        if (!isAlive)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            currentHealth -= 25;
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= 10;
        }
        currentHealth = Math.Clamp(currentHealth, 0, startingHealth);
    }

    void UpdateHealthSlider()
    {
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }
    }
}
