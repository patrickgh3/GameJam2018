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
    public float spriteSize;
    public Vector3 moveLocation;
    private string[] collisionLayers = { "Player" };

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
        Vector2 perpendicular = (Quaternion.Euler(0, 0, 90) * directions[direction]) * spriteSize;
        Debug.DrawRay(transform.position, directions[direction] * (moveAmount+50), Color.red);
        Debug.DrawRay(transform.position - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), directions[direction] * (moveAmount + 50), Color.red);
        Debug.DrawRay(transform.position + (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), directions[direction] * (moveAmount + 50), Color.red);
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
            Player.Move(playerCollision.gameObject, (playerCollision.transform.position - this.transform.position).normalized * 1f); 
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
        Vector2 perpendicular = (Quaternion.Euler(0, 0, 90) * directions[direction]) * spriteSize;
        if (Physics2D.Raycast(transform.position, delta, moveAmount + spriteSize, LayerMask.GetMask(collisionLayers)).collider == null &&
            Physics2D.Raycast(transform.position - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), delta, 
                moveAmount + spriteSize, LayerMask.GetMask(collisionLayers)).collider == null &&
            Physics2D.Raycast(transform.position - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), delta, 
                moveAmount + spriteSize, LayerMask.GetMask(collisionLayers)).collider == null)
        {
            Vector2 toMove = toMove = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y, transform.position.z);
            moveLocation = toMove;
        }
    }
}
