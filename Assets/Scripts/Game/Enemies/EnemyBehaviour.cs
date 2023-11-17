using System;
using System.Collections.Generic;
using Game.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    public int damage = 5;
    // States on how exactly our enemies will go forward (Attack, walking)
    public enum EnemyStates
    {
        Attacking,
        Walking,
        Idle
    }

    public Vector3 EndTargetPosition;
    public Transform RootPosition;
    public float StopRadius;
    public bool reached = false; // Has destination been reached
    public bool attacking = false;
    public IAttackable attackTarget;
    public bool attackEndTarget = false;
    private Vector2 finishAttackMove;
    [SerializeField]
    private EnemyStates state = EnemyStates.Idle;
    
    public float WalkSpeed = 5.0f;

    private void Awake()
    {
        var sprite = this.GetComponent<SpriteRenderer>();
        if (sprite)
        {
            //sprite.color = Random.ColorHSV();
        }

        //this.WalkSpeed = 0.5f + (Random.value * 5);
    }

    public void Update()
    {
        switch (state)
        {
            case EnemyStates.Attacking:
                attacking = true;

                if (attackTarget != null && !attackTarget.isDead())
                {
                    attackTarget.Attack(damage);


                    if (attackTarget.isDead())
                    {
                        FinishAttack();
                    }
                }
                else
                {
                    FinishAttack();
                }
                
                // TODO: Do stuff with attack
                    // Chunk health every so often
                break;
            case EnemyStates.Walking:
                var currentPosition = RootPosition.position;
                var endGoalPosition = EndTargetPosition;

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
    public void SetWalkTarget(Vector2 position, Transform target = null)
    {
        EndTargetPosition = position;
        this.state = EnemyStates.Walking;
        this.reached = false;
    }

    public void SetAttackTarget(IAttackable attackable, Vector2 startAttackPosition, Vector2 finishAttackMove)
    {
        // TODO: Set what the end target's entity is so this can attack it properly
        finishAttackMove = finishAttackMove;
        EndTargetPosition= attackable.GetWorldPosition(); // TODO: Change this line
        this.state = EnemyStates.Walking;
        attackEndTarget = true;
    }


    private int counter = 0;
    public void SetPath(List<Vector2Int> path)
    {
        counter++;
        Debug.Log("Got new path " + path + " processed " + counter + " jobs");
    }

    public Vector2Int GetGridPosition()
    {
        return new Vector2Int(Mathf.FloorToInt(this.transform.position.x), Mathf.FloorToInt(this.transform.position.y));
    }

    private void FinishAttack()
    {
        this.attackTarget = null;
        this.attacking = false;
        this.attackEndTarget = false;
        this.SetWalkTarget(finishAttackMove);
    }


    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;

    public void SetHealth(int health)
    {
        this.health = health;
    }
    public float GetHealth()
    {
        return health;
    }

    public void Damage(int amount)
    {
        health -= amount;
        
        // Kill
        if(health <= 0) Kill();
    }

    public void SetPosition(Vector3 worldPosition)
    {
        this.RootPosition.transform.position = worldPosition;
    }

    public void Kill()
    {
        Destroy(this.RootPosition.gameObject);
    }
    
    
}
