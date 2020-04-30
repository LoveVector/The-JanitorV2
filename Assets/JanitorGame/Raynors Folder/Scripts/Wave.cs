using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave 
{
    public GameObject[] enemyTypes;
    public int amountOfEnemies;
    public int firstSpawn;
    public int spawnAmount;
    public int enemiesKilled;
    
    int enemiesSpawned = 0;
    bool beginning = true;
    public bool end = false;
    int spawn = 0;

    public VoiceActing waveVoice;

    public bool spawnNext;

    public void StartWave(Transform[] spawnPositions)
    {
        int type = Random.Range(enemyTypes.Length - enemyTypes.Length, enemyTypes.Length);
        if(beginning == true)
        {
            for (int i = 0; i < firstSpawn; i++)
            {
                LevelManager.Instance.Spawn(enemyTypes[type], spawnPositions[spawn]);
                enemiesSpawned++;
                spawn++;
                if(spawn == spawnPositions.Length)
                {
                    spawn = 0;
                }
               if(enemiesSpawned == amountOfEnemies)
               {
                   return;
               }
            }
            beginning = false;
        }

        if(enemiesSpawned != amountOfEnemies && spawnNext == true)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                LevelManager.Instance.Spawn(enemyTypes[type], spawnPositions[spawn]);
                enemiesSpawned++;
                spawn++;
                if (spawn == spawnPositions.Length)
                {
                    spawn = 0;
                }
              if (enemiesSpawned == amountOfEnemies)
              {
                  return;
              }
            }
            spawnNext = false;
        }

        if(enemiesSpawned == enemiesKilled)
        {
            if (waveVoice != null)
            {
                LevelManager.Instance.currentVoice =  waveVoice;
            }
            end = true;
        }
    }
}
