using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;

    PlayerControll playerControls;
    [Header("Camara movement input")]
    [SerializeField] Vector2 camaraInput;
    public float camaraHorizontalInput;
    public float camaraVerticalInput;


    [Header("Player movement Input")]
    [SerializeField] Vector2 movementInput;

    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

    [Header("Player Actions Input")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool RB_Input = false;

    [Header("Trigger Inputs")]
    [SerializeField] bool RT_Input=false;
    [SerializeField] bool Hold_RT_Input= false;

    [SerializeField] bool Switch_Left_Weapon= false;
    [SerializeField] bool Switch_Right_Weapon= false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
     
        SceneManager.activeSceneChanged += OnSceneChange;

 
        CheckSceneAndToggleInput(SceneManager.GetActiveScene());
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControll();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamara.Movement.performed += i => camaraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;



            playerControls.PlayerActions.RB.performed += i => RB_Input=true;

            playerControls.PlayerActions.RT.performed += i => RT_Input=true;
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => Switch_Right_Weapon=true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => Switch_Left_Weapon=true;

            playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input=true;
            playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input=false;

         
  

            playerControls.PlayerActions.Sprint.performed += i => {
                sprintInput = true;
                
            };
            playerControls.PlayerActions.Sprint.canceled += i => {
                sprintInput = false;
            
            };
        }

        playerControls.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.PlayerMovement.Disable();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        CheckSceneAndToggleInput(newScene);
    }

    private void CheckSceneAndToggleInput(Scene scene)
    {
        if (WorldSaveManager.instance != null)
        {
            if (scene.buildIndex == WorldSaveManager.instance.worldSceneIndex)
            {
                instance.enabled = true; 
            }
            else
            {
                instance.enabled = false;
            }
        }
        else if (scene.buildIndex == 0) 
        {
            instance.enabled = false;
        }
        else 
        {
            instance.enabled = true;
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();

            }
            else
            {
                playerControls.Disable();
            }
        }
    }
    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCamaraMovementInput();
        if (player == null)
        return;
        if(!player.isGrounded)
        return;
        HandleDodgeInput();
        HandleSprinting();
        HandleRBInput();
        HandeRTInput();
        HandeHoldRTInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
    }

    private void HandlePlayerMovementInput()
    {

        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
        if (player == null)
            return;

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
    }

    private void HandleCamaraMovementInput()
    {
        camaraVerticalInput = camaraInput.y;
        camaraHorizontalInput = camaraInput.x;
    }


    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();

        }
    }
    private void HandleSprinting()
    {
        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSprinting();
        }else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleRBInput()
    {
        if (RB_Input)
        {
            RB_Input=false;

            //si hay ui abierto no hacer nada

            player.playerNetworkManager.SetCharacterActionHand(true);

            player.playerCombatManager.PerformActionBasedAction(player.playerInventoryManager.currentRightHandWeapon.RB_Action,player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandeRTInput()
    {
        if (RT_Input)
        {
            RT_Input=false;

            //si hay ui abierto no hacer nada

            player.playerNetworkManager.SetCharacterActionHand(true);

            player.playerCombatManager.PerformActionBasedAction(player.playerInventoryManager.currentRightHandWeapon.RT_Action,player.playerInventoryManager.currentRightHandWeapon);
        }
    }
    private void HandeHoldRTInput()
    {
        if (player.isPerformingAction)
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerNetworkManager.isChargingAttack.Value=Hold_RT_Input;
            }
        }
    }

    private void HandleSwitchRightWeaponInput()
    {

        if (Switch_Right_Weapon)
        {
            Debug.Log("si entro ya minimo");

            Switch_Right_Weapon=false;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftWeaponInput()
    {
        if (Switch_Left_Weapon)
        {
            Debug.Log("si entro ya minimo");

            Switch_Left_Weapon=false;
            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }

}