using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGetPad : SpeechPad
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    override
    public void Action0()
    {
        if(caller.GetComponent<Player>() != null)
        {
            caller.GetComponent<Player>().giveKey();
        }
        if (caller.GetComponent<NPC>() != null)
        {
            caller.GetComponent<NPC>().giveKey();
        }

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
