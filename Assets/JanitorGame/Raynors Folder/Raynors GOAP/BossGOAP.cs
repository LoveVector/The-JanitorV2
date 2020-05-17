using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BossGOAP : MonoBehaviour
{
    List<GoalGoap> goals;
    List<ActionGoap> actions;

    public Slider healthSlider;

    public StatesCollection worldState;

    public GameObject player;

    ActionGoap currentAction;

    Queue<ActionGoap> currentPlan;

    public List<Transform> walkLocation1;
    public List<Transform> walkLocation2;

    public Transform healPosition;

    [HideInInspector] public List<List<Transform>> allWalkLocations;

    public int planDepth;
    [HideInInspector] public int selectedGoal;

    public float speed;
    public float fireRate;
    public int maxHealth;
    float lastHealedHealth;
    public float damageTaken;

    bool dead;

    public GameObject gun;
    public ParticleSystem muzzle;
    public Transform shootPosition;

    [HideInInspector] public bool check = false;
    [HideInInspector] public List<Vector3> waypoints;
    [HideInInspector] public float lastTimeFired;
    [HideInInspector] public int selectedLocation;
    [HideInInspector] public int selectedPath;
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool coroutineDone;
    [HideInInspector] public bool lookAtPlayer;
    public float currentHealth;

    public bool useAStar;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        lastHealedHealth = maxHealth;
        damageTaken = 0;

        gun.SetActive(false);

        dead = false;

        if (useAStar)
        {

            StartCoroutine("GetPlayerDirection");
        }
        StartCoroutine("CallBackup");

        anim = GetComponent<Animator>();

        lastTimeFired = Time.time + fireRate;

        actions = new List<ActionGoap>(GetComponents<ActionGoap>());
        goals = new List<GoalGoap>(GetComponents<GoalGoap>());

        currentPlan = new Queue<ActionGoap>();

        selectedGoal = 0;
        selectedPath = 0;
        selectedLocation = 0;

        allWalkLocations = new List<List<Transform>>();
        allWalkLocations.Add(walkLocation1);
        allWalkLocations.Add(walkLocation2);

        StartCoroutine("CheckPlayerLocation");

        Plan();

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == false)
        {
            if (currentAction == null || currentAction.runAction(this))
            {
                if (currentPlan.Count != 0)
                {
                    currentAction = currentPlan.Dequeue();
                }
                else
                {
                    currentAction = null;
                }
            }
            if (lookAtPlayer)
            {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            }

            if (lastHealedHealth - currentHealth >= 2500 && !worldState.states.Contains("lowHealth")) //Check whether its time to go to heal position
            {
                damageTaken = 0;
                lastHealedHealth = currentHealth;
                selectedGoal = 2;
                worldState.states.Clear();
                worldState.states.Add("lowHealth");
                Plan();
            }


            if (currentHealth <= 0)
            {
                if (dead == false)
                {
                    anim.SetTrigger("Death");
                    currentAction = null;
                    currentPlan.Clear();
                    dead = true;
                }
            }
        }
    }

    public void Plan()
    {
        currentAction = null;
        currentPlan.Clear();

        Stack<StatesCollection> simWorldState = new Stack<StatesCollection>();
        Stack<ActionGoap> simActions = new Stack<ActionGoap>();
        Stack<int> simDepths = new Stack<int>();

        ActionGoap[] simPlan = new ActionGoap[planDepth];

        int minDepth = int.MaxValue;

        simWorldState.Push(new StatesCollection(worldState));
        simDepths.Push(0);
        simActions.Push(null);

        while (simWorldState.Count != 0)
        {
            StatesCollection cSimState = simWorldState.Pop();
            int cDepth = simDepths.Pop();
            ActionGoap cSimActions = simActions.Pop();

            simPlan[cDepth] = cSimActions;

            if (cDepth > minDepth) // Bigger than previous plan thus inefficient
            {
                continue;
            }

            if (cSimState.CompareStates(goals[selectedGoal].desiredStates) == 0 || cDepth >= planDepth)
            {
                if(cDepth < minDepth)
                {
                    minDepth = cDepth;
                    currentPlan.Clear();
                    for (int i = 0; i < simPlan.Length; i++)
                    {
                        if(simPlan[i] != null)
                        {
                            currentPlan.Enqueue(simPlan[i]);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    if(cSimState.CompareStates(actions[i].requiredStates) == 0 && cSimState.CompareStates(actions[i].outcomeStates) > 0) // has to be possible and has to cause something
                    {
                        StatesCollection newState = new StatesCollection(cSimState);
                        newState.AddStates(actions[i].outcomeStates);
                        simWorldState.Push(newState);
                        simActions.Push(actions[i]);
                        simDepths.Push(cDepth + 1);
                    }
                }
            }
        }
    }

    IEnumerator GetPlayerDirection()
    {
        check = true;
        yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        check = false;
        waypoints = LevelManager.Instance.pathfinding.PathFind(this.transform.position, allWalkLocations[selectedPath][selectedLocation].position, LevelManager.Instance.grid);
    }

    IEnumerator CheckPlayerLocation()
    {
        yield return new WaitForSeconds(1f);
        if (selectedGoal != 1)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) <= 3)
            {
                Debug.Log("here");
                worldState.states.Clear();
                worldState.states.Add("playerClose");
                selectedGoal = 1;
                Plan();
            }
        }
        StartCoroutine("CheckPlayerLocation");
    }

    IEnumerator ShootPlayer()
    {
        lookAtPlayer = true;
        yield return new WaitForSeconds(2.5f);
        lookAtPlayer = false;
        gun.SetActive(false);
        Plan();
        lastTimeFired = Time.time + fireRate;
    }

    IEnumerator CallBackup()
    {
        yield return new WaitForSeconds(3f);
        if (!worldState.states.Contains("lowHealth"))
        {
            worldState.states.Clear();
            worldState.states.Add("noEnemies");
            selectedGoal = 3;
            Plan();
        }
        else
        {
            StartCoroutine("CallBackup");
        }
    }

    IEnumerator CallBackUpAction()
    {
        lookAtPlayer = true;
        yield return new WaitForSeconds(5f);
        lookAtPlayer = false;
        worldState.states.Clear();
        worldState.states.Add("canMove");
        selectedGoal = 0;
        Plan();
    }
    public IEnumerator Teleport(Vector3 position, float time)
    {
        yield return new WaitForSeconds(time);
        this.transform.position = position;
        if(useAStar == false)
        {
            agent.destination = position;
        }
        if (worldState.states.Contains("canMove"))
        {
            lastTimeFired = Time.time + fireRate;
            selectedGoal = 0;
        }
            Plan();
    }

    public void Fire()
    {
        muzzle.Emit(10);
        RaycastHit hit;
        Vector3 direction = (player.transform.position - shootPosition.position).normalized;
        if (Physics.Raycast(shootPosition.position, direction * 1000, out hit))
        {
            if(hit.collider.tag == "Player")
            {
                Debug.Log("hit");
            }
        }
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value -= damage;
        if(damage > 0)
        {
            damageTaken += damage;
        }
    }
}