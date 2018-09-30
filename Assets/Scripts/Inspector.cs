using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : NPC {

    private Player nearPlayer = null;
    //private float leftRadiusTime;
    private ExclamationPoint exclamation;
    private InspectorDetect detect;
    private bool playerWasCorrect = false;
    public float radius = 200f;

    public int[] symbolCycle;
    private int cyclePos = 0;
    private int correctSymbol = 0;

    private void Start() {
        base.Start();
        exclamation = GetComponentInChildren<ExclamationPoint>();
        detect = GetComponentInChildren<InspectorDetect>();
        correctSymbol = symbolCycle[0];
        detect.validActions = new int[] { correctSymbol };
    }

    private void Update() {
        Collider2D playerInRadius = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask(new string[] { "Player" }));

        if (playerInRadius) {
            nearPlayer = playerInRadius.GetComponent<Player>();
            //leftRadiusTime = Time.time;
        }
        else {
            if (nearPlayer != null && !playerWasCorrect && !nearPlayer.frozen) {// && Time.time > leftRadiusTime + 0.25f) {
                World.Instance.Freeze();
                exclamation.SetStatus(true, 1);
            }
        }

        base.Update();
    }

    public void Speak(int symbol, bool isPlayer) {
        if (symbol == correctSymbol && !(isPlayer && playerWasCorrect)) {
            if (isPlayer) playerWasCorrect = true;
            cyclePos = (cyclePos + 1) % symbolCycle.Length;
            correctSymbol = symbolCycle[cyclePos];
            detect.validActions[0] = correctSymbol;
        }
        else {
            if (nearPlayer != null && isPlayer) {
                World.Instance.Freeze();
                exclamation.SetStatus(true, 1);
                speed = 0;
            }
        }
    }

    // Copied from NPC... i'm sorry rick ord
    void populateDirections() {
        directions[(int)Directions.DOWN] = new Vector2(0, -1).normalized;
        directions[(int)Directions.LEFT_DOWN] = new Vector2(-1, -1).normalized;
        directions[(int)Directions.LEFT_UP] = new Vector2(-1, 1).normalized;
        directions[(int)Directions.LEFT] = new Vector2(-1, 0).normalized;
        directions[(int)Directions.RIGHT_DOWN] = new Vector2(1, -1).normalized;
        directions[(int)Directions.RIGHT_UP] = new Vector2(1, 1).normalized;
        directions[(int)Directions.RIGHT] = new Vector2(1, 0).normalized;
        directions[(int)Directions.UP] = new Vector2(0, 1).normalized;
    }
}
