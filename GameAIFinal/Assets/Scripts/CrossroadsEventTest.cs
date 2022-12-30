using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossroadsEventTest : MonoBehaviour {
    private void OnEnable() {
        foreach(Crossroads crossroads in FindObjectsOfType<Crossroads>()) {
            crossroads.crossroadsReachedEvent += onCrossroadsReached;
        }
    }

    private void OnDisable() {
        foreach(Crossroads crossroads in FindObjectsOfType<Crossroads>()) {
            crossroads.crossroadsReachedEvent -= onCrossroadsReached;
        }
    }

    private void onCrossroadsReached(object sender, EventArgs e) {
        Crossroads senderCrossroads = (Crossroads)sender;
    }
}
