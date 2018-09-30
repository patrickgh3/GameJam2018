using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorDetect : SpeechPad {

    private Inspector inspector;
    public void Start() {
        inspector = transform.parent.GetComponent<Inspector>();
    }

    override public void Action0() {
        inspector.Speak(0, caller.GetComponent<Player>() != null);
    }
    override public void Action1() {
        inspector.Speak(1, caller.GetComponent<Player>() != null);
    }
    override public void Action2() {
        inspector.Speak(2, caller.GetComponent<Player>() != null);
    }
    override public void Action3() {
        inspector.Speak(3, caller.GetComponent<Player>() != null);
    }
}
