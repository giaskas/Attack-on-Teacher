using UnityEngine;

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
        
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);


    }

}
