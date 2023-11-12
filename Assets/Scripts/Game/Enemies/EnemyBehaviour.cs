using System;
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
    public bool reached; // Has destination been reached

    [SerializeField]
    private EnemyStates state;
    
    public float WalkSpeed = 5.0f;

    public void Update()
    {
        switch (state)
        {
            case EnemyStates.Attacking:
                
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
                    this.state = EnemyStates.Idle;
                    this.reached = true;
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

}
