using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDataBase : MonoBehaviour
{
    
    public static WorldItemDataBase Instance ;

    public ItemWeapons unarmedWeapon; 
    public ItemWeapons unarmedWeaponLeftHand;
    [SerializeField] List<ItemWeapons> weapons = new List<ItemWeapons>();

    [SerializeField] List<Items> items = new List<Items>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
            
        }

        foreach(var weapon in weapons)
        {
            items.Add(weapon);
        }

        for(int i =0 ; i<items.Count; i++)
        {
            items[i].itemID=i;
        }
    }

    public ItemWeapons GetWeaponID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID==ID);
    }

}
