/* Pac-Man ghost AI behavior is based on this article:
 * https://gameinternals.com/understanding-pac-man-ghost-behavior
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBrain : MonoBehaviour {
    protected enum Behavior {
        CHASE,
        SCATTER,
        FRIGHTENED
    }
    
    private const float CHASE_TIMER_START = 20;
    private const float SCATTER_TIMER_START_PHASE_0 = 7;
    private const float SCATTER_TIMER_START_PHASE_4 = 5;
    
    private int phase = 0;
    protected Behavior behavior = Behavior.SCATTER;
    private float phaseTimer = SCATTER_TIMER_START_PHASE_0;

    [SerializeField] private Square scatterCorner;
    [SerializeField] private Square scatterOpposite;
    bool targetOpposite = false;

    private Square lastKnownCrossroads;
    private Square playerSquare { get => FindObjectOfType<PacManInput>().GetComponent<CharacterMovement>().square; }

    private AIPathfind pathfind;

    private void Start() {
        pathfind = GetComponent<AIPathfind>();
        pathfind.setPath(scatterCorner);
    }

    private void OnEnable() {
        foreach(Crossroads crossroads in FindObjectsOfType<Crossroads>()) {
            crossroads.crossroadsReachedEvent += onCrossroadsReached;
        }
    }

    private void Update() {
        phaseTimer -= Time.deltaTime;
        if(phaseTimer <= 0 && phase < 7) {
            advancePhase();
        }
    }

    private void advancePhase() {
        phase++;

        if(phase % 2 == 0) {
            behavior = Behavior.SCATTER;
            phaseTimer = phase > 4 ? SCATTER_TIMER_START_PHASE_4 : SCATTER_TIMER_START_PHASE_0;
            pathfind.setPath(scatterCorner);
        } else {
            behavior = Behavior.CHASE;
            phaseTimer = CHASE_TIMER_START;
            if(lastKnownCrossroads != null) {
                pathfind.setPath(lastKnownCrossroads);
            } else {
                behavior = Behavior.SCATTER;
            }
        }
    }

    private void onCrossroadsReached(object sender, EventArgs e) {
        Crossroads crossroadsSender = (Crossroads)sender;

        lastKnownCrossroads = crossroadsSender.square;
        if(behavior == Behavior.CHASE) {
            pathfind.setPath(lastKnownCrossroads);
        }
    }

    public void onTargetReached() {
        switch(behavior) {
            case Behavior.SCATTER:
                targetOpposite = !targetOpposite;
                pathfind.setPath(targetOpposite ? scatterOpposite : scatterCorner);
                break;
            case Behavior.CHASE:
                pathfind.setPath(playerSquare);
                break;
        }
    }
}
