using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [SerializeField] Weapon[] initialWeaponsPrefabs;
    [SerializeField] Transform[] weaponSlots;
    [SerializeField] Transform defaultWeaponSlots;
    private List<Weapon> weapons = new List<Weapon>();

    int currentWeaponIndex = -1; //negative value means something will not exit.

    private void Awake()
    {
        InitialWeapons();
    }

    private void InitialWeapons()
    {
        foreach(Weapon weaponPrefab in initialWeaponsPrefabs)
        {
            Transform weaponSlot = defaultWeaponSlots;
            foreach(Transform slot in weaponSlots)
            {
                if (slot.name == weaponPrefab.GetSlotName())
                {
                    weaponSlot = slot;
                    break;
                }
            }
            Weapon newWeapon = Instantiate<Weapon>(weaponPrefab, weaponSlot);
            newWeapon.Init(gameObject);
            weapons.Add(newWeapon);
            newWeapon.UnEquip();
        }

        NextWeapon();
    }

    public void NextWeapon()
    {
        int nextIndex = currentWeaponIndex + 1;
        if( nextIndex >= weapons.Count )
        {
            nextIndex = 0;
        }

        EquipWeapon(nextIndex);
    }

    private void EquipWeapon(int weaponIndex)
    {
        if( weaponIndex<0 || weaponIndex >= weapons.Count )
        {
            return;
        }

        if(currentWeaponIndex != -1)
        {
            weapons[currentWeaponIndex].UnEquip();
        }

        currentWeaponIndex = weaponIndex;
        weapons[currentWeaponIndex].Equip();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void DamagePoint()
    {
        if(currentWeaponIndex>= 0 && currentWeaponIndex< weapons.Count )
        {
            weapons[currentWeaponIndex].Attack();
        }
    }    
}
