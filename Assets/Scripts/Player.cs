using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	void Start() {
		
	}
	
	void FixedUpdate() {
        float moveSpeed = 300f * Time.deltaTime;
        Vector3 toMove = new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);
        toMove = new Vector3(Mathf.Round(toMove.x), Mathf.Round(toMove.y));

        transform.position += toMove;

        Vector2 size = new Vector2(100, 100);
        int wallsLayer = 1 << 9;

        if (Physics2D.OverlapBox(transform.position, size, 0, wallsLayer)) {
            transform.position -= toMove;

            for (int i = 0; i < Mathf.Abs(toMove.x); i++) {
                transform.position += new Vector3(Mathf.Sign(toMove.x), 0);
                if (Physics2D.OverlapBox(transform.position, size, 0, wallsLayer)) {
                    transform.position -= new Vector3(Mathf.Sign(toMove.x), 0);
                    break;
                }
            }

            for (int i = 0; i < Mathf.Abs(toMove.y); i++) {
                transform.position += new Vector3(0, Mathf.Sign(toMove.y));
                if (Physics2D.OverlapBox(transform.position, size, 0, wallsLayer)) {
                    transform.position -= new Vector3(0, Mathf.Sign(toMove.y));
                    break;
                }
            }
        }
    }
}
