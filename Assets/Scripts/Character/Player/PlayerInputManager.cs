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




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mover el DontDestroyOnLoad aquí es más seguro
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Suscribirse al evento de cambio de escena
        SceneManager.activeSceneChanged += OnSceneChange;

        // Comprobar la escena actual AL INICIAR
        // Para que el input funcione si empezamos directo en la escena del mundo
        CheckSceneAndToggleInput(SceneManager.GetActiveScene());
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControll();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamara.Movement.performed += i => camaraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.Dodge.performed += instance => dodgeInput = true;
        }

        // 2. SOLUCIÓN: Activar el mapa de acción específico
        playerControls.PlayerMovement.Enable();
    }

    // Se llamará cuando el script sea deshabilitado (instance.enabled = false)
    private void OnDisable()
    {
        // Buena práctica: deshabilitar el mapa de acción cuando el script se deshabilita
        if (playerControls != null)
        {
            playerControls.PlayerMovement.Disable();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        CheckSceneAndToggleInput(newScene);
    }

    // 3. SOLUCIÓN: Lógica centralizada para habilitar/deshabilitar
    private void CheckSceneAndToggleInput(Scene scene)
    {
        // Asumiendo que 'WorldSaveManager' está listo
        if (WorldSaveManager.instance != null)
        {
            if (scene.buildIndex == WorldSaveManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true; // Esto llamará a OnEnable()
            }
            else
            {
                instance.enabled = false; // Esto llamará a OnDisable()
            }
        }
        else if (scene.buildIndex == 0) // Fallback si es la escena 0 (Menú)
        {
            instance.enabled = false;
        }
        else // Fallback si no hay save manager pero no es el menú
        {
            instance.enabled = true;
        }
    }

    private void OnDestroy()
    {
        // Limpiar la suscripción
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
        HandleDodgeInput();
    }

    //movimiento
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

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
    }

    private void HandleCamaraMovementInput()
    {
        camaraVerticalInput = camaraInput.y;
        camaraHorizontalInput = camaraInput.x;
    }
    
    
    //acciones
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();

        }
    }

}