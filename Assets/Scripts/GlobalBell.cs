using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBell : MonoBehaviour {

    public static bool bellActive;
    public float bellThreshhold;
    public float bellTimer;
    public float bellLeeway;
    public float bellStopThreshhold;
    [SerializeField]
    private GameObject moveDeathObject;
    private World world;
	// Use this for initialization
	void Start () {
        moveDeathObject.GetComponent<BoxCollider2D>().enabled = false;
        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    void Update () {
        handleBell();
	}

    void handleBell()
    {
        bellTimer += Time.deltaTime;
        if(bellActive)
        {
            if(bellTimer > bellLeeway)
            {
                //spawn moveDeath
                moveDeathObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            if(bellTimer > bellStopThreshhold)
            {
                bellTimer = 0;
                bellActive = false;
                moveDeathObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            if(bellTimer > bellThreshhold)
            {
                world.PlaySound(World.Clip.Bell);
                bellTimer = 0;
                bellActive = true;
            }
        }
    }
}
