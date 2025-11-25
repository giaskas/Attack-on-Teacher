using UnityEngine;
using Unity.Netcode;
public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    public ItemWeapons currentWeaponBeingUsed;
    protected override void Awake()
    {
        base.Awake();
        player =GetComponent<PlayerManager>();

    }
    

    public void PerformActionBasedAction(WeaponItemAction weaponAction, ItemWeapons weaponPerformingAction)
    {
        if(currentWeaponBeingUsed==null)
            return;
        if (player.IsOwner)
        {
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId,weaponAction.actionID,weaponPerformingAction.itemID);
        
        }
        
    }

    public virtual void DrainStaminaBaseOnAttack()
    {

        if (!player.IsOwner)
            return;
        if(currentWeaponBeingUsed==null)
            return;

        float staminaDeducted=0;
        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.light_Attack_01_Modifier;
                break;
            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavy_Attack_01_Modifier;
                break;
            case AttackType.ChargedAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.charge_Attack_01_Modifier;
                break;
            default:

                break;
        }

        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);

    }

}
