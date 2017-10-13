using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite _dmgSprite;
    public int _hitPoints = 4;

    private SpriteRenderer __spriteRenderer;

    void Awake()
    {
        __spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        __spriteRenderer.sprite = _dmgSprite;

        _hitPoints -= loss;

        if (_hitPoints <= 0)
            gameObject.SetActive(false);
    }
}
