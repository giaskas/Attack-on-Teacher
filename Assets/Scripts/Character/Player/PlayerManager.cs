using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : CharacterManager

{

    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;

    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    protected override void Awake()
    {
        base.Awake();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
    }
    protected override void Update()
    {
        base.Update();
        //si no eres dueño del gameobject, no lo puedes editar ni controlar
        if (!IsOwner)
        {
            return;
        }
        playerLocomotionManager.HandleAllMovement();
        playerStatsManager.RegenerateStamina();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            
            PlayerCamara.instance.player = this;
            PlayerInputManager.instance.player = this;
            WorldSaveManager.instance.player = this;

            //cambiar el total de stamina o vida cuando los stats son cambiados
            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;


            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetHealthValue;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetStaminaValue;
            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;


        }

        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.ChechHP;

    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDamageAnimation = false)
    {
        if (IsOwner)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
        }
        return base.ProcessDeathEvent(manuallySelectDamageAnimation);
        
    }


    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.positionX = transform.position.x;
        currentCharacterData.positionY = transform.position.y;
        currentCharacterData.positionZ = transform.position.z;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
        currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        currentCharacterData.endurance= playerNetworkManager.endurance.Value;
    }
    
    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData,bool loadPosition)
    {
        

        playerNetworkManager.characterName.Value = currentCharacterData.characterName;

            Vector3 myPosition = new Vector3(currentCharacterData.positionX, currentCharacterData.positionY, currentCharacterData.positionZ);
            transform.position = myPosition; 
        


            playerNetworkManager.vitality.Value= currentCharacterData.vitality;
            playerNetworkManager.endurance.Value=currentCharacterData.endurance;
            
            playerNetworkManager.maxHealth.Value=playerStatsManager.CalculateHealthBasedOnHealthLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        
    }
}