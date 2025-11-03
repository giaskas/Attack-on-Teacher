using UnityEngine;
using Unity.Netcode;

// ¡ASEGÚRATE DE QUE HEREDE DE NETWORKBEHAVIOUR!
public class PlayerSpawnManager : NetworkBehaviour
{
    [Header("Player Prefab")]
    [Tooltip("El prefab del jugador que se va a crear")]
    [SerializeField] private GameObject playerPrefab;

    [Header("Spawn")]
    [Tooltip("Punto donde aparecerá el jugador. Si se deja vacío, usará (0,0,0)")]
    [SerializeField] private Transform spawnPoint;

    // Esto se llama automáticamente en el servidor cuando el script inicia
    public override void OnNetworkSpawn()
    {
        // Nos aseguramos de que esto solo lo ejecute el SERVIDOR (Host)
        if (!IsServer)
        {
            return;
        }

        // --- Spawnea al Host (que también es un cliente) ---
        // El Host no dispara OnClientConnectedCallback para sí mismo al inicio,
        // así que lo spawneamos manualmente.
        SpawnPlayer(NetworkManager.Singleton.LocalClientId);

        // --- Escucha conexiones de NUEVOS clientes ---
        // Suscríbete al evento que se dispara CADA VEZ que un cliente TERMINA de conectarse
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        // No queremos spawnear al Host dos veces.
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            return;
        }
        
        // Alguien se conectó, spawnea un jugador para él
        SpawnPlayer(clientId);
    }

    private void SpawnPlayer(ulong clientId)
    {
        Debug.Log($"Spawneando jugador para el cliente: {clientId}");

        // 1. Define la posición de spawn
        Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.zero;

        // 2. Crea la instancia del prefab
        GameObject playerInstance = Instantiate(playerPrefab, position, Quaternion.identity);

        // 3. Obtiene el NetworkObject del jugador
        NetworkObject netObject = playerInstance.GetComponent<NetworkObject>();

        // 4. ¡LA MAGIA! Spawnea el objeto en la red y le da "propiedad" (ownership)
        // al cliente que se acaba de conectar (usando su clientId).
        netObject.SpawnAsPlayerObject(clientId);
    }

    // Es buena práctica des-suscribirse del evento al destruir el objeto
    public override void OnNetworkDespawn()
    {
        if (IsServer && NetworkManager.Singleton != null) // Comprobación extra por si el NetworkManager ya no existe
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        }
    }
}