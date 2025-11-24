using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]MeleeWeaponDamageCollider  meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();

    }
    
    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, ItemWeapons weapon)
    {
        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage; 
        meleeDamageCollider.magicDamage = weapon.magicDamage;
        meleeDamageCollider.fireDamage= weapon.fireDamage;
    }


}
