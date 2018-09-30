using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorDetect : SpeechPad {

    private Inspector inspector;
    public void Start() {
        inspector = transform.parent.GetComponent<Inspector>();
    }

    override public void Action0() {
        Debug.Log("Action0");
        inspector.PlayerSpeak(0);
    }
    override public void Action1() {
        inspector.PlayerSpeak(1);
    }
    override public void Action2() {
        inspector.PlayerSpeak(2);
    }
    override public void Action3() {
        inspector.PlayerSpeak(3);
    }
}
