using UnityEngine;
using UnityEngine.TextCore.Text;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager characterCausingDamage;

    [Header("Weapon Attack Modifier")]
    public float light_Attack_01_Modifier;
    public float heavy_Attack_01_Modifier;
    public float charge_Attack_01_Modifier;

    protected override void Awake()
    {
        base.Awake();
        damageCollider.enabled=false;
        if(damageCollider == null)
        {
            damageCollider= GetComponent<Collider>();
        }
        damageCollider.enabled= false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if (damageTarget != null)
        {
            if(damageTarget == characterCausingDamage)
                return;
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
    }
    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if(charactersDamaged.Contains(damageTarget))
        {
            return;
        }
            
        
        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage=physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.contactPoint = contactPoint;

        switch (characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifier(light_Attack_01_Modifier,damageEffect);
                break;
            case AttackType.HeavyAttack01:
                ApplyAttackDamageModifier(heavy_Attack_01_Modifier,damageEffect);
                break;
            case AttackType.ChargedAttack01:
                ApplyAttackDamageModifier(charge_Attack_01_Modifier,damageEffect);
                break;

        }

        if (characterCausingDamage.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                characterCausingDamage.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z);
        }

        //damageTarget.characterEffectsManager.ProcessInstantEffects(damageEffect);    
    }

    private void ApplyAttackDamageModifier(float modifier, TakeDamageEffect damage)
    {
        damage.physicalDamage +=modifier;
        damage.magicDamage += modifier;
        damage.fireDamage += modifier;
        
    }
}
