using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Transform player;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float damages = 8f;

    private NavMeshAgent _navMeshAgent;
    private float _attackTimer = 0f;
    private float _health;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = maxHealth;
    }
    
    // Update is called once per frame
    void Update()
    {
        _attackTimer -= Time.deltaTime;
        if (Vector3.Distance(player.position, transform.position) < 3f)
        {
            if (_attackTimer < 0f)
            {
                Attack();
                _attackTimer = attackDelay;
            }
        }
        else
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(player.position);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Attack()
    {
        Physics.Linecast(transform.position, transform.forward * attackDistance, out RaycastHit hit);
        Debug.DrawLine(transform.position, transform.forward * attackDistance, Color.blue, 5);
        
        if (hit.collider && hit.collider.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TryGetComponent<IDamageable>(out IDamageable damageable);
            damageable.TakeDamage(damages);
        }
    }
}
