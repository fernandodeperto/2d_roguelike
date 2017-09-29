using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager _boardScript;
    public static GameManager _instance = null;

    public int _playerFoodPoints = 100;
    [HideInInspector] public bool _playersTurn = true;

    private int __level = 3;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        _boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        _boardScript.SetupScene(__level);
    }

    public void GameOver()
    {
        enabled = false;
    }
}
