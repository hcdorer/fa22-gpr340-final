using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour 
{
    private Vector2 direction;
    private Vector2 targetGridPosition;

    [SerializeField] private Grid levelGrid;

    private void lerp()
    {
        transform.position = Vector2.MoveTowards(transform.position)
    }
}
