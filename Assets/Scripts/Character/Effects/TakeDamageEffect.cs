using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Character Effects / Instant Effects/ Take Damage")]


public class TakeDamageEffect : InstantCharacterEffects
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0 ;
    public float magicDamage = 0;
    public float fireDamage = 0;

     [Header("Final Damage")]
    private int finalDamageDealt= 0; //el daño despues de toda la defensa y demas

    [Header("Animation")]
    public bool PlayDamageAnimation=true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageFX = true;

    [Header("Direction Damage Taken")]
    public float angleHitFrom;
    public Vector3 contactPoint;

   
    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);
        if(character.isDead.Value)
            return;



        CalculateDamage(character);

        
        
    }

    private void CalculateDamage(CharacterManager character)
    {

        if(!character.IsOwner)
            return;
        if(characterCausingDamage!= null)
        {
            
        }




        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage);
        

        
        
            if (finalDamageDealt <= 0)
            {
                finalDamageDealt=1;
            }
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        
    }
}
