using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speech : MonoBehaviour {
    [SerializeField] private GameObject speechBubble;

    [SerializeField] private Sprite[] symbols;

    private bool speaking = false;
    private float speakStartTime = 0;

    private const float speakDuration = 0.65f;

    private SpriteRenderer speechSprite;
    private Vector3 speechScale;

    private void Start() {
        speechSprite = speechBubble.GetComponent<SpriteRenderer>();
        speechScale = speechBubble.transform.localScale;
    }

    private void FixedUpdate() {
        if (speaking) {
            if (Time.time > speakStartTime + speakDuration) {
                speaking = false;
            }

            //float alpha = Mathf.Min(1f, speechSprite.color.a + 5f * Time.deltaTime);
            //speechSprite.color = new Color(1, 1, 1, alpha);

            speechBubble.transform.localScale += (speechScale - speechBubble.transform.localScale) * 0.1f;
        }
        else {
            float alpha = Mathf.Max(0, speechSprite.color.a - 5f * Time.deltaTime);
            speechSprite.color = new Color(1, 1, 1, alpha);
        }
    }

    public void Speak(int symbolNum) {
        speechSprite.enabled = true;
        speechSprite.color = new Color(1, 1, 1, 1);
        speechSprite.sprite = symbols[symbolNum];
        speechBubble.transform.localScale = speechScale * 1.4f;
        speaking = true;
        speakStartTime = Time.time;
    }
}
