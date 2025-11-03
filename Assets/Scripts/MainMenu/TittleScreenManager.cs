using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI; 

public class TittleScreenManager : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
        DisableButtons();

        // ¡¡AÑADE ESTA LÍNEA AQUÍ!!
        // Ahora, tan pronto como seas Host, cargarás la escena.
        WorldSaveManager.instance.LoadNewGame(); 
    }

    public void StartNetworkAsClient()
    {
        NetworkManager.Singleton.StartClient();
        DisableButtons();
        // El cliente no carga la escena, espera a que el Host le diga.
    }

    private void DisableButtons()
    {
        if (hostButton != null)
            hostButton.interactable = false;
        
        if (clientButton != null)
            clientButton.interactable = false;
    }

    // Esta función ("StartNewGame") ya no es necesaria
    // porque la estamos llamando desde "StartNetworkAsHost".
    // Puedes borrar el botón que la llamaba.
    public void StartNewGame()
    {
        // ... esta función ya no hace falta.
    }
}