using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {
    [SerializeField] private Texture fullWhiteTexture;

    private int fadeToSceneIndex;
    private bool musicPlaying = false;
    private bool startMusicNextScene = false;
    
    private float fadeTime = 0;
    private const float fadeLength = 0.5f;
    private enum FadeState {
        Idle,
        ToBlack,
        FromBlack,
    }
    private FadeState fadeState = FadeState.Idle;

    public static World Instance = null;

    private AudioSource ambiance;
    private AudioSource music;
    private AudioSource wind;

    private void Start() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ambiance = GameObject.Find("Ambiance").GetComponent<AudioSource>();
            music = GameObject.Find("Music").GetComponent<AudioSource>();
            wind = GameObject.Find("Wind").GetComponent<AudioSource>();
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
                SceneManager.LoadScene(fadeToSceneIndex);

                if (startMusicNextScene) {
                    startMusicNextScene = false;
                }

                if (SceneManager.GetActiveScene().name == "Ritual") {
                    EnableMusic(false);
                    if (!wind.isPlaying) wind.Play();
                }
                else {
                    EnableMusic(true);
                    if (wind.isPlaying) wind.Stop();
                }
            }
        }
        else if (fadeState == FadeState.FromBlack) {
            fadeTime -= Time.deltaTime;
            if (fadeTime < 0) {
                fadeTime = 0;
                fadeState = FadeState.Idle;
            }
        }

        // Ctrl-M to mute and unmute music
        if (Input.GetKeyDown(KeyCode.M) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftControl))) {
            EnableMusic(!musicPlaying);
        }

        if (fadeState == FadeState.Idle) {
            // Escape to return to title
            if (Input.GetKeyDown(KeyCode.Escape)) {
                StartFade(false, World.Instance.GetTitleScene(), 0);
            }

            // Page up and down to switch scenes
            int curScene = SceneManager.GetActiveScene().buildIndex;
            int numScenes = SceneManager.sceneCountInBuildSettings;
            if (Input.GetKeyDown(KeyCode.PageUp) && curScene < numScenes - 1)
            {
                StartFade(false, curScene + 1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.PageDown) && curScene > 0)
            {
                StartFade(false, curScene - 1, 0);
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

    public void StartFade(bool restart, int scene, float pauseTime) {
        fadeState = FadeState.ToBlack;
        fadeTime = -pauseTime;

        if (restart) {
            fadeToSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            fadeToSceneIndex = scene;
            startMusicNextScene = true;
        }
    }

    public int GetNextScene() {
        return SceneManager.GetActiveScene().buildIndex + 1;
    }

    public int GetTitleScene() {
        return 0;
        //return SceneManager.GetSceneByName("Assets/Scenes/Title").buildIndex;
    }

    public void Freeze() {
        Player player = FindObjectOfType<Player>();
        player.GetComponent<PlayerSprite>().Animate(Vector2.zero);
        player.frozen = true;

        StartFade(true, 0, 0.75f);

        /*foreach (NPC npc in FindObjectsOfType<NPC>()) {
            npc.speed = 0;
        }*/
    }

    public void EnableMusic(bool enabled) {
        if (!musicPlaying && enabled) {
            music.Play();
            if (!ambiance.isPlaying) ambiance.Play();
        }
        else if (!enabled) {
            music.Stop();
            ambiance.Stop();
        }
        musicPlaying = enabled;
    }

    public enum Clip {
        Speech1,
        Speech2,
        Speech3,
        Speech4,
        GateOpen,
        Exclamation,
        Bell,
        Sacrifice,
    }

    public AudioClip Speech1;
    public AudioClip Speech2;
    public AudioClip Speech3;
    public AudioClip Speech4;
    public AudioClip GateOpen;
    public AudioClip Exclamation;
    public AudioClip Bell;
    public AudioClip Sacrifice;

    public AudioClip drumsClip;
    public AudioClip windClip;

    public void PlaySound(Clip clip) {
        AudioClip audioClip = null;
        float volume = 1;
        switch (clip) {
            case Clip.Speech1: audioClip = Speech1; volume = 0.5f; break;
            case Clip.Speech2: audioClip = Speech2; volume = 0.5f; break;
            case Clip.Speech3: audioClip = Speech3; volume = 0.5f; break;
            case Clip.Speech4: audioClip = Speech4; volume = 0.5f; break;
            case Clip.GateOpen: audioClip = GateOpen; volume = 1; break;
            case Clip.Exclamation: audioClip = Exclamation; volume = 1; break;
            case Clip.Bell: audioClip = Bell; volume = 1; break;
            case Clip.Sacrifice: audioClip = Sacrifice; volume = 0.5f; break;
        }
        GetComponent<AudioSource>().PlayOneShot(audioClip, volume);
    }
}
