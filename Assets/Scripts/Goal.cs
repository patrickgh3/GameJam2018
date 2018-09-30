using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer goalSprite;
    public bool isOpen = false;
    public bool keyDoor = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if(isOpen)
        {
            goalSprite.color = new Color(1, 1, 1, 1);
        }	
        else
        {
            goalSprite.color = Color.blue;
        }
	}
}
