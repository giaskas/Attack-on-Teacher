using UnityEngine;
using UnityEngine.SceneManagement; // Sigue necesitando esto
using System.Collections;
using Unity.Netcode; // ¡AÑADE ESTO!

public class WorldSaveManager : MonoBehaviour
{
    public static WorldSaveManager instance;
    
    // Cambiamos el índice por un string para el nombre
    [SerializeField] string worldSceneName = "Scene_World_01"; 
    [SerializeField] int worldSceneIndex = 1; 

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
    }

    // Ya no necesita ser una Corrutina (IEnumerator)
    public void LoadNewGame()
    {
        // Esta es la línea clave:
        // Carga la escena usando Netcode para que se sincronice
        // con todos los clientes.
        NetworkManager.Singleton.SceneManager.LoadScene(worldSceneName, LoadSceneMode.Single);
    }
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}