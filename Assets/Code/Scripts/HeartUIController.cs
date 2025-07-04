﻿using UnityEngine;
using UnityEngine.UI;

public class HeartUIController : MonoBehaviour
{
    public Slider healthSlider;

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        Debug.Log(currentHealth);
        healthSlider.value = currentHealth;
    }
}
