using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
    [SerializeField] private Texture fullWhiteTexture;

    private float fadeTime = 0;
    private const float fadeLength = 0.75f;
    private enum FadeState {
        Idle,
        ToBlack,
        FromBlack,
    }
    private FadeState fadeState = FadeState.Idle;

    public static World Instance = null;

    private void Start() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this);
        }
    }

    private void Update() {
        if (fadeState == FadeState.ToBlack) {
            fadeTime += Time.deltaTime;
            if (fadeTime > fadeLength * 1.2f) {
                fadeState = FadeState.FromBlack;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (fadeState == FadeState.FromBlack) {
            fadeTime -= Time.deltaTime;
            if (fadeTime < 0) {
                fadeTime = 0;
                fadeState = FadeState.Idle;
            }
        }
    }

    private void OnGUI() {
        if (!(Event.current.type.Equals(EventType.Repaint))) {
            return;
        }

        float alpha = fadeTime / fadeLength;
        Graphics.DrawTexture(Camera.main.pixelRect, fullWhiteTexture, new Rect(0, 0, 1, 1), 0, 0, 0, 0, new Color(0, 0, 0, alpha * 0.5f));
    }

    public void StartFade() {
        fadeState = FadeState.ToBlack;
        fadeTime = -0.2f;
    }
}
