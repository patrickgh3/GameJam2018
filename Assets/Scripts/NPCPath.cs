using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/**
 * NPCPath, to be attached to corresponding path object to direct NPC Movement.
 **/
public class NPCPath: MonoBehaviour
{
    public int direction;
    public string directionString;
    // Use this for initialization
    void Start()
    {
        direction = (int)Enum.Parse(typeof(Directions), directionString);
    }

    // Update is called once per frame
    void Update()
    {

    }
}