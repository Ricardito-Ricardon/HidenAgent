using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void OnHealthChanged(float currenthHealth, float delta, float maxHealth);
    public delegate void OnHealthEmpty(float delta, float maxHealth);
    public delegate void OnTakeDamage(float currentHealth, float delta, float maxHealth);

    public event OnHealthChanged onHealthChanged;
    public event OnHealthEmpty onHealthEmpty;
    public event OnTakeDamage onTakeDamage;

    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth;

    public void ChangeHealth(float amt)
    {
        if(amt == 0 || currentHealth == 0)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth, amt, maxHealth);
        if(amt < 0)
        {
            onTakeDamage.Invoke(currentHealth, amt, maxHealth);
        }
        if(currentHealth == 0)
        {
            onHealthEmpty?.Invoke(amt, maxHealth);
        }
    }
}
