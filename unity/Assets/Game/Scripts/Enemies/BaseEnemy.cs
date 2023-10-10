using System;
using Cubeshift;
using DG.Tweening;
using Frictionless;
using Game.Scripts;
using ToolBox.Pools;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour, IEnemy
{
    public NavMeshAgent NavMeshAgent;

    public GameObject Target;
    public GameObject DieFx;
    public GameObject Graphics;

    public float CurrentHealth;
    public float StartHealth;
    public int Xp;
    public int Damage = 1;
    public float Cooldown = 0.5f;
    public float Range = 1f;
    public bool ImmuneToPushback = false;
    public bool IsBoss = false;
    public float lastAttack = Single.MinValue;
    public float lastPushback = Single.MinValue;
    
    private float lastNavMeshUpdate = Single.MinValue;

    void Start()
    {
        if (NavMeshAgent.isActiveAndEnabled)
        {
            NavMeshAgent.SetDestination(Target.transform.position);   
        }
    }

    private void OnEnable()
    {
        Graphics.transform.localScale = Vector3.zero;
        Graphics.transform.DOScale(Vector3.one, 0.5f);
    }

    public void Hit(float damage, GameObject opponent)
    {
        if (!ImmuneToPushback && lastPushback + 0.5f < Time.time)
        {
            Vector3 distanceVector = transform.position - opponent.transform.position;
            distanceVector.y = 0;
            transform.position += distanceVector.normalized * 0.5f;
            lastPushback = Time.time;
        } 
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f);
        CurrentHealth -= damage;
        ServiceFactory.Resolve<BlimpManager>().SpawnBlimp(transform.position, damage.ToString(), Color.white);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        var enemyManager = ServiceFactory.Resolve<EnemyManager>();
        enemyManager.SpawnXpBlobByEnemy(this);
        enemyManager.RemoveEnemy(this);
        gameObject.Release();
        var hitFx = DieFx.Reuse();
        hitFx.transform.position = transform.position;
        if (IsBoss)
        {
            ServiceFactory.Resolve<GameController>().GameOver(true);
        }
    }

    public void OnUpdate()
    {
        if (NavMeshAgent.isActiveAndEnabled && lastNavMeshUpdate + 0.3f < Time.time)
        {
            NavMeshAgent.SetDestination(Target.transform.position);
            lastNavMeshUpdate = Time.time;
        }

        if (lastAttack + Cooldown < Time.time)
        {
            var playerController = ServiceFactory.Resolve<PlayerController>();
            if ((transform.position - playerController.transform.position).magnitude < Range)
            {
                playerController.Hit(Damage);
                Graphics.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.14f);
                lastAttack = Time.time;
            }
        }
    }
}
