using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpeechPad : MonoBehaviour {

    public int[] validActions;
    public GameObject caller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Actions performed when a speech is used on this pad.
     * Actions should be performed by both NPCs and the Player
     */
    public abstract void Action0();
    public abstract void Action1();
    public abstract void Action2();
    public abstract void Action3();
}
