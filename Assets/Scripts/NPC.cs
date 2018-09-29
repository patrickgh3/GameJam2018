using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    bool testSpoken = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!testSpoken && Mathf.Floor(Time.time) % 5 == 2) {
            GetComponent<Speech>().Speak(0);
            testSpoken = true;
        }
	}
}
