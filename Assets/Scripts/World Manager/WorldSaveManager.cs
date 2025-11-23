using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;
using Unity.Netcode;
using NUnit.Framework;

public class WorldSaveManager : MonoBehaviour
{
    public static WorldSaveManager instance;

    public PlayerManager player;


    [Header("Save/Load")]
    [SerializeField] private bool saveGame;
    [SerializeField] private bool loadGame;
    private bool isNewGame = false;


    // Cambiamos el índice por un string para el nombre
    [SerializeField] string worldSceneName = "Scene_World_01";

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;
    public int worldSceneIndex = 1;

    [Header("Current Character Save Data")]
    public CharacterSlot currentCharacterSlot;
    public CharacterSaveData currentCharacterData;
    private string fileName;


    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;
    

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
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }
    public string DecideFileNameBasedOnCharacterSlot(CharacterSlot characterSlot)
    {
        string fileName = "";

        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "characterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "characterSlot_04";
                break;

        }
        return fileName;
    }

    public void CreateNewGame()
    {
        isNewGame= true;

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_01);
        if(!saveFileDataWriter.CheckToSeeIfSaveFileExists())
        {
            startAsAHost();
            currentCharacterSlot = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            NewGame(); 
            return;

        }
        

        //crear un nuevo archivo, dependiendo del nombre del archivo del slot que se eligio
        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_02);
        if (!saveFileDataWriter.CheckToSeeIfSaveFileExists())
        {
            startAsAHost();
            currentCharacterSlot = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            NewGame(); 
            return;

        }
        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_03);
        
        if (!saveFileDataWriter.CheckToSeeIfSaveFileExists())
        {
            startAsAHost();
            currentCharacterSlot = CharacterSlot.CharacterSlot_03;
            currentCharacterData = new CharacterSaveData();
            NewGame(); 
            return;

        }

        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_04);
        if (!saveFileDataWriter.CheckToSeeIfSaveFileExists())
        {
            startAsAHost(); 
            currentCharacterSlot = CharacterSlot.CharacterSlot_04;
            currentCharacterData = new CharacterSaveData();
            NewGame(); 
            return;


        }
        TittleScreenManager.instance.DisplayNoFreeCharacterSlotsMessage();


    }

    private void NewGame()
    {

        SaveGame();
        StartCoroutine(LoadWorldScene()); 





    }
    public void LoadGame()
    {
        //cargar un archivo anterior, dependiendo del nombre del archivo del slot que se eligio
        
        isNewGame = false;
        fileName = DecideFileNameBasedOnCharacterSlot(currentCharacterSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
        saveFileDataWriter.saveFileName = fileName;
        

        currentCharacterData = saveFileDataWriter.LoadSaveFile();



        StartCoroutine(LoadWorldScene());
        
    }

    public void SaveGame()
    {
        //guardar el archivo actual, dependiendo del nombre del archivo del slot que se esta usando
        fileName = DecideFileNameBasedOnCharacterSlot(currentCharacterSlot);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = fileName;

        //pasar los datos del jugador, del juego al archivo de guardado
        if (player != null)
        {
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
        }
        else
        {
            // Si player es null, estamos creando un juego nuevo desde el menú.
            // No hacemos nada, simplemente usamos los datos que ya están en 'currentCharacterData'
            // (los que creamos en NewGame() o LoadGame())
            Debug.Log("Guardando nuevo archivo de juego (Jugador aún no instanciado)");
        }
    
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);

    }
    
    public void DeleteGame(CharacterSlot characterSlot)
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(characterSlot);


        saveFileDataWriter.DeleteSaveFile();
    }
    
    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();
        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();
        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();
        saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlot(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

    }

    public IEnumerator LoadWorldScene()
    {
        if (!NetworkManager.Singleton.IsListening)
        {
            startAsAHost();            
            // Esperamos un frame para asegurar que el Host inicie y el SceneManager se cree
            yield return null; 
        }
        NetworkManager.Singleton.SceneManager.LoadScene(worldSceneName, LoadSceneMode.Single);
        yield return new WaitUntil(() => player != null);
      
            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData, isNewGame);
            player.playerNetworkManager.vitality.Value = 15;  
            player.playerNetworkManager.endurance.Value = 10;
        

        

        yield return null;
    } 

    public void startAsAHost()
    {
        NetworkManager.Singleton.StartHost();

    }
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }

    
     
}