using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

    [SerializeField] private Object startScene;
    private bool started = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown && !started) {
            World.Instance.StartFade(false, World.Instance.GetNextScene(), 0);
            World.Instance.EnableMusic(true);
            started = true;
        }
	}
}
