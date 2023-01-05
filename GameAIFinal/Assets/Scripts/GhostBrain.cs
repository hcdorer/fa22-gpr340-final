/* Pac-Man ghost AI behavior is based on this article:
 * https://gameinternals.com/understanding-pac-man-ghost-behavior
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostBrain : MonoBehaviour {
    private bool chase = false;
    private bool frightened = false;

    private const float CHASE_TIMER_START = 20;
    private const float SCATTER_TIMER_START_PHASE_0 = 7;
    private const float SCATTER_TIMER_START_PHASE_4 = 5;
    private const float FRIGHTENED_TIMER_START = 5;

    private int phase = 0;
    private float phaseTimer = SCATTER_TIMER_START_PHASE_0;
    private float frightenedTimer = FRIGHTENED_TIMER_START;

    private Color defaultColor;
    [SerializeField] private Color frightenedColor;

    [SerializeField] private Square scatterCorner;
    protected Square ScatterCorner { get => scatterCorner; }
    [SerializeField] private Square scatterOpposite;
    private bool targetOpposite = false;

    private Square lastKnownCrossroads;
    protected Square LastKnownCrossroads { get => lastKnownCrossroads; }
    private Square playerSquare { get => FindObjectOfType<PacManInput>().GetComponent<CharacterMovement>().square; }

    private bool stuck = false;
    private bool stuckThisFrame = false;

    private CharacterMovement movement;
    protected CharacterMovement Movement { get => movement; }
    private AIPathfind pathfind;
    protected AIPathfind Pathfind { get => pathfind; }
    private SpriteRenderer sRenderer;

    private void Awake() {
        movement = GetComponent<CharacterMovement>();
        pathfind = GetComponent<AIPathfind>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        foreach(Crossroads crossroads in FindObjectsOfType<Crossroads>()) {
            crossroads.crossroadsReachedEvent += onCrossroadsReached;
        }

        foreach(PowerDot powerDot in FindObjectsOfType<PowerDot>()) {
            powerDot.powerDotCollectedEvent += onPowerDotCollected;
        }
    }

    private void Start() {
        defaultColor = sRenderer.color;

        setPathFromBehavior(false);
    }

    private void Update() {
        if(frightened) {
            updateFrightened();
        } else {
            updatePhase();
        }

        stuckThisFrame = false;
    }

    private void OnDisable() {
        foreach(Crossroads crossroads in FindObjectsOfType<Crossroads>()) {
            crossroads.crossroadsReachedEvent -= onCrossroadsReached;
        }

        foreach(PowerDot powerDot in FindObjectsOfType<PowerDot>()) {
            powerDot.powerDotCollectedEvent -= onPowerDotCollected;
        }
    }

    private void advancePhase() {
        phase++;
        setupPhase();
    }

    private void repeatPhase() {
        phase--;
        setupPhase();
    }

    private void setupPhase() {
        if(phase % 2 == 0) {
            chase = false;
            phaseTimer = phase > 4 ? SCATTER_TIMER_START_PHASE_4 : SCATTER_TIMER_START_PHASE_0;
            targetOpposite = false;
            setPathFromBehavior(false);
        } else {
            chase = true;
            phaseTimer = CHASE_TIMER_START;
            if(lastKnownCrossroads != null) {
                setPathFromBehavior(false);
            } else {
                repeatPhase();
            }
        }
    }

    private void updatePhase() {
        phaseTimer -= Time.deltaTime;
        if(phaseTimer <= 0 && phase < 7) {
            advancePhase();
        }
    }

    private void updateFrightened() {
        frightenedTimer -= Time.deltaTime;
        if(frightenedTimer <= 0) {
            frightened = false;
            sRenderer.color = defaultColor;
            setupPhase();
        }
    }

    private void onCrossroadsReached(CrossroadsReachedEventArgs e) {
        lastKnownCrossroads = e.square;

        if(!frightened) {
            setPathFromBehavior(false);
        }
    }

    private void setPathFromBehavior(bool chasePlayer) {
        if(chase) {
            pathfind.setPath(chasePlayer ? playerSquare : getChaseTarget());
        } else {
            pathfind.setPath(targetOpposite ? scatterOpposite : scatterCorner);
        }
    }

    private void chooseRandomDirection(Square square, bool prioritizeCurrentDirection) {
        List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        bool validDirection = false;
        Vector2Int nextDirection = Vector2Int.zero;
        Vector2Int nextPosition = Vector2Int.zero;
        Square next;

        if(prioritizeCurrentDirection) {
            if(directions.Contains(movement.Direction)) {
                directions.Remove(movement.Direction);
            }

            nextDirection = movement.Direction;
            nextPosition = square.GridPosition + nextDirection;
            next = Square.getSquareAt(movement.LevelGrid.CellToWorld(new Vector3Int(nextPosition.x, nextPosition.y, 0)));
            validDirection = CharacterMovement.canMoveIntoSquare(next, nextDirection);
        }

        while(!validDirection) {
            int roll = UnityEngine.Random.Range(0, directions.Count - 1);

            nextDirection = directions[roll];
            nextPosition = square.GridPosition + nextDirection;
            next = Square.getSquareAt(movement.LevelGrid.CellToWorld(new Vector3Int(nextPosition.x, nextPosition.y, 0)));
            validDirection = CharacterMovement.canMoveIntoSquare(next, nextDirection);
            directions.Remove(directions[roll]);
        }

        movement.Direction = nextDirection;
    }

    public void getUnstuck() {
        if(!frightened) {
            stuck = true;
            stuckThisFrame = true;
        }

        chooseRandomDirection(movement.square, true);
    }

    public void remakePath() {
        if(stuck && !stuckThisFrame) {
            setPathFromBehavior(false);
            stuck = false;
        }
    }

    public void updatePath() {
        if(!chase) {
            targetOpposite = !targetOpposite;
        }
        setPathFromBehavior(true);
    }

    private void onPowerDotCollected() {
        frightened = true;
        frightenedTimer = FRIGHTENED_TIMER_START;
        sRenderer.color = frightenedColor;
        pathfind.stop();
    }

    public void changeDirection(Square crossroads) {
        if(frightened) {
            chooseRandomDirection(crossroads, false);
        }
    }

    protected abstract Square getChaseTarget();
}
