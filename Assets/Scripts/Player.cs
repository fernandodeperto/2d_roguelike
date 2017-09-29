using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject {
    public int _wallDamage = 1;
    public int _pointsPerFood = 10;
    public int _pointsPerSoda = 20;
    public float _restartLevelDelay = 1f;

    private Animator __animator;
    private int __food;

	protected override void Start () {
        __animator = GetComponent<Animator>();

        __food = GameManager._instance._playerFoodPoints;

        base.Start();
	}
	
	void Update () {
        if (!GameManager._instance._playersTurn)
            return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
    }

    private void OnDisable()
    {
        GameManager._instance._playerFoodPoints = __food;
    }

    private void CheckIfGameOver()
    {
        if (__food <= 0)
            GameManager._instance.GameOver();
    }

    protected override void AttemptMove<T>(int xDirection, int yDirection)
    {
        __food--;

        base.AttemptMove<T>(xDirection, yDirection);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager._instance._playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(_wallDamage);

        __animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)
    {
        __animator.SetTrigger("playerHit");

        __food -= loss;

        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            Invoke("Restart", _restartLevelDelay);
            enabled = false;
        }

        else if (collision.tag == "Food")
        {
            __food += _pointsPerFood;
            collision.gameObject.SetActive(false);
        }

        else if (collision.tag == "Soda")
        {
            __food += _pointsPerSoda;
            collision.gameObject.SetActive(false);
        }
    }
}
