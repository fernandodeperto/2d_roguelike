using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject _gameManager;

	void Awake ()
    {
        if (GameManager._instance == null)
            Instantiate(_gameManager);
	}
}
