/* Pac-Man ghost AI behavior is based on this article:
 * https://gameinternals.com/understanding-pac-man-ghost-behavior
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostBrain : MonoBehaviour {
    private enum Behavior {
        CHASE,
        SCATTER,
        FRIGHTENED
    }
    
    private const float CHASE_TIMER_START = 20;
    private const float SCATTER_TIMER_START_PHASE_0 = 7;
    private const float SCATTER_TIMER_START_PHASE_4 = 5;
    
    private int phase = 0;
    private Behavior behavior = Behavior.SCATTER;
    private float phaseTimer = SCATTER_TIMER_START_PHASE_0;

    [SerializeField] private Square scatterCorner;
    [SerializeField] private Square scatterOpposite;
    private bool targetOpposite = false;

    protected Square lastKnownCrossroads;
    private Square playerSquare { get => FindObjectOfType<PacManInput>().GetComponent<CharacterMovement>().square; }

    private bool stuck = false;
    private bool stuckThisFrame = false;

    private CharacterMovement movement;
    protected AIPathfind pathfind;

    private void Start() {
        movement = GetComponent<CharacterMovement>();
        pathfind = GetComponent<AIPathfind>();

        setPathFromBehavior();
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

        stuckThisFrame = false;
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
                pathfind.setPath(getChaseTarget());
            } else {
                behavior = Behavior.SCATTER;
            }
        }
    }

    private void onCrossroadsReached(object sender, EventArgs e) {
        Crossroads crossroads = (Crossroads)sender;

        lastKnownCrossroads = crossroads.square;
        if(behavior == Behavior.CHASE) {
            pathfind.setPath(getChaseTarget());
        }
    }

    private void setPathFromBehavior() {
        switch(behavior) {
            case Behavior.SCATTER:
                pathfind.setPath(targetOpposite ? scatterOpposite : scatterCorner);
                break;
            case Behavior.CHASE:
                pathfind.setPath(playerSquare);
                break;
        }
    }

    protected abstract Square getChaseTarget();

    public void chooseRandomDirection() {
        stuck = true;
        stuckThisFrame = true;

        List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        if(directions.Contains(movement.Direction)) {
            directions.Remove(movement.Direction);
        }
        
        Vector2Int nextDirection = movement.Direction;
        Vector2Int nextTarget = movement.GridPosition + nextDirection;
        bool validDirection = movement.canMoveIntoSquare(Square.getSquareAt(pathfind.LevelGrid.CellToWorld(new Vector3Int(nextTarget.x, nextTarget.y, 0))));
        while(!validDirection) {
            int roll = UnityEngine.Random.Range(0, directions.Count - 1);

            nextDirection = directions[roll];
            nextTarget = movement.GridPosition + nextDirection;
            validDirection = movement.canMoveIntoSquare(Square.getSquareAt(pathfind.LevelGrid.CellToWorld(new Vector3Int(nextTarget.x, nextTarget.y, 0))));
            directions.Remove(directions[roll]);
        }

        movement.Direction = nextDirection;
    }

    public void remakePath() {
        if(stuck && !stuckThisFrame) {
            setPathFromBehavior();
            stuck = false;
        }
    }

    public void updatePath() {
        if(behavior == Behavior.SCATTER) {
            targetOpposite = !targetOpposite;
        }
        setPathFromBehavior();
    }
}
