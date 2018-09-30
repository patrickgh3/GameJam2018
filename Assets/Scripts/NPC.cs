using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    SpeechPad currentSpeechPad;
    int speechChoice = 0;
    //determines current direction for movement. Defined by directions enum
    public int direction = (int)Directions.RIGHT;
    protected Vector2[] directions = new Vector2[8];
    //when timer reaches this Threshhold, the NPC will move
    public float moveThreshold;
    //actions will be performed this many seconds before next move
    private float actionCheckTime;
    [SerializeField]
    private float actionTimeNecessary;
    private GameObject gateObject;
    private float moveSpeed;
    public float spriteSize;
    private string[] collisionLayers = { "Player", "NPC"};
    public float blocked = 0;
    private PlayerSprite playerSprite;
    private bool lockMovement = false;
    public float speed = 300f;
    private bool sacrificed = false;

    private bool hasKey;

    // Use this for initialization
    protected void Start () {
        gateObject = GameObject.FindGameObjectWithTag("Finish");
        populateDirections();
        playerSprite = GetComponent<PlayerSprite>();
    }
	
	// Update is called once per frame

	protected void Update () {
        if (sacrificed) return;

        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        if (!lockMovement)
        {
            moveSpeed = speed * Time.deltaTime;
            if (BellStop.bellActive)
            {
                playerSprite.Animate(Vector2.zero);
            }
            else
            {
                playerSprite = GetComponent<PlayerSprite>();
                checkMovement();
                checkDirection();
            }
        }
        else
        {
            float moveSpeed = 300f * Time.deltaTime;
            Vector3 toMove = new Vector3(Mathf.Round((moveSpeed * Vector3.up).x), Mathf.Round((moveSpeed * Vector3.up).y));
            gameObject.transform.position += toMove;
            string[] despawnLayers = { "Despawn" };
            Collider2D despawnCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(despawnLayers));
            if (despawnCollision)
            {
                gateObject.GetComponent<Animator>().SetTrigger("gateClose");
                Destroy(this.gameObject);
            }
        }

        // Sacrifice trigger
        if (!sacrificed && Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(new string[] { "SacrificeTrigger" }))) {
            gameObject.AddComponent<Sacrifice>();
            sacrificed = true;
            playerSprite.Animate(Vector2.zero);
            World.Instance.PlaySound(World.Clip.Sacrifice);
        }
    }

    void checkMovement()
    {
        if (actionCheckTime <= 0)
        {
            move(moveSpeed * directions[direction]);
            Vector3 moveDelta = moveSpeed * directions[direction];
            if (moveDelta.magnitude < 0.0001f) playerSprite.Animate(Vector2.zero);
            else playerSprite.Animate(moveDelta);

            Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
            Collider2D playerCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(collisionLayers));
            if (playerCollision)
            {
                Player.Move(playerCollision.gameObject, (playerCollision.transform.position - this.transform.position).normalized * 8f);
            }
        }
        else
        {
            actionCheckTime -= Time.deltaTime;
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


        Vector2 perpendicular = (Quaternion.Euler(0, 0, 90) * directions[direction]) * spriteSize*1.5f;
        Vector2 parallel = directions[direction] * spriteSize*1.5f;
        Debug.DrawRay(transform.position + new Vector3(parallel.x, parallel.y, transform.position.z), delta, Color.red);
        if (Physics2D.Raycast(transform.position + new Vector3(parallel.x, parallel.y, transform.position.z), delta, spriteSize * 2, LayerMask.GetMask(collisionLayers)).collider == null &&
            Physics2D.Raycast(transform.position + new Vector3(parallel.x, parallel.y, transform.position.z) - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), delta,
                spriteSize * 2, LayerMask.GetMask(collisionLayers)).collider == null &&
            Physics2D.Raycast(transform.position + new Vector3(parallel.x, parallel.y, transform.position.z) - (new Vector3(perpendicular.x, perpendicular.y, transform.position.z)), delta,
                spriteSize * 2, LayerMask.GetMask(collisionLayers)).collider == null)
        {
            Vector3 toMove = new Vector3(delta.x, delta.y);
            transform.position += toMove;
            blocked = 0;
        }
        else
        {
            moveSpeed = 0;
            //Debug.Log("Blocked");
            blocked += Time.deltaTime;
        }
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
        if (actionCollision && actionCollision.GetComponent<SpeechPad>() && actionCollision.GetComponent<SpeechPad>() != currentSpeechPad)
        {
            actionCheckTime = actionTimeNecessary;
            playerSprite.Animate(Vector2.zero);
            int[] validActions = actionCollision.GetComponent<SpeechPad>().validActions;
            speechChoice = validActions[(int)Random.Range(0, validActions.Length)];
            currentSpeechPad = actionCollision.GetComponent<SpeechPad>();
            GetComponentInChildren<Speech>().Speak(speechChoice);
            if (speechChoice == 0)
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
        }
        //placeholder for gate opener
        else if(actionCollision && actionCollision.name == "GateOpen")
        {
            GameObject.Find("Goal").GetComponent<Goal>().isOpen = true;
        }
        string[] goalLayers = { "Goal" };
        Collider2D goalCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(goalLayers));
        if (goalCollision && (goalCollision.GetComponent<Goal>().isOpen || hasKey && goalCollision.GetComponent<Goal>().keyDoor))
        {
            goalCollision.gameObject.GetComponent<Goal>().isOpen = false;
            gateObject.GetComponent<Animator>().SetTrigger("gateOpen");
            lockMovement = true;
        }

    }

    public void giveKey()
    {
        hasKey = true;
        GetComponentInChildren<Key>().SetStatus(true);
    }
}
