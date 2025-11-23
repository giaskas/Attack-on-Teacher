using UnityEngine;
using System.IO;
using System;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool CheckToSeeIfSaveFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        if (CheckToSeeIfSaveFileExists())
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }
    }

    public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creando juego guardado en: " +savePath);

            //serializar el objeto de Unity a json
            string dataToStore = JsonUtility.ToJson(characterSaveData, true);
            //escribir el json en un archivo
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.Write(dataToStore);
                    Debug.Log("Juego guardado creado correctamente.");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al crear el archivo de guardado en: " + savePath + "\n error: " + ex.Message);
        }
    }

    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterSave = null;
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                //leer el archivo
                string dataToLoad = "";
                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader fileReader = new StreamReader(stream))
                    {
                        dataToLoad = fileReader.ReadToEnd();
                    }
                }

                //deserializar el json para convertirlo a Unity otra vez
                characterSave = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error al cargar el archivo de guardado en: " + loadPath + "\n error: " + ex.Message);
            }
        }
        return characterSave;

    }


}
