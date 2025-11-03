using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    

    // --- AÑADE ESTA LÍNEA ---
    // Necesitamos una referencia al PlayerManager para pasársela a la cámara
    PlayerManager playerManager; 

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();

        // --- AÑADE ESTA LÍNEA ---
        playerManager = GetComponent<PlayerManager>(); // Obtiene el componente
        
    }

    // --- AÑADE ESTA FUNCIÓN NUEVA ---
    // OnNetworkSpawn se llama cuando este objeto es creado en la red
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn(); // No olvides llamar al método base

        // Si este objeto de jugador me pertenece (es MI jugador, no el de otro)
        if (IsOwner)
        {
            // Busca la instancia de la cámara que ya existe en la escena
            if (PlayerCamara.instance != null)
            {
                // Asígnale ESTE jugador (su componente PlayerManager) a la variable 'player' de la cámara
                PlayerCamara.instance.player = playerManager;
            }
        }
    }

    protected virtual void Update()
    {
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            //... tu código de interpolación
            transform.position = Vector3.SmoothDamp
                (transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp
                (transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {
        if (!IsOwner)
            return;
        
        // Esta línea ahora SÍ funcionará porque 'PlayerCamara.instance.player' ya no es null
        PlayerCamara.instance.HandleAllCameraActions();
    }
}