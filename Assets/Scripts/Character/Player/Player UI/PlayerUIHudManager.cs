using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_StatBar healthBar;

    [SerializeField] UI_StatBar staminaBar;
    
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
   
}
