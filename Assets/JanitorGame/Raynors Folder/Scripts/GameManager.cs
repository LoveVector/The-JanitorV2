using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int akAmmo;
    public int shottyAmmo;
    public static GameManager Instance { get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        Load();
    }

    // Update is called once per frame
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream stream = new FileStream(Application.persistentDataPath + "/witcher3.sav", FileMode.Create);

        SavableData data = new SavableData(this);

        bf.Serialize(stream, data);
        stream.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/witcher3.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream stream = new FileStream(Application.persistentDataPath + "/witcher3.sav", FileMode.Open);

            SavableData data = (SavableData)bf.Deserialize(stream);

            stream.Close();

            akAmmo = data.savedAkAmmo;
            shottyAmmo = data.savedShottyAmmo;
        }
        else
        {
            akAmmo = 200;
            shottyAmmo = 8;
        }
    }

  
[Serializable]
public class SavableData
{
    public int savedAkAmmo;
    public int savedShottyAmmo;

    public SavableData(GameManager manager)
    {
        savedAkAmmo = manager.akAmmo;
        savedAkAmmo = manager.shottyAmmo;
    }
}
}
