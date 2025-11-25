using UnityEngine;
[CreateAssetMenu(menuName = "Character Actions/Weapon Action/Heavy Attack Action")]

public class HeavyAttackWeaponItemAction : WeaponItemAction
{
     [SerializeField] string heavy_Attack_01="Main_Heavy_Attack_01";
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
        PerformHeavyAttack(playerPerfomingAction,weaponPerformingAction);

        
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, ItemWeapons weaponPerformingAction)
    {
        
        if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.HeavyAttack01,heavy_Attack_01,true);
        }
        else if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
        {
            
        }
    }
}
