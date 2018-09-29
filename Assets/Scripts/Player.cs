using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private bool frozen = false;
    private float timeOutOfLine = 0;
    private const float timeUntilCaught = 1f;

    [SerializeField] private AnimationClip idleAnim;
    [SerializeField] private AnimationClip walkAnim;

    void Start() {
		
	}
	
	void FixedUpdate() {
        if (frozen) return;

        float moveSpeed = 300f * Time.deltaTime;
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        Vector3 toMove = new Vector3(horiz * moveSpeed, vert * moveSpeed);
        Move(gameObject, toMove);

        if (horiz == 0 && vert == 0) {
            GetComponent<Animator>().Play("Idle");
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else {
            GetComponent<Animator>().Play("WalkRight");
            GetComponent<SpriteRenderer>().flipX = (horiz < 0);
        }

        if (Input.GetButtonDown("Speak1")) {
            GetComponentInChildren<Speech>().Speak(0);
        }
        if (Input.GetButtonDown("Speak2")) {
            GetComponentInChildren<Speech>().Speak(1);
        }
        if (Input.GetButtonDown("Speak3")) {
            GetComponentInChildren<Speech>().Speak(2);
        }
        if (Input.GetButtonDown("Speak4")) {
            GetComponentInChildren<Speech>().Speak(3);
        }

        Vector2 size = GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        if (Physics2D.OverlapBox(transform.position, size, 0, LayerMask.GetMask(new string[] { "Death" }))) {
            timeOutOfLine += Time.deltaTime;
            if (timeOutOfLine > timeUntilCaught) {
                frozen = true;
                World.Instance.StartFade();
            }

            GetComponentInChildren<ExclamationPoint>().SetStatus(true, timeOutOfLine / timeUntilCaught);
        }
        else {
            timeOutOfLine = 0;
            GetComponentInChildren<ExclamationPoint>().SetStatus(false, 0);
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
}
