using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speech : MonoBehaviour {
    [SerializeField] private Sprite[] symbols;

    private bool speaking = false;
    private float speakStartTime = 0;

    private const float speakDuration = 0.65f;

    private SpriteRenderer speechSprite;
    private SpriteRenderer symbolSprite;
    private Vector3 speechScale;

    private void Start() {
        speechSprite = GetComponent<SpriteRenderer>();
        speechSprite.enabled = false;
        symbolSprite = GetComponentsInChildren<Transform>()[1].GetComponent<SpriteRenderer>();
        symbolSprite.enabled = false;

        speechScale = transform.localScale;
    }

    private void FixedUpdate() {
        if (speaking) {
            if (Time.time > speakStartTime + speakDuration) {
                speaking = false;
            }

            //float alpha = Mathf.Min(1f, speechSprite.color.a + 5f * Time.deltaTime);
            //speechSprite.color = new Color(1, 1, 1, alpha);

            transform.localScale += (speechScale - transform.localScale) * 0.1f;
        }
        else {
            float alpha = Mathf.Max(0, speechSprite.color.a - 5f * Time.deltaTime);
            speechSprite.color = new Color(1, 1, 1, alpha);
            symbolSprite.color = speechSprite.color;
        }
    }

    public void Speak(int symbolNum) {
        speechSprite.enabled = true;
        symbolSprite.enabled = true;
        speechSprite.color = new Color(1, 1, 1, 1);
        symbolSprite.color = new Color(1, 1, 1, 1);
        symbolSprite.sprite = symbols[symbolNum];
        transform.localScale = speechScale * 1.4f;
        speaking = true;
        speakStartTime = Time.time;
        Debug.Log("Speak");
    }
}
