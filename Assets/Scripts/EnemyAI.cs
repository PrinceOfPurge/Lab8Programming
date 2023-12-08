using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public enum EnemyState
    {
        Start,
        Patrol,
        Follow,
        CloseToPlayer
    }
    
    public Transform[] Path;
    public EnemyState MyState;
    public GameObject Player;
    public Coroutine CurrentBehaviour;
    public float StartFollowDistance;
    public float FollowSpeed; 
    public float StartCloseToPlayerDistance;
    public float CurrentDistance;
    public float PatrolSpeed;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        CurrentDistance = Vector3.Distance(Player.transform.position, transform.position);
        if (CurrentDistance > StartFollowDistance && MyState != EnemyState.Patrol)
        {
            UpdateBehaviour(EnemyState.Patrol); 
        }
        else if (CurrentDistance <= StartFollowDistance && MyState != EnemyState.Follow)
        {
            UpdateBehaviour(EnemyState.Follow);
        }
        else if (CurrentDistance <= StartCloseToPlayerDistance && MyState != EnemyState.CloseToPlayer)
        {
            UpdateBehaviour(EnemyState.CloseToPlayer);
        }
    }

    private void UpdateBehaviour(EnemyState state)
    {
        MyState = state;
        if (CurrentBehaviour != null)
        {
            StopCoroutine(CurrentBehaviour);
        }

        switch (MyState)
        {
            case EnemyState.Patrol:
                CurrentBehaviour = StartCoroutine(Patrol());
                break;
            case EnemyState.Follow:
                CurrentBehaviour = StartCoroutine(Follow());
                break;
            case EnemyState.CloseToPlayer:
                break;
        }
    }

    public IEnumerator Follow()
    {
            while (true)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, FollowSpeed * Time.deltaTime);
                yield return null; 
            }
    }

    public IEnumerator Patrol()
    {
        int closestPathPoint = 0;

        for (int i = 0; i < Path.Length; i++)
        {
            if (Vector2.Distance(transform.position, Path[i].position) < Vector2.Distance(transform.position, Path[closestPathPoint].position))
            {
                closestPathPoint = i; 
            }
        }

        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, Path[closestPathPoint].position, PatrolSpeed * Time.deltaTime);

            if ((Vector2)transform.position == (Vector2)Path[closestPathPoint].position)
            {
                if (closestPathPoint == Path.Length - 1)
                {
                    closestPathPoint = 0;
                }
                else
                {
                    closestPathPoint++;
                }
            }

            yield return null; 
        }
    }

    public IEnumerator Attack()
    {
        while (true)
        {
            yield return null;
        }
    }
    
}
