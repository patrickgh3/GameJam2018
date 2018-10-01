using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    SpeechPad currentSpeechPad;
    public bool frozen = false;
    private bool hasKey = false;
    private float timeOutOfLine = 0;
    private const float timeUntilCaught = 0.75f;
    public bool moving = false;
    private bool lockMovement = false;
    private GameObject gateObject;
    private float speed;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private bool startedFade = false;

    private PlayerSprite playerSprite;
    private ExclamationPoint exclamation;

    void Start()
    {
        hasKey = false;
        gateObject = GameObject.FindGameObjectWithTag("Finish");
        playerSprite = GetComponent<PlayerSprite>();
        exclamation = GetComponentInChildren<ExclamationPoint>();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Run"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
        if (lockMovement)
        {
            float moveSpeed = speed * Time.deltaTime;
            Vector3 toMove = new Vector3(Mathf.Round((moveSpeed * Vector3.up).x), Mathf.Round((moveSpeed * Vector3.up).y));
            gameObject.transform.position += toMove;

            if (!startedFade)
            {
                string[] despawnLayers = { "Despawn" };
                Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
                Collider2D despawnCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(despawnLayers));
                if (despawnCollision)
                {
                    gateObject.GetComponent<Animator>().SetTrigger("gateClose");
                    World.Instance.StartFade(false, World.Instance.GetNextScene(), 0);
                    startedFade = true;
                }
            }
        }
        else
        {
            if (frozen) return;

            float moveSpeed = speed * Time.deltaTime;
            float horiz = Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");
            Vector3 toMove = new Vector3(horiz * moveSpeed, vert * moveSpeed);

            if (toMove != Vector3.zero)
            {
                moving = true;
            }
            else
            {
                moving = false;
            }
            Move(gameObject, toMove);

            GetComponent<PlayerSprite>().Animate(toMove);

            Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
            string[] actionLayers = { "Interactable" };
            Collider2D actionCollision = Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(actionLayers));
            if (actionCollision && actionCollision.GetComponent<SpeechPad>())
            {
                currentSpeechPad = actionCollision.GetComponent<SpeechPad>();
            }
            //for gate open (Interactable, but doesn't have a speech pad)
            else if (actionCollision && actionCollision.name == "GateOpen")
            {
                GameObject.Find("Goal").GetComponent<Goal>().isOpen = true;
                currentSpeechPad = null;
            }
            else
            {
                currentSpeechPad = null;
            }
            if (Input.GetButtonDown("Speak1"))
            {
                GetComponentInChildren<Speech>().Speak(0);
                if (currentSpeechPad)
                {
                    currentSpeechPad.caller = this.gameObject;
                    currentSpeechPad.Action0();
                }
            }
            if (Input.GetButtonDown("Speak2"))
            {
                GetComponentInChildren<Speech>().Speak(1);
                if (currentSpeechPad)
                {
                    currentSpeechPad.caller = this.gameObject;
                    currentSpeechPad.Action1();
                }
            }
            if (Input.GetButtonDown("Speak3"))
            {
                GetComponentInChildren<Speech>().Speak(2);
                if (currentSpeechPad)
                {
                    currentSpeechPad.caller = this.gameObject;
                    currentSpeechPad.Action2();
                }
            }
            if (Input.GetButtonDown("Speak4"))
            {
                GetComponentInChildren<Speech>().Speak(3);
                if (currentSpeechPad)
                {
                    currentSpeechPad.caller = this.gameObject;
                    currentSpeechPad.Action3();
                }
            }

            /*
            if (Input.GetKeyDown("1")) playerSprite.playerColor = PlayerSprite.PlayerColor.Black;
            if (Input.GetKeyDown("2")) playerSprite.playerColor = PlayerSprite.PlayerColor.Blue;
            if (Input.GetKeyDown("3")) playerSprite.playerColor = PlayerSprite.PlayerColor.Red;
            */

            if (Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(new string[] { "Death" })))
            {
                if (timeOutOfLine == 0)
                {
                    World.Instance.PlaySound(World.Clip.Exclamation);
                }
                timeOutOfLine += Time.deltaTime;
                if (timeOutOfLine > timeUntilCaught)
                {
                    World.Instance.Freeze();
                }
                float exValue = timeOutOfLine / timeUntilCaught;
                foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, size.x * 7, LayerMask.GetMask(new string[] { "NPC" })))
                {
                    ExclamationPoint ex = collider.GetComponentInChildren<ExclamationPoint>();
                    ex.SetStatus(true, exValue);
                }
                exclamation.SetStatus(true, exValue);
            }
            else if (Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(new string[] { "MoveDeath" })) && moving)
            {
                exclamation.SetStatus(true, 1);
                World.Instance.Freeze();
            }
            else
            {
                if (timeOutOfLine != 0)
                {
                    // Remove all exclamation points
                    foreach (ExclamationPoint ex in FindObjectsOfType<ExclamationPoint>())
                    {
                        ex.SetStatus(false, 0);
                    }
                }

                timeOutOfLine = 0;
            }

            goalCheck();

            if (Input.GetKeyDown(KeyCode.R))
            {
                frozen = true;
                World.Instance.StartFade(true, 0, 0);
            }

            // Sacrifice trigger
            if (Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(new string[] { "SacrificeTrigger" })))
            {
                gameObject.AddComponent<Sacrifice>();
                frozen = true;
                playerSprite.Animate(Vector2.zero);
                World.Instance.PlaySound(World.Clip.Sacrifice);
                World.Instance.StartFade(false, World.Instance.GetTitleScene(), 6.0f);
            }
        }
    }

    // Moves the Player or an NPC while colliding with walls and NPCs.
    public static void Move(GameObject obj, Vector2 delta)
    {
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
            gateObject.GetComponent<Animator>().SetTrigger("gateOpen");
            lockMovement = true;
        }
    }

    public void giveKey()
    {
        hasKey = true;
        GetComponentInChildren<Key>().SetStatus(true);
    }

    public void die()
    {
        exclamation.SetStatus(true, 1);
        World.Instance.Freeze();
    }
}
