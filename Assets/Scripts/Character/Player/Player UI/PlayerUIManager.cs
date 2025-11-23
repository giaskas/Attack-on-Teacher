using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [HideInInspector] public PlayerUIHudManager playerUIHudManager;
    [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }
        playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        playerUIPopUpManager= GetComponentInChildren<PlayerUIPopUpManager>();


    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    private void Update()
    {
        
    }
}
