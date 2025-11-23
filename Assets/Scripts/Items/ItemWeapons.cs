using UnityEngine;

public class ItemWeapons : Items
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ=0;
    public int intREQ =0;
    public int faithREQ=0;

    [Header("Weapon Base Damage")]
    public int physicalDamage=0;
    public int magicDamage=0;
    public int fireDamage=0;

    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;

    
}
