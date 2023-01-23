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
        if (Time.frameCount % 10 == 0)
            UpdateHintVisibilityAndText();
    }

    private void UpdateHintVisibilityAndText() {
        if (pc.NPCdetected) {
            //hintText.text = "Press " + pc.getBinding()..
            hintText.text = "Press F to talk with nearby character";
            hintText.enabled = true;
            hintBackground.enabled = true;
        }
        if (pc.ItemDetected) {
            hintText.text = "Press E to pickup nearby item";
            hintText.enabled = true;
            hintBackground.enabled = true;
        }
        else if (!pc.ItemDetected && !pc.NPCdetected) {
            hintText.enabled = false;
            hintBackground.enabled = false;
        }
    }
}
