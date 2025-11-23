using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager instance;

    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;

    [SerializeField] List<InstantCharacterEffects> instantEffects;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GenerateEffects();
    }

    private void GenerateEffects()
    {
        for(int i=0; i<instantEffects.Count; i++)
        {
            instantEffects[i].instantEffectID = i;
        }
    }

}
