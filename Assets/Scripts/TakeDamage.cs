using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TakeDamage : MonoBehaviour
{
    public Slider healthSlider;
    public int startingHealth = 100;
    public float speedAtMaxDamage = 30;
    public float maxDamage = 25;

    private int currentHealth;
    private bool isAlive;

    void Start()
    {
        currentHealth = startingHealth;
        isAlive = true;
        healthSlider.maxValue = startingHealth;
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
            LevelManager levelManager = GetComponent<LevelManager>();
            levelManager.HandleLevelFail();
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        float maxDamagePercent = Mathf.Clamp(relativeVelocityMagnitude / speedAtMaxDamage, 0, 1);
        int damage = (int)(maxDamage * maxDamagePercent);

        currentHealth -= damage;
    }

    void UpdateHealthSlider()
    {
        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }
    }
}
