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

    // 1. Guardamos el índice actual para referencia
    int currentWeaponIndex = player.playerInventoryManager.rightHandedWeaponIndex;
    
    // 2. Intentamos buscar la siguiente arma válida (probamos máximo 3 veces, una por cada slot)
    for (int i = 1; i < player.playerInventoryManager.weaponsInRigthHandSlots.Length + 1; i++)
    {
        // Calculamos el siguiente índice usando Módulo (%) para que dé la vuelta (0, 1, 2, 0...)
        int potentialIndex = (currentWeaponIndex + i) % player.playerInventoryManager.weaponsInRigthHandSlots.Length;

        // Obtenemos el arma en ese slot potencial
        ItemWeapons potentialWeapon = player.playerInventoryManager.weaponsInRigthHandSlots[potentialIndex];

        // 3. Verificamos si el arma NO es nula y NO es la desarmada
        if (potentialWeapon != null && potentialWeapon.itemID != WorldItemDataBase.Instance.unarmedWeapon.itemID)
        {
            // ¡ENCONTRAMOS UN ARMA VÁLIDA!
            player.playerInventoryManager.rightHandedWeaponIndex = potentialIndex;
            player.playerNetworkManager.currentRightHandWeaponID.Value = potentialWeapon.itemID;
            return; // Salimos de la función, ya cambiamos el arma
        }
    }

    // 4. Si llegamos aquí, significa que revisamos todos los slots y NO había armas válidas (solo Unarmed).
    // Entonces sí, equipamos Unarmed y ponemos índice -1.
    if (player.playerInventoryManager.rightHandedWeaponIndex != -1)
    {
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
            rightHandWeaponModel= Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

        }
    }

   

}
