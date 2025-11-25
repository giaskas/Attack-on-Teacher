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

    [Header("Attack Modifiers")]
    public float light_Attack_01_Modifier =1.1f;
    public float heavy_Attack_01_Modifier =1.5f;
    public float charge_Attack_01_Modifier =2.0f;


    [Header("Stamina Costs Modifiers")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier=1;

    [Header("Actions")]
    public WeaponItemAction RB_Action;
    public WeaponItemAction RT_Action;
    
}
