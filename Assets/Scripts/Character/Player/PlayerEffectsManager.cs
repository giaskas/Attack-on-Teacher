using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("debub delete later")]
    [SerializeField] InstantCharacterEffects effectsToTest;
    [SerializeField] bool ProcessEffect = false;

    private void Update()
    {
        if (ProcessEffect)
        {
            ProcessEffect= false;
            InstantCharacterEffects effect = Instantiate(effectsToTest);
            ProcessInstantEffects(effect);
        }
    }
}
