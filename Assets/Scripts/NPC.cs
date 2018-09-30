﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    SpeechPad currentSpeechPad;
    int speechChoice = 0;
    bool spokeLastTurn = false;
    bool speakNextTurn = false;
    bool testSpoken = false;
    //determines current direction for movement. Defined by directions enum
    public int direction = (int)Directions.RIGHT;
    private Vector2[] directions = new Vector2[8];
    public float moveAmount;
    public float moveTimer;
    //when timer reaches this Threshhold, the NPC will move
    public float moveThreshold;
    //actions will be performed this many seconds before next move
    public float actionCheckTime;
    public float moveSpeed;
    public float spriteSize;
    public Vector3 moveLocation;
    private string[] collisionLayers = { "Player", "NPC"};
    public int blocked = 0;
    private PlayerSprite playerSprite;

    public bool hasKey;

    // Use this for initialization
    void Start () {
        moveLocation = transform.position;
        populateDirections();
        moveTimer = Random.Range(0, moveThreshold / 4);
    }
	
	// Update is called once per frame
	void Update () {
        if (GlobalBell.bellActive)
        {
            playerSprite.Animate(Vector2.zero);
        }
        else
        {
            playerSprite = GetComponent<PlayerSprite>();
            if (!testSpoken && Mathf.Floor(Time.time) % 5 == 2)
            {
                testSpoken = true;
                GetComponentInChildren<Speech>().Speak(0);
            }
            checkMovement();
            checkDirection();
        }
	}

    void checkMovement()
    {

        moveTimer += Time.deltaTime;
        if(moveTimer > moveThreshold)
        {
            moveTimer = 0;
            move(moveAmount * directions[direction]);
        }
        transform.position = Vector3.MoveTowards(transform.position, moveLocation, Time.deltaTime * moveSpeed);

        Vector3 moveDelta = moveLocation - transform.position;
        if (moveDelta.magnitude < 0.0001f) playerSprite.Animate(Vector2.zero);
        else playerSprite.Animate(moveDelta);

        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        Collider2D playerCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(collisionLayers));
        if (playerCollision)
        {
            Player.Move(playerCollision.gameObject, (playerCollision.transform.position - this.transform.position).normalized * 8f); 
        }
    }

    void populateDirections()
    {
        directions[(int)Directions.DOWN] = new Vector2(0, -1).normalized;
        directions[(int)Directions.LEFT_DOWN] = new Vector2(-1, -1).normalized;
        directions[(int)Directions.LEFT_UP] = new Vector2(-1, 1).normalized;
        directions[(int)Directions.LEFT] = new Vector2(-1, 0).normalized;
        directions[(int)Directions.RIGHT_DOWN] = new Vector2(1, -1).normalized;
        directions[(int)Directions.RIGHT_UP] = new Vector2(1, 1).normalized;
        directions[(int)Directions.RIGHT] = new Vector2(1, 0).normalized;
        directions[(int)Directions.UP] = new Vector2(0, 1).normalized;
    }

    void move(Vector2 delta)
    {

        if (!speakNextTurn)
        {
            spokeLastTurn = false;
            Vector2 perpendicular = (Quaternion.Euler(0, 0, 90) * directions[direction]) * spriteSize;
            Vector2 parallel = directions[direction] * spriteSize;
            if (Physics2D.Raycast(transform.position + new Vector3(parallel.x, parallel.y, transform.position.z), delta, moveAmount + spriteSize, LayerMask.GetMask(collisionLayers)).collider == null &&
                Physics2D.Raycast(transform.position + new Vector3(parallel.x, parallel.y, transform.position.z) - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), delta,
                    moveAmount + spriteSize, LayerMask.GetMask(collisionLayers)).collider == null &&
                Physics2D.Raycast(transform.position + new Vector3(parallel.x, parallel.y, transform.position.z) - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), delta,
                    moveAmount + spriteSize, LayerMask.GetMask(collisionLayers)).collider == null)
            {
                blocked = 0;
                Vector2 toMove = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y, transform.position.z);
                moveLocation = toMove;
            }
            else
            {
                blocked++;
            }
        }
        else
        {
            GetComponentInChildren<Speech>().Speak(speechChoice);
            if(speechChoice == 0)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action0();
            }
            if (speechChoice == 1)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action1();
            }
            if (speechChoice == 2)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action2();
            }
            if (speechChoice == 3)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action3();
            }
            spokeLastTurn = true;
        }
        speakNextTurn = false;
    }

    void checkDirection()
    {
        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        string[] pathLayers = { "Path" };
        Collider2D pathCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(pathLayers));
        if (pathCollision)
        {
            direction = pathCollision.GetComponent<NPCPath>().direction;
        }
        string[] actionLayers = { "Interactable" };
        Collider2D actionCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(actionLayers));
        if (actionCollision && !spokeLastTurn)
        {
            speakNextTurn = true;
            int[] validActions = actionCollision.GetComponent<SpeechPad>().validActions;
            speechChoice = validActions[(int)Random.Range(0, validActions.Length)];
            currentSpeechPad = actionCollision.GetComponent<SpeechPad>();
        }
        string[] despawnLayers = { "Goal" };
        Collider2D goalCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(despawnLayers));
        if (goalCollision && (goalCollision.GetComponent<Goal>().isOpen || hasKey && goalCollision.GetComponent<Goal>().keyDoor))
        {
            goalCollision.gameObject.GetComponent<Goal>().isOpen = false;
            Destroy(this.gameObject);
        }
    }

    public void giveKey()
    {
        hasKey = true;
        GetComponentInChildren<Key>().SetStatus(true);
    }
}
