using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] UI_StatBar healthBar;
    [SerializeField] UI_StatBar staminaBar;
    
    [Header("Quick SLots")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }
    public void SetHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);

    }
    public void SetMaxHealthValue(int maxValue)
    {
        healthBar.SetMaxStat(maxValue);

    }
    public void SetStaminaValue(float  oldValue, float  newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue));

    }
    public void SetMaxStaminaValue(int maxValue)
    {
        
        staminaBar.SetMaxStat(maxValue);

    }

    public void SetRightWeaponQuickSlotIcon(int weaponID)
    {


        ItemWeapons weapon = WorldItemDataBase.Instance.GetWeaponID(weaponID);

        if(WorldItemDataBase.Instance.GetWeaponID(weaponID) == null)
        {
            Debug.Log("item es nulo");
            rightWeaponQuickSlotIcon.enabled= false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;  
        }
        if(weapon.itemIcon == null)
        {
            Debug.Log("item no tiene icon");
            rightWeaponQuickSlotIcon.enabled= false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;  
        }
        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;
    }
    public void SetLeftWeaponQuickSlotIcon(int weaponID)
    {


        ItemWeapons weapon = WorldItemDataBase.Instance.GetWeaponID(weaponID);

        if(WorldItemDataBase.Instance.GetWeaponID(weaponID) == null)
        {
            Debug.Log("item es nulo");
            leftWeaponQuickSlotIcon.enabled= false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;  
        }
        if(weapon.itemIcon == null)
        {
            Debug.Log("item no tiene icon");
            leftWeaponQuickSlotIcon.enabled= false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;  
        }

        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;
       
    }
   
}
