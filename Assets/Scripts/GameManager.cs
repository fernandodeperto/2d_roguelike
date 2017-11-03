using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BoardManager _boardScript;
    public static GameManager _instance = null;
    public int _playerFoodPoints = 100;
    [HideInInspector] public bool _playersTurn = true;
    public float _turnDelay = .1f;
    public float levelStartupDelay = 1f;    

    private int __level = 1;
    private List<Enemy> __enemies;
    private bool __enemiesMoving;
    private bool __doingSetup = true;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        __enemies = new List<Enemy>();

        _boardScript = GetComponent<BoardManager>();
        InitGame();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void InitGame()
    {
        //__doingSetup = true;

        __enemies.Clear();
        _boardScript.SetupScene(__level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        __level++;

        InitGame();
    }

    IEnumerator MoveEnemies()
    {
        __enemiesMoving = true;

        yield return new WaitForSeconds(_turnDelay);

        if (__enemies.Count == 0)
            yield return new WaitForSeconds(_turnDelay);

        for (int i = 0; i < __enemies.Count; i++)
        {
            __enemies[i].MoveEnemy();
            yield return new WaitForSeconds(__enemies[i]._moveTime);
        }

        _playersTurn = true;
        __enemiesMoving = false;
    }

    private void Update()
    {
        if (_playersTurn || __enemiesMoving)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy enemyScript)
    {
        __enemies.Add(enemyScript);
    }
}
