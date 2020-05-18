using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AStar;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    List<Vector3> waypoints;

    public bool isThereBegVoiceAct;

    public VoiceActing beginningVoice;
    public Text text;
    public VoiceActing currentVoice;

    public Wave[] waves;
    public Wave currentWave;

    public Transform[] spawnLocations;

    public int wavePoint = 0;

    float lastWaveEndTime = 0;
    public float waveRate;

    [HideInInspector] public bool bossWave;

    AudioSource source;

    public bool isThisBoss;

    public GameObject player;
    public GameObject boss;

    BossGOAP bossCode;

    public AGrid grid;
    public APathFinding pathfinding;

    public float gridSizeX;
    public float gridSizeY;
    public float nodeRadius;

    public Transform position;

    public LayerMask unwalkable;

    public bool levelStart;
    public bool levelEnd;

    public int currentScene;

    public Text ammoText;

    public int shottyAmmo;
    public int ak;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        source = GetComponent<AudioSource>();
        if (isThereBegVoiceAct) //if level does not have beginning voicing
        {
            StartCoroutine("BeginningVoiceActing");
        }
        currentWave = waves[wavePoint];

        bossWave = false;

        if(boss != null)
        {
            bossCode = boss.GetComponent<BossGOAP>();
        }

        currentVoice = null;

        grid = new AGrid(this.transform, gridSizeX, gridSizeY, nodeRadius, unwalkable);// set up astar dll
        grid.CreateNodes();
        pathfinding = new APathFinding();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentVoice != null)
        {
            VoiceAct();
        }

        if (levelStart == true)
        {
            Waves();
            grid.NodeChecks();
        }
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
        if (!isThisBoss)
        {
            if (Time.time >= lastWaveEndTime)
            {
                currentWave.StartWave(spawnLocations);
            }
        }
        else if (bossWave == true && isThisBoss)
        {
            currentWave.StartWave(spawnLocations);
        }

        if(currentWave.end == true)
        {
            if (!isThisBoss && wavePoint <= waves.Length)
            {
                if (wavePoint < waves.Length)
                {
                    currentWave = waves[wavePoint];
                    wavePoint++;
                }
                else
                {
                    levelStart = false; 
                    levelEnd = true;
                }
            }
            else if (boss != null)
            {
                int o = Random.Range(0, waves.Length);
                currentWave = waves[o];
                currentWave.end = false;
                bossCode.StartCoroutine("CallBackup");
                bossWave = false;
            }
            lastWaveEndTime = Time.time + waveRate;
        }
    }

    public void Spawn(GameObject enemy,Transform spawnPosition)
    {
        GameObject enemy1 = Instantiate(enemy, spawnPosition.transform.position, Quaternion.identity);
        EnemyAbstract enemyCode = enemy1.GetComponent<EnemyAbstract>();
        enemyCode.player = player;
        enemyCode.level = this;
        Debug.Log("spawned");
    }

    public void DeadEnemy()
    {
        Debug.Log("here");
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, 0, gridSizeY));
    }

    public void Save()
    {
        GameManager.Instance.akAmmo = ak;
        GameManager.Instance.shottyAmmo = shottyAmmo;
        GameManager.Instance.Save();
    }
}