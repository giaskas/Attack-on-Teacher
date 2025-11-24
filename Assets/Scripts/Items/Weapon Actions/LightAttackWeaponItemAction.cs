using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Action/Light Attack Action")]

public class LightAttackWeaponItemAction : WeaponItemAction
{

    [SerializeField] string light_Attack_01="Main_Light_Attack_01";
    public override void AttemptToPerformAction(PlayerManager playerPerfomingAction, ItemWeapons weaponPerformingAction)
    {
        
        base.AttemptToPerformAction(playerPerfomingAction, weaponPerformingAction);
        if(!playerPerfomingAction.IsOwner)
            return;
        if(playerPerfomingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;
        if(playerPerfomingAction.isPerformingAction)
            return;

        if(!playerPerfomingAction.isGrounded)
            return;
        PerformLightAttack(playerPerfomingAction,weaponPerformingAction);

        
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, ItemWeapons weaponPerformingAction)
    {
        
        if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.LightAttack01,light_Attack_01,true);
        }
        else if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
        {
            
        }
    }
}
