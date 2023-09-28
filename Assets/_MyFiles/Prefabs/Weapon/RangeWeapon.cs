using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    [SerializeField] float damage = 10f;
    [SerializeField] ParticleSystem fireVFX;
    private AimTargettingComponent aimTargettingComponent;

    private void Awake()
    {
        aimTargettingComponent = GetComponent<AimTargettingComponent>();
    }

    public override void Attack()
    {
        GameObject target = aimTargettingComponent.GetTarget();
        if (target != null)
        {
            DamageGameObject(target, damage);
        }

        fireVFX.Emit(fireVFX.emission.GetBurst(0).maxCount);
    }
}
