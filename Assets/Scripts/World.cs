using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
    [SerializeField] private Texture fullWhiteTexture;

    private string toScene;

    private float fadeTime = 0;
    private const float fadeLength = 0.5f;
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
            if (fadeTime > fadeLength * 1.5f) {
                fadeState = FadeState.FromBlack;
                SceneManager.LoadScene(toScene);
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

    public void StartFade(bool restart, string scene) {
        fadeState = FadeState.ToBlack;
        fadeTime = -0.75f;

        if (restart) toScene = SceneManager.GetActiveScene().name;
        else toScene = scene;
    }

    public void Freeze() {
        Player player = FindObjectOfType<Player>();
        player.GetComponent<PlayerSprite>().Animate(Vector2.zero);
        player.frozen = true;

        StartFade(true, "");

        /*foreach (NPC npc in FindObjectsOfType<NPC>()) {
            npc.speed = 0;
        }*/
    }

    public enum Clip {
        Speech1,
        Speech2,
        Speech3,
        Speech4,
        GateOpen,
        Exclamation,
        Bell,
    }

    public AudioClip Speech1;
    public AudioClip Speech2;
    public AudioClip Speech3;
    public AudioClip Speech4;
    public AudioClip GateOpen;
    public AudioClip Exclamation;
    public AudioClip Bell;

    public void PlaySound(Clip clip) {
        AudioClip audioClip = null;
        switch (clip) {
            case Clip.Speech1: audioClip = Speech1; break;
            case Clip.Speech2: audioClip = Speech2; break;
            case Clip.Speech3: audioClip = Speech3; break;
            case Clip.Speech4: audioClip = Speech4; break;
            case Clip.GateOpen: audioClip = GateOpen; break;
            case Clip.Exclamation: audioClip = Exclamation; break;
            case Clip.Bell: audioClip = Bell; break;
        }
        GetComponent<AudioSource>().PlayOneShot(audioClip, 1f);
    }
}
