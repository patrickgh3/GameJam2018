using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    private SpriteRenderer sprite;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    public void SetStatus(bool enabled)
    {
        if (enabled)
        {
            sprite.enabled = true;
        }
        else
        {
            sprite.enabled = false;
        }
    }
}
