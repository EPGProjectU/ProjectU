using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHint : MonoBehaviour
{
    PlayerController pc;
    Text hintText;
    Image hintBackground;
    private void Start() {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        hintText = this.gameObject.GetComponent<Text>();
        hintBackground = this.gameObject.GetComponentInParent<Image>();

    }

    private void Update() {
        if (pc.NPCdetected)
            //hintText.text = "Press " + pc.getBinding()..
            hintText.text = "Press F to talk with nearby character";
        if (pc.ItemDetected)
            hintText.text = "Press E to pickup nearby item";
        else if (!pc.ItemDetected && !pc.NPCdetected) {
            hintText.enabled = false;
            hintBackground.enabled = false;
        }
    }
}
