using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

    [SerializeField] private Object startScene;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown) {
            World.Instance.StartFade(false, startScene.name);
        }
	}
}
