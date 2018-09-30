using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    SpeechPad currentSpeechPad;
    private bool frozen = false;
    private bool hasKey = true;
    private float timeOutOfLine = 0;
    private const float timeUntilCaught = 0.75f;

    [SerializeField] private AnimationClip idleAnim;
    [SerializeField] private AnimationClip walkAnim;

    private PlayerSprite playerSprite;

    void Start() {
        GetComponentInChildren<Key>().SetStatus(false);
        playerSprite = GetComponent<PlayerSprite>();
	}
	
	void FixedUpdate() {
        if (frozen) return;

        float moveSpeed = 300f * Time.deltaTime;
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        Vector3 toMove = new Vector3(horiz * moveSpeed, vert * moveSpeed);
        Move(gameObject, toMove);

        GetComponent<PlayerSprite>().Animate(toMove);

        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        string[] actionLayers = { "Interactable" };
        Collider2D actionCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(actionLayers));
        if (actionCollision)
        {
            currentSpeechPad = actionCollision.GetComponent<SpeechPad>();
        }
        else
        {
            currentSpeechPad = null;
        }
        if (Input.GetButtonDown("Speak1")) {
            GetComponentInChildren<Speech>().Speak(0);
            if(currentSpeechPad)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action0();
            }
        }
        if (Input.GetButtonDown("Speak2")) {
            GetComponentInChildren<Speech>().Speak(1);
            if (currentSpeechPad)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action1();
            }
        }
        if (Input.GetButtonDown("Speak3")) {
            GetComponentInChildren<Speech>().Speak(2);
            if (currentSpeechPad)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action2();
            }
        }
        if (Input.GetButtonDown("Speak4")) {
            GetComponentInChildren<Speech>().Speak(3);
            if (currentSpeechPad)
            {
                currentSpeechPad.caller = this.gameObject;
                currentSpeechPad.Action3();
            }
        }

        if (Input.GetKeyDown("1")) playerSprite.playerColor = PlayerSprite.PlayerColor.Black;
        if (Input.GetKeyDown("2")) playerSprite.playerColor = PlayerSprite.PlayerColor.Blue;
        if (Input.GetKeyDown("3")) playerSprite.playerColor = PlayerSprite.PlayerColor.Red;

        if (Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(new string[] { "Death" }))) {
            timeOutOfLine += Time.deltaTime;
            if (timeOutOfLine > timeUntilCaught) {
                frozen = true;
                World.Instance.StartFade(true, "");
            }

            GetComponentInChildren<ExclamationPoint>().SetStatus(true, timeOutOfLine / timeUntilCaught);
        }
        else {
            timeOutOfLine = 0;
            GetComponentInChildren<ExclamationPoint>().SetStatus(false, 0);
        }
        goalCheck();
        if (Input.GetKeyDown(KeyCode.R)) {
            frozen = true;
            World.Instance.StartFade(true, "");
        }
    }

    // Moves the Player or an NPC while colliding with walls and NPCs.
    public static void Move(GameObject obj, Vector2 delta) {
        Vector3 toMove = new Vector3(Mathf.Round(delta.x), Mathf.Round(delta.y));

        obj.transform.position += toMove;

        Vector2 size = obj.GetComponent<BoxCollider2D>().size * obj.transform.lossyScale.x;
        int collisionLayers = (1 << 9) | (1 << 10);

        if (Physics2D.OverlapBox(obj.transform.position, size, 0, collisionLayers))
        {
            obj.transform.position -= toMove;

            for (int i = 0; i < Mathf.Abs(toMove.x); i++)
            {
                obj.transform.position += new Vector3(Mathf.Sign(toMove.x), 0);
                if (Physics2D.OverlapBox(obj.transform.position, size, 0, collisionLayers))
                {
                    obj.transform.position -= new Vector3(Mathf.Sign(toMove.x), 0);
                    break;
                }
            }

            for (int i = 0; i < Mathf.Abs(toMove.y); i++)
            {
                obj.transform.position += new Vector3(0, Mathf.Sign(toMove.y));
                if (Physics2D.OverlapBox(obj.transform.position, size, 0, collisionLayers))
                {
                    obj.transform.position -= new Vector3(0, Mathf.Sign(toMove.y));
                    break;
                }
            }
        }
    }

    public void goalCheck()
    {
        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        string[] despawnLayers = { "Goal" };
        Collider2D goalCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(despawnLayers));
        if (goalCollision && (goalCollision.GetComponent<Goal>().isOpen || hasKey && goalCollision.GetComponent<Goal>().keyDoor))
        {
            goalCollision.gameObject.GetComponent<Goal>().isOpen = false;
            Debug.Log("Won the level");
            Destroy(this.gameObject);
        }
    }

    public void giveKey()
    {
        hasKey = true;
        GetComponentInChildren<Key>().SetStatus(true);
    }
}
