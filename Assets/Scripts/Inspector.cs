using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspector : MonoBehaviour {

    private Player nearPlayer = null;
    //private float leftRadiusTime;
    private ExclamationPoint exclamation;
    private InspectorDetect detect;
    private bool playerWasCorrect = false;
    public float radius = 200f;

    public int[] symbolCycle;
    private int cyclePos = 0;
    private int correctSymbol = 1;

    private void Start() {
        exclamation = GetComponentInChildren<ExclamationPoint>();
        detect = GetComponentInChildren<InspectorDetect>();
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
                nearPlayer.frozen = true;
                World.Instance.StartFade(true, "");
                exclamation.SetStatus(true, 1);
            }
        }
    }

    public void PlayerSpeak(int symbol) {
        if (symbol == correctSymbol) {
            playerWasCorrect = true;
            cyclePos = (cyclePos + 1) % symbolCycle.Length;
            correctSymbol = symbolCycle[cyclePos];
            detect.validActions[0] = correctSymbol;
        }
        else {
            nearPlayer.frozen = true;
            World.Instance.StartFade(true, "");
            exclamation.SetStatus(true, 1);
        }
    }
}
