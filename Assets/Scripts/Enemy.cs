using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
    public int _playerDamage;

    private Animator __animator;
    private Transform __target;
    private bool __skipMove;

	protected override void Start () {
        __animator = GetComponent<Animator>();
        __target = GameObject.FindGameObjectWithTag("Player").transform;

        GameManager._instance.AddEnemyToList(this);

        base.Start();
	}
	
	void Update () {	
	}

    protected override void AttemptMove<T>(int xDirection, int yDirection)
    {
        if (__skipMove)
        {
            __skipMove = false;
            return;
        }
 
        base.AttemptMove<T>(xDirection, yDirection);

        __skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDirection = 0;
        int yDirection = 0;

        if (Mathf.Abs(__target.position.x - transform.position.x) < float.Epsilon)
            yDirection = __target.position.y > transform.position.y ? 1 : -1;
        else
            xDirection = __target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDirection, yDirection);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        __animator.SetTrigger("enemyAttach");

        hitPlayer.LoseFood(_playerDamage);
    }
}
