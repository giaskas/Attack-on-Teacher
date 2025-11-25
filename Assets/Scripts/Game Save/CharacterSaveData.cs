using UnityEngine;


[System.Serializable]
public class CharacterSaveData
{
    [Header("Nombre del personaje")]
    public string characterName ="Character";

    
    [Header("Tiempo jugado")]
    public float secondsPlayer;


    [Header("Coordenadas del mundo")]
    public float positionX= 0;
    public float positionY=-25; 
    public float positionZ= 0;

    [Header("Resources")]
    public int currentHealth;
    public float currentStamina;

    [Header("Stats")]
    public int vitality=10;
    public int endurance=10;
    
}
