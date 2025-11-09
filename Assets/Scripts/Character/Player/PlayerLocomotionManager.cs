using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    // 1. SOLUCIÓN: El tipo debe ser 'PlayerManager'
    PlayerManager player;
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;


    [Header ("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningspeed = 5;
    [SerializeField] float sprintingSpeed = 7;

    [SerializeField] float rotationSpeed = 15;

    [Header("Dodge")]
    private Vector3 rollDirection;

    protected override void Awake()
    {
        // Esta parte estaba bien si CharacterLocomotionManager tiene un Awake()
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    protected override void Update()
    {
        base.Update();
        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
        
    }
    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
            return;
        GetMovementValues();

        // 2. SOLUCIÓN: Usar 'Camera.main' y corregir 'forward'
        moveDirection = Camera.main.transform.forward * verticalMovement;
        moveDirection = moveDirection + Camera.main.transform.right * horizontalMovement;

        // Esta línea está BIEN en el código que pegaste (en la captura tenías 'Normalized')
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);


        }
        else
        {
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * runningspeed * Time.deltaTime);

            }
            else if (PlayerInputManager.instance.moveAmount > 0f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }
    }


        // 3. SOLUCIÓN: Lógica corregida para caminar/correr

    private void HandleRotation()
    {
        if (!player.canRotate)
            return;
            
        targetRotationDirection = Vector3.zero;

        targetRotationDirection = PlayerCamara.instance.cameraObject.transform.forward * verticalMovement;

        targetRotationDirection = targetRotationDirection + PlayerCamara.instance.cameraObject.transform.right * horizontalMovement;

        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
            return;

        if (PlayerInputManager.instance.moveAmount > 0)
        {
            rollDirection = PlayerCamara.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            rollDirection += PlayerCamara.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            rollDirection.y = 0;
            rollDirection.Normalize();


            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayerTargetActionAnimation("RollFoward_01", true, true);

        }
        else
        {
            //roll para atras
        }

    }

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }


        if(moveAmount > 0.5f)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
        


    }


}