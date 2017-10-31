using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int _minimum;
        public int _maximum;

        public Count(int minimum, int maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }
    }

    public int _rows = 8;
    public int _columns = 8;
    public Count _wallCount = new Count(5, 9);
    public Count _foodCount = new Count(1, 5);
    public GameObject _exit;
    public GameObject[] _floorTiles;
    public GameObject[] _foodTiles;
    public GameObject[] _enemyTiles;
    public GameObject[] _innerWallTiles;
    public GameObject[] _outerWallTiles;

    private Transform __boardHolder;
    private List<Vector3> __gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        __gridPositions.Clear();

        for (int x = 1; x < _columns - 1; x++)
        {
            for (int y = 1; y < _rows - 1; y++)
            {
                __gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        __boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < _columns + 1; x++)
        {
            for (int y = -1; y < _rows + 1; y++)
            {
                GameObject chosenObject;

                if (x == -1 || y == -1 || x == _columns || y == _rows)
                    chosenObject = _outerWallTiles[Random.Range(0, _outerWallTiles.Length)];
                else
                    chosenObject = _floorTiles[Random.Range(0, _floorTiles.Length)];

                GameObject instance = Instantiate(chosenObject, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(__boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, __gridPositions.Count);

        Vector3 randomPosition = __gridPositions[randomIndex];
        __gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject chosenTile = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(chosenTile, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        print("SetupScene");

        int enemyCount = (int)Mathf.Log(level, 2f);

        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(_innerWallTiles, _wallCount._minimum, _wallCount._maximum);
        LayoutObjectAtRandom(_foodTiles, _foodCount._minimum, _foodCount._maximum);
        LayoutObjectAtRandom(_enemyTiles, enemyCount, enemyCount);
        Instantiate(_exit, new Vector3(_columns - 1, _rows - 1, 0f), Quaternion.identity);
    }
}