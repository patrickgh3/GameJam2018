using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice : MonoBehaviour {

    private float time = 0;
    private Vector3 velocity = Vector2.zero;
    private Vector3 startPos;
    private float sinMagnitude = 0;
    private SpriteRenderer sprite;

    private void Start() {
        startPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        time += Time.deltaTime;

        velocity.y += Time.deltaTime * 1.5f;
        transform.position += velocity;

        sinMagnitude += Time.deltaTime * 15.0f;
        transform.position = new Vector2(startPos.x + sinMagnitude * Mathf.Sin(time * 2 * Mathf.PI * 2), transform.position.y);

        sprite.color = new Color(1, 1, 1, sprite.color.a - 0.35f * Time.deltaTime);
        if (sprite.color.a < 0) Destroy(gameObject);
	}
}
