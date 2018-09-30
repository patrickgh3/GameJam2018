using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour {

    public enum PlayerColor {
        Black,
        Blue,
        Red,
    }
    public PlayerColor playerColor = PlayerColor.Black;

    public void Animate(Vector2 velocity) {
        if (velocity.x == 0 && velocity.y == 0) {
            string clip = "BlackIdle";
            if (playerColor == PlayerColor.Blue) clip = "BlueIdle";
            if (playerColor == PlayerColor.Red) clip = "RedIdle";
            GetComponent<Animator>().Play(clip);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else {
            string clip = "BlackWalk";
            if (playerColor == PlayerColor.Blue) clip = "BlueWalk";
            if (playerColor == PlayerColor.Red) clip = "RedWalk";
            GetComponent<Animator>().Play(clip);
            GetComponent<SpriteRenderer>().flipX = (velocity.x < 0);
        }
    }
}
