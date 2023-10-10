using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Perception/DamageSense"))]
public class DamageSense : Sense
{
    GameObject damageInstigator;
    public override void Init(MonoBehaviour owner)
    {
        base.Init(owner);
        HealthComponent healthComp = owner.GetComponent<HealthComponent>();
        if (healthComp != null )
            healthComp.onTakeDamage += OwnerTookDamage;
    }

    private void OwnerTookDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        damageInstigator = instigator;
    }

    public override bool IsStimuliSensable(PerceptionStimuli stimuli)
    {
        if(stimuli.gameObject == damageInstigator)
        {
            damageInstigator = null;
            return true;
        }

        return false;
    }
}
