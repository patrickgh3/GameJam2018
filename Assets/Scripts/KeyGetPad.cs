using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGetPad : SpeechPad
{

    [SerializeField]
    private int KeyCorrect;

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
        attemptGiveKey(0);
    }

    override
    public void Action1()
    {
        attemptGiveKey(1);

    }

    override
    public void Action2()
    {
        attemptGiveKey(2);

    }

    override
    public void Action3()
    {
        attemptGiveKey(3);

    }

    public void attemptGiveKey(int attempt)
    {
        if (attempt == KeyCorrect)
        {
            if (caller.GetComponent<Player>() != null)
            {
                caller.GetComponent<Player>().giveKey();
            }
            if (caller.GetComponent<NPC>() != null)
            {
                caller.GetComponent<NPC>().giveKey();
            }
        }
        else if(caller.GetComponent<Player>() != null)
        {
            caller.GetComponent<Player>().die();
        }
    }
}
