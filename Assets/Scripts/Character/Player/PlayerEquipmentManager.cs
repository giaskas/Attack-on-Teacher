using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{

    PlayerManager player;
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();    

        InitializeWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();

        LoadWeaponOnBothHands();

    }

    private void InitializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach(var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands()
    {
        LoadRightWeapon();
    }

    public void SwitchRightWeapon()
{
    if (!player.IsOwner)
        return;

    // 1. Ejecutar Animación
    player.playerAnimatorManager.PlayerTargetActionAnimation("Switch_Weapon_01", true, false, true, true, "RightHand Override");

    // 2. Obtener índice actual
    int currentIndex = player.playerInventoryManager.rightHandedWeaponIndex;
    
    // Variable para guardar el indice de la siguiente arma. 
    // Lo iniciamos en -1 (Manos Vacías) por defecto.
    int nextWeaponIndex = -1; 

    ItemWeapons[] weapons = player.playerInventoryManager.weaponsInRigthHandSlots;

    // 3. Buscar arma en los slots SIGUIENTES al actual
    // (Ejemplo: Si tienes la espada en el slot 0, busca en el 1 y el 2)
    for (int i = currentIndex + 1; i < weapons.Length; i++)
    {
        if (weapons[i].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
        {
            nextWeaponIndex = i;
            break; // ¡Encontramos la siguiente arma! Dejamos de buscar.
        }
    }

    // 4. Caso Especial: Si estamos en Manos Vacías (-1) y no encontramos nada arriba...
    // Significa que tenemos que buscar desde el principio (Slot 0)
    if (currentIndex == -1 && nextWeaponIndex == -1)
    {
         for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
            {
                nextWeaponIndex = i;
                break; // Encontramos la primera arma de la lista
            }
        }
    }

    // 5. Aplicar el cambio
    if (nextWeaponIndex != -1)
    {
        // Encontramos una espada nueva
        player.playerInventoryManager.rightHandedWeaponIndex = nextWeaponIndex;
        player.playerNetworkManager.currentRightHandWeaponID.Value = weapons[nextWeaponIndex].itemID;
    }
    else
    {
        // No encontramos nada más adelante en la lista, así que toca GUARDAR el arma (ir a -1)
        player.playerInventoryManager.rightHandedWeaponIndex = -1;
        player.playerNetworkManager.currentRightHandWeaponID.Value = WorldItemDataBase.Instance.unarmedWeapon.itemID;
    }
}

    public void SwitchLeftWeapon()
    {
        
    }


    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            rightHandSlot.UnloadWeapon();
            rightHandWeaponModel= Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

        }
    }


    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            leftHandSlot.UnloadWeapon();
            leftHandWeaponModel= Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();

            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);

        }
    }
    public void OpenDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
        }
        else if(player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();

        }

        //reproducir CLAAAANKKKKKK (espada) sonido
    }
    public void CloseDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
        }
        else if(player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();

        }

        //reproducir CLAAAANKKKKKK (espada) sonido
    }

   

}
