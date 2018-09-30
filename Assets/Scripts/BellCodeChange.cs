using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellCodeChange : MonoBehaviour {

    public static bool bellActive;
    public float bellThreshhold;
    public float bellStopThreshhold;
    public float bellTimer;
    public Animator BellAnimator;
    private World world;
    // Use this for initialization
    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {
        handleBell();
    }

    void handleBell()
    {
        bellTimer += Time.deltaTime;
        if (bellActive)
        {
            if (bellTimer > bellStopThreshhold)
            {
                if (BellAnimator != null) BellAnimator.SetTrigger("BellStop");
                bellTimer = 0;
                bellActive = false;
            }
        }
        else
        {
            if (bellTimer > bellThreshhold)
            {
                world.PlaySound(World.Clip.Bell);
                bellTimer = 0;
                bellActive = true;
                if (BellAnimator != null) BellAnimator.SetTrigger("BellTrigger");
                foreach (Inspector inspector in FindObjectsOfType<Inspector>())
                {
                    for (int i = 0; i < inspector.symbolCycle.Length; i++)
                    {
                        inspector.symbolCycle[i] = (inspector.symbolCycle[i] + 2) % 4;
                    }
                    inspector.Revalidate();
                }
            }
        }
    }
}

