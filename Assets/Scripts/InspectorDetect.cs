using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorDetect : SpeechPad {

    private Inspector inspector;
    public void Start() {
        inspector = transform.parent.GetComponent<Inspector>();
    }

    override public void Action0() {
        if (caller.GetComponent<Player>() != null) inspector.PlayerSpeak(0);
    }
    override public void Action1() {
        if (caller.GetComponent<Player>() != null) inspector.PlayerSpeak(1);
    }
    override public void Action2() {
        if (caller.GetComponent<Player>() != null) inspector.PlayerSpeak(2);
    }
    override public void Action3() {
        if (caller.GetComponent<Player>() != null) inspector.PlayerSpeak(3);
    }
}
