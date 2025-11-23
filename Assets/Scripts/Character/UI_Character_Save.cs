using UnityEngine;
using TMPro;

public class UI_Character_Save : MonoBehaviour
{
    SaveFileDataWriter saveFileDataWriter;

    [Header("Character Slot")]
    public CharacterSlot characterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timedPlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }
    
    private void LoadSaveSlots()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        if (characterSlot == CharacterSlot.CharacterSlot_01)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_02)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot02.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_03)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot03.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_04)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot04.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_05)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot05.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_06)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot06.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_07)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot07.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_08)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot08.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_09)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot09.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }else if (characterSlot == CharacterSlot.CharacterSlot_10)
        {
            saveFileDataWriter.saveFileName = WorldSaveManager.instance.DecideFileNameBasedOnCharacterSlot(characterSlot);
        
            if (saveFileDataWriter.CheckToSeeIfSaveFileExists())
            {
                
                characterName.text = WorldSaveManager.instance.characterSlot10.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void LoadGameFromCharacterSlot()
    {
        WorldSaveManager.instance.currentCharacterSlot = characterSlot;
        WorldSaveManager.instance.LoadGame();
    }


    public void SelectCurrentSlot()
    {
        TittleScreenManager.instance.SelectCharacterSlot(characterSlot);
    }
} 