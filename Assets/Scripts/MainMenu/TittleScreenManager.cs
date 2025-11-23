using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TittleScreenManager : MonoBehaviour
{

    public static TittleScreenManager instance;
    
    [Header("Start Screen")]
    [SerializeField] GameObject pressAnyButtonScreen; // Arrastra aquí tu panel de "Attack on Teacher"
    [SerializeField] GameObject mainMenuContainer;    // Arrastra aquí el objeto padre de los botones del menú
    private bool isGameStarted = false; // Para controlar que no se active doble vez

    [Header("Buttons")]
    [SerializeField]  Button createGameButton;
    [SerializeField]  Button joinButton;
    [SerializeField]  Button loadGameButton;
    [SerializeField]  Button loadMenuReturnButton;
    [SerializeField]  Button mainMenuLoadButton;
   [SerializeField]  Button deleteCharacterPopUpConfirmButton;

    [Header("Menus")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject LoadMenu;

    [Header("Pop ups")]
    [SerializeField] GameObject noFreeCharacterSlotsPopUp;
    [SerializeField] Button noFreeCharacterSlotsPopUpOkayeButton;
    [SerializeField] GameObject deleteCharacterSlotSlotPopUp;


   [Header("Save Slots")]
   public  CharacterSlot currentSelectedSlot=CharacterSlot.NO_SLOT;


  

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Al iniciar el juego:
        // 1. Aseguramos que la pantalla de "Press Any Button" esté activa
        if(pressAnyButtonScreen != null) pressAnyButtonScreen.SetActive(true);
        
        // 2. Aseguramos que el Menú Principal esté oculto al principio
        if(mainMenuContainer != null) mainMenuContainer.SetActive(false);
    }

    private void Update()
    {
        // Si el juego AÚN NO ha pasado la pantalla de título...
        if (!isGameStarted)
        {
            // Detectamos si presionan cualquier tecla (Teclado) o botones principales (Gamepad)
            if (Keyboard.current.anyKey.wasPressedThisFrame || 
               (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame) || // Botón A / X
               (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame) ||  // Botón Start
                Input.GetMouseButtonDown(0)) // Clic izquierdo ratón
            {
                TransitionToMainMenu();
            }
        }
    }

    private void TransitionToMainMenu()
    {
        isGameStarted = true;

        // 1. Apagamos la pantalla de título
        if(pressAnyButtonScreen != null) pressAnyButtonScreen.SetActive(false);

        // 2. Prendemos el menú principal
        if(mainMenuContainer != null) mainMenuContainer.SetActive(true);

        // 3. ¡CRUCIAL! Le decimos al control: "Tu nuevo foco es este botón"
        // Sin esto, el control se muere hasta que toques el mouse.
        createGameButton.Select();
    }
    public void StartNetworkAsHost()
    {
        

        // ¡¡AÑADE ESTA LÍNEA AQUÍ!!
        // Ahora, tan pronto como seas Host, cargarás la escena.
        WorldSaveManager.instance.CreateNewGame();
        
    }

    public void StartNetworkAsClient()
    {
        NetworkManager.Singleton.StartClient();
        // El cliente no carga la escena, espera a que el Host le diga.
    }

    
   

    public void OpenLoadMenu()
    {
        //cerrar main menu y abrir load menu
        titleScreenMainMenu.SetActive(false);
        LoadMenu.SetActive(true);
        loadMenuReturnButton.Select();
    }
    public void CloseLoadMenu()
    {
        //cerrar load menu y abrir main menu
        LoadMenu.SetActive(false);
        titleScreenMainMenu.SetActive(true);
        mainMenuLoadButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsMessage()
    {
        noFreeCharacterSlotsPopUp.SetActive(true);
        noFreeCharacterSlotsPopUpOkayeButton.Select();
    }

    public void CloseNoFreeCharacterSlotsMessage()
    {
        noFreeCharacterSlotsPopUp.SetActive(false);
        createGameButton.Select();
    }

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if(currentSelectedSlot != CharacterSlot.NO_SLOT)
        {
            deleteCharacterSlotSlotPopUp.SetActive(true);
            deleteCharacterPopUpConfirmButton.Select();
        }


    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotSlotPopUp.SetActive(false);
        WorldSaveManager.instance.DeleteGame(currentSelectedSlot);
        LoadMenu.SetActive(false);
        LoadMenu.SetActive(true);
        loadMenuReturnButton.Select();
    }

    public void CloseDeleteCharacterPopUp()
    {
        deleteCharacterSlotSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }

}