using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public ItemWeapons currentRightHandWeapon;
    public ItemWeapons currentLeftHandWeapon;


    [Header("QuickSlots")]
    public ItemWeapons[] weaponsInRigthHandSlots = new ItemWeapons[3];
    public int rightHandedWeaponIndex = 0;
    public ItemWeapons[] weaponsInLeftHandSlots = new ItemWeapons[3];
    public int leftHandedWeaponIndex = 0;



}
