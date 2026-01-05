using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private PlayerController player;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float damages = 8f;
    [SerializeField] public Window assignatedWindow;
    [SerializeField] private float windowTraverseDuration;
    [SerializeField] private GameManager gameManager;
    
    private NavMeshAgent _navMeshAgent;
    private float _attackTimer = 0f;
    private float _health;
    private bool _isTraversingWindow;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = maxHealth;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isTraversingWindow) return;
        
        _attackTimer -= Time.deltaTime;
        if (Vector3.Distance(player.transform.position, transform.position) < 3f)
        {
            if (_attackTimer < 0f)
            {
                Attack();
                _attackTimer = attackDelay;
            }
        }
        else
        {
            HandleMovement();
        }
    }
    
    private void HandleMovement()
    {
        // Si une fenêtre est assignée et qu'on n'est pas encore passé
        if (assignatedWindow)
        {
            float dist = Vector3.Distance(transform.position, assignatedWindow.start.position);

            if (dist > 0.5f)
            {
                _navMeshAgent.SetDestination(assignatedWindow.start.position);
            }
            else
            {
                if (_navMeshAgent.enabled && _navMeshAgent.isOnNavMesh)
                {
                    _navMeshAgent.isStopped = true;
                }
                StartCoroutine(TraverseWindow());
            }
        }
        else
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(player.transform.position);
        }
    }
    
    private IEnumerator TraverseWindow()
    {
        _isTraversingWindow = true;
        
        if (_navMeshAgent.enabled && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = true;
        }

        _navMeshAgent.enabled = false;

        Vector3 startPos = assignatedWindow.start.position;
        Vector3 endPos = assignatedWindow.end.position;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / windowTraverseDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        _navMeshAgent.enabled = true;
        _navMeshAgent.Warp(endPos);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.stoppingDistance = 3f;

        assignatedWindow.FinishedTraversal();
        assignatedWindow = null;
        _isTraversingWindow = false;
    }

    public void TakeDamage(float damage)
    {
        player.AddMoney();
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (assignatedWindow)
        {
            assignatedWindow.AssignedEnemyKilled(this);
            if (_isTraversingWindow)
            {
                assignatedWindow.FinishedTraversal();
            }
        }
       
        gameManager.EnemyKilled();
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
