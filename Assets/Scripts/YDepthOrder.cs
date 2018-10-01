using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YDepthOrder : MonoBehaviour {
    public bool aboveParent = false;
    private SpriteRenderer sprite;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        if (aboveParent) {
            sprite.sortingOrder = -(int)Mathf.Round(transform.parent.position.y) + 1;
        }
        else {
            sprite.sortingOrder = -(int)Mathf.Round(transform.position.y);
        }
	}
}
