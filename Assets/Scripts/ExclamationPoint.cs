using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationPoint : MonoBehaviour {

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite[] exclamationSprites;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }

    public void SetStatus(bool enabled, float fill) {
        if (enabled) {
            sprite.enabled = true;
            int spriteIndex = (int)Mathf.Floor(fill * (exclamationSprites.Length - 1));
            sprite.sprite = exclamationSprites[spriteIndex];
        }
        else {
            sprite.enabled = false;
        }
    }
}
