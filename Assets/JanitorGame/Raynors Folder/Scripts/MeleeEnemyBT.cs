using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyBT : EnemyAbstract
{
    Selector mainSel;
    Selector runStateSel;

    Sequence deathSeq;
    Sequence chaseSeq;
    Sequence surrenderSeq;

    SurrenderCheckNode surrenderCheck;
    DeathHealthCheckNode deathHealthCheck;
    DeathNode death;
    SurrenderNode surrender;
    AttackNode attack;
    InjuredRunNode injured;
    NormalRunNode normal;
    ChasePlayer chase;
    Node node;
    // Start is called before the first frame update
    void Start()
    {
        bulletLayer = LayerMask.NameToLayer("PlayerBullet");

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        mainSel = new Selector();
        runStateSel = new Selector();

        deathSeq = new Sequence();
        chaseSeq = new Sequence();
        surrenderSeq = new Sequence();

        deathHealthCheck = new DeathHealthCheckNode();
        surrenderCheck = new SurrenderCheckNode();
        death = new DeathNode();
        surrender = new SurrenderNode();
        attack = new AttackNode();
        injured = new InjuredRunNode();
        normal = new NormalRunNode();
        chase = new ChasePlayer();
        node = mainSel;

        mainSel.nList.Add(deathSeq);
        mainSel.nList.Add(surrenderSeq);
        mainSel.nList.Add(chaseSeq);

        deathSeq.nList.Add(deathHealthCheck);
        deathSeq.nList.Add(death);

        surrenderSeq.nList.Add(surrenderCheck);
        surrenderSeq.nList.Add(surrender);

        chaseSeq.nList.Add(runStateSel);
        chaseSeq.nList.Add(chase);
        chaseSeq.nList.Add(attack);

        runStateSel.nList.Add(injured);
        runStateSel.nList.Add(normal);
    }

    // Update is called once per frame
    void Update()
    {
        node.UpdateNode(this);
    }
}
