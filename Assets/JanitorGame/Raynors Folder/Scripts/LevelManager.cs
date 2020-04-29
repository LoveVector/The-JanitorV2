using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public VoiceActing beginningVoice;
    public Text text;
    public VoiceActing currentVoice;

    public Wave[] waves;
    Wave currentWave;

    public Transform[] spawnLocations;

    public int wavePoint = 0;

    float lastWaveEndTime = 0;
    public float waveRate;

    AudioSource source;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine("BeginningVoiceActing");
        currentWave = waves[wavePoint];
    }

    // Update is called once per frame
    void Update()
    {
        VoiceAct();
        Waves();
    }

    public void VoiceAct()
    {
        if (currentVoice != null)
        {
            if (currentVoice.finished == false)
            {
                currentVoice.DoIt(text, source);
            }
            else
            {
                text.text = " ";
            }
        }
    }

    void Waves()
    {
        if (Time.time >= lastWaveEndTime)
        {
            currentWave.StartWave(spawnLocations, this);
        }
        if(currentWave.end == true && wavePoint < waves.Length)
        {
            currentWave = waves[wavePoint];
            wavePoint++;
            lastWaveEndTime = Time.time + waveRate;
        }
    }

    public void Spawn(GameObject enemy,Transform spawnPosition)
    {
        GameObject enemy1 = Instantiate(enemy, spawnPosition);
        EnemyAbstract enemyCode = enemy1.GetComponent<EnemyAbstract>();
        enemyCode.player = player;
        enemyCode.level = this;
    }

    public void DeadEnemy()
    {
        currentWave.spawnNext = true;
        currentWave.enemiesKilled++;
    }

    public void WaveVoiceAct(VoiceActing voice)
    {
        currentVoice = voice;
    }

    IEnumerator BeginningVoiceActing()
    {
        yield return new WaitForSeconds(2f);
        currentVoice = beginningVoice;
    }
}
