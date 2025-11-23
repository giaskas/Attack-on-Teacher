using System.Diagnostics;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    //process instant effects (take damage, heal)

    //process timed effects (poison, build UPS)

    //process static effects (adding/removing buffs from equipment)

    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void ProcessInstantEffects(InstantCharacterEffects effect)
    {
        effect.ProcessEffect(character);
    }
}
