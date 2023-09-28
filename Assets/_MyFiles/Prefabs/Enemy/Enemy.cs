using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] ValueGauge healtBarPrefab;
    [SerializeField] Transform healthBarAttachTransform;
    HealthComponent healthComponent;

    ValueGauge healthBar;
    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onTakeDamage += TookDamage;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onHealthChanged += HealthChanged;

        healthBar = Instantiate(healtBarPrefab, FindObjectOfType<Canvas>().transform);
        UIAttachComponent attachmentComp = healthBar.AddComponent<UIAttachComponent>();
        attachmentComp.SetupAttachment(healthBarAttachTransform);
    }

    private void HealthChanged(float currenthHealth, float delta, float maxHealth)
    {

    }

    private void StartDeath(float delta, float maxHealth)
    {
        Debug.Log("Dead!");
    }

    private void TookDamage(float currentHealth, float delta, float maxHealth)
    {
        Debug.Log($"Took Damage {delta}, now health is: {currentHealth}/{maxHealth}");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
