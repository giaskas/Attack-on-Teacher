using UnityEngine;
using Unity.Netcode;
public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    public void SetNewMaxHealthValue(int oldVitality, int newVitality)
    {
        maxHealth.Value = player.playerStatsManager.CalculateHealthBasedOnHealthLevel(newVitality);
        PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }
    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = player.playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }
}
