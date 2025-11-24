using UnityEngine;


[CreateAssetMenu(menuName ="Character Actions/Weapon Action/Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;

    public virtual void AttemptToPerformAction (PlayerManager playerPerfomingAction, ItemWeapons weaponPerformingAction)
    {
        if(playerPerfomingAction.IsOwner)
        {
            playerPerfomingAction.playerNetworkManager.currentWeaponBeingUsed.Value= weaponPerformingAction.itemID;
        }
    }   
}
