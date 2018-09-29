using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    //determines current direction for movement. Defined by directions enum
    public int direction = (int)Directions.RIGHT;
    private Vector2[] directions = new Vector2[8];
    public float moveAmount;
    public float moveTimer;
    //when timer reaches this Threshhold, the NPC will move
    public float moveThreshold;
    public float moveSpeed;
    public float spriteSize;
    public Vector3 moveLocation;
    private string[] collisionLayers = { "Player" };

    // Use this for initialization
    void Start () {
        moveLocation = transform.position;
        populateDirections();
        moveTimer = Random.Range(0, moveThreshold / 4);
	}
	
	// Update is called once per frame
	void Update () {
        checkMovement();
        checkDirection();
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
        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        Collider2D playerCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(collisionLayers));
        if (playerCollision)
        {
            Player.Move(playerCollision.gameObject, (playerCollision.transform.position - this.transform.position).normalized * 5f); 
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

        Vector2 toMove = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y, transform.position.z);
            moveLocation = toMove;
    }

    void checkDirection()
    {
        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        string[] pathLayers = { "Paths" };
        Collider2D pathCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(pathLayers));
        if (pathCollision)
        {
            direction = pathCollision.GetComponent<NPCPath>().direction;
        }
    }
}
