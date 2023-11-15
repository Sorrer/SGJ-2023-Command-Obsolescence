using System;
using System.Collections.Generic;
using Game.Enemies;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    // States on how exactly our enemies will go forward (Attack, walking)
    public enum EnemyStates
    {
        Attacking,
        Walking,
        Idle
    }

    public Transform EndTarget;
    public Transform RootPosition;
    public float StopRadius;
    public bool reached = false; // Has destination been reached
    public bool attacking = false;
    public bool attackEndTarget = false;
    [SerializeField]
    private EnemyStates state = EnemyStates.Idle;
    
    public float WalkSpeed = 5.0f;

    public void Update()
    {
        switch (state)
        {
            case EnemyStates.Attacking:
                // TODO: Do stuff with attack
                    // Chunk health every so often
                break;
            case EnemyStates.Walking:
                var currentPosition = RootPosition.position;
                var endGoalPosition = EndTarget.position;

                var directionVector = endGoalPosition - currentPosition;
                var mag = directionVector.magnitude;

                if (mag < StopRadius)
                {
                    // We have finished our walking
                    // TODO: If this enemy wants to attack, then we would have to specify what happens during an attack here
                    this.reached = true;

                    if (attackEndTarget)
                    {
                        this.state = EnemyStates.Attacking;
                    }
                    else
                    {
                        this.state = EnemyStates.Idle;
                    }
                    break;
                }

                directionVector = directionVector / mag;
                
                RootPosition.position = currentPosition + (directionVector * (WalkSpeed * Time.deltaTime));
                
                
                break;
            case EnemyStates.Idle:
                // He do nothing, cause he can't do anything, he need help
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    /// <summary>
    /// Used by an exterior system to plan this enemy's path
    /// </summary>
    /// <param name="target"></param>
    public void SetWalkTarget(Transform target)
    {
        EndTarget = target;
        this.state = EnemyStates.Walking;
        this.reached = false;
    }

    public void SetAttackTarget(Transform target)
    {
        // TODO: Set what the end target's entity is so this can attack it properly
        EndTarget = target;
        this.state = EnemyStates.Walking;
        attackEndTarget = true;
    }


    private int counter = 0;
    public void SetPath(List<Vector2Int> path)
    {
        counter++;
        Debug.Log("Got new path " + path + " processed " + counter + " jobs");
    }

}
