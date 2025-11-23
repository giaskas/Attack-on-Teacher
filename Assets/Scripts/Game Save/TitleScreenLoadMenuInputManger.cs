using System.Linq;
using UnityEngine;

public class TitleScreenLoadMenuInputManger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerControll playerControl;

    [Header ("Title Screen Inputs")]
    [SerializeField]bool deleteCharacterSlot = false;

    private void Update()
    {
        if (deleteCharacterSlot)
        {
            deleteCharacterSlot=false;
            TittleScreenManager.instance.AttemptToDeleteCharacterSlot();

        }
    }
    private void OnEnable()
    {
        if (playerControl == null)
        {
            playerControl = new PlayerControll();
            playerControl.UI.X.performed += i => deleteCharacterSlot = true;
        }
        playerControl.Enable();

    }

    private void OnDisable()
    {
        playerControl.Disable();

    }

}
