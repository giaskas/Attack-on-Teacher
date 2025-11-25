using System.Diagnostics;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    //process instant effects (take damage, heal)

    //process timed effects (poison, build UPS)

    //process static effects (adding/removing buffs from equipment)

    CharacterManager character;
    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void ProcessInstantEffects(InstantCharacterEffects effect)
    {
        effect.ProcessEffect(character);
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        if(bloodSplatterVFX != null)
        {
            GameObject bloodSplater = Instantiate (bloodSplatterVFX,contactPoint , Quaternion.identity);
        }
        else
        {
            GameObject bloodSplater = Instantiate (WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }
}
