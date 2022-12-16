using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinky : GhostBrain {
    protected override Square getChaseTarget() {
        return lastKnownCrossroads;
    }
}
