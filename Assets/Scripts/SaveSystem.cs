using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveProgress(PlayerProgress playerProgress)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerProgress.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerProgress);
        stream.Close();
    }

    public static PlayerProgress LoadProgress()
    {
        string path = Application.persistentDataPath + "/playerProgress.fun";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerProgress data = formatter.Deserialize(stream) as PlayerProgress;
            stream.Close();

            return data;
        } else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSaves()
    {
        string path = Application.persistentDataPath + "/playerProgress.fun";

        if (File.Exists(path))
        {
            File.Delete(path);
            if (File.Exists(path))
            {
                Debug.Log("File didn't get Deleted");
            }
            //BinaryFormatter formatter = new BinaryFormatter();
            //FileStream stream = new FileStream(path, FileMode.Open);
            //formatter.
        }
    }
}
