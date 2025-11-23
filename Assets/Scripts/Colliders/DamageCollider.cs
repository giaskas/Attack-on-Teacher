using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DamageCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Damage")]
    public float physicalDamage = 0 ;
    public float magicDamage = 0;
    public float fireDamage = 0;

    [Header("Contact Point")]
    private Vector3 contactPoint;

    [Header("Character Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
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

        damageTarget.characterEffectsManager.ProcessInstantEffects(damageEffect);
    }
}
