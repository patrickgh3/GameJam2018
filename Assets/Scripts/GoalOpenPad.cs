using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalOpenPad : SpeechPad {

    public GameObject Goal;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    override
    public void Action0()
    {
        Goal.GetComponent<Goal>().isOpen = true;
    }

    override
    public void Action1()
    {

    }

    override
    public void Action2()
    {

    }

    override
    public void Action3()
    {

    }
}
