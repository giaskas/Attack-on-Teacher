using UnityEngine;
using Unity.Netcode;
public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager player;

    [Header("Equipment")]
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLefttHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


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

    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
    {
        ItemWeapons newWeapon = Instantiate(WorldItemDataBase.Instance.GetWeaponID(newID));
        player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        ItemWeapons newWeapon = Instantiate(WorldItemDataBase.Instance.GetWeaponID(newID));
        player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
    //    player.playerEquipmentManager.LoadLeftWeapon();

    }
}
