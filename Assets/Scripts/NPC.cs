using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    //determines current direction for movement. Defined by directions enum
    private int direction = (int)Directions.RIGHT;
    private Vector2[] directions = new Vector2[8];
    public float moveAmount;
    public float moveTimer;
    //when timer reaches this Threshhold, the NPC will move
    public float moveThreshold;
    public float moveSpeed;
    public Vector3 moveLocation;

	// Use this for initialization
	void Start () {
        moveLocation = transform.position;
        populateDirections();
        moveTimer = Random.Range(0, moveThreshold / 2);
	}
	
	// Update is called once per frame
	void Update () {
        checkMovement();
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
        Vector2 toMove = toMove = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y, transform.position.z);

        moveLocation = toMove;
    }
}
