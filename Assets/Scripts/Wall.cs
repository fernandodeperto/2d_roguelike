using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int hitPoints = 4;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = dmgSprite;

        hitPoints -= loss;

        if (hitPoints <= 0)
            gameObject.SetActive(false);
    }
}
