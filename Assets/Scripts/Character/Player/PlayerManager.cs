using UnityEngine;

public class PlayerManager : CharacterManager

{
    
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
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
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //si es el host
        if (IsOwner)
        {
            PlayerCamara.instance.player = this;
            PlayerInputManager.instance.player = this;
        }
    }
}
