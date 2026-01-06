using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private PlayerController player;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float attackDistance = 4f;
    [SerializeField] private float damages = 8f;
    [SerializeField] public Window assignatedWindow;
    [SerializeField] private float windowTraverseDuration;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private HealingItem healingItemPrefab;
    
    private NavMeshAgent _navMeshAgent;
    private float _attackTimer;
    private float _health;

    private bool _wentTroughWindow;
    // Seulement un ennemi par fenêtre doit traverser en même temps
    // Si la fenêtre est bloquée, il doit l'attaquer d'abord pour détruire les barricades
    private bool _mustTraverseWindow;
    private bool _isTraversingWindow;

    public bool MustTraverseWindow
    {
        set => _mustTraverseWindow = value;
    }

    private Animator _animator;
    private int _attackHash;
    private int _traverseWindowHash;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _health = maxHealth;
        
        _animator = GetComponentInChildren<Animator>();
        _attackHash = Animator.StringToHash("Attack");
        _traverseWindowHash = Animator.StringToHash("TraverseWindow");
    }
    
    // Update is called once per frame
    void Update()
    {
        _attackTimer -= Time.deltaTime;
        if (_isTraversingWindow) return;
        
        // Attaque le joueur s'il est asssez proche (et a passé sa fenêtre)
        if (_wentTroughWindow && Vector3.Distance(player.transform.position, transform.position) < 2f)
        {
            _navMeshAgent.enabled = false;
            transform.rotation =  Quaternion.LookRotation(player.transform.position - transform.position);
            if (_attackTimer < 0f)
            {
                StartCoroutine(Attack());
            }
        }
        // Gère le mouvement le reste du temps
        else
        {
            HandleMovement();
        }
    }
    
    private void HandleMovement()
    {
        // S'il est passé, il poursuit le joueur
        if (_wentTroughWindow)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(player.transform.position);
        }
        // Sinon il se déplace vers la fenêtre (ou la traverseà
        else
        {
            // Si c'est à son tour de traverser
            if (_mustTraverseWindow)
            {
                // Si il est loin du start il y va
                if (Vector3.Distance(transform.position, assignatedWindow.start.position) > 1f)
                {
                    _navMeshAgent.enabled = true;
                    _navMeshAgent.stoppingDistance = 0f;
                    _navMeshAgent.SetDestination(assignatedWindow.start.position);
                }
                // S'il est collé il essaye de traverser
                else
                {
                    if (assignatedWindow.ShouldAttackWindow())
                    {
                        if (_attackTimer < 0f)
                        {
                            StartCoroutine(Attack());
                        }
                    }
                    else
                    {
                        if (_attackTimer < 0f)
                        {
                            StartCoroutine(TraverseWindow());
                        }
                    }
                }
            }
            // Sinon il se rapproche juste de la fenêtre
            else
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(assignatedWindow.start.position);
            }
        }
    }
    
    private IEnumerator TraverseWindow()
    {
        _isTraversingWindow = true;
        assignatedWindow.StartTraversal(this);
        
        var startPos = assignatedWindow.start.position;
        var middlePos = assignatedWindow.start.position;
        var endPos = assignatedWindow.end.position;
        
        if (_navMeshAgent.enabled && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
        }
        
        _navMeshAgent.Warp(startPos);
        transform.rotation =  Quaternion.LookRotation(assignatedWindow.end.position - transform.position);
        _animator.SetTrigger(_traverseWindowHash);
        
        yield return new WaitForSeconds(windowTraverseDuration / 3);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / (windowTraverseDuration / 3);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        
        yield return new WaitForSeconds(windowTraverseDuration / 3);

        _navMeshAgent.enabled = true;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.stoppingDistance = 2f;

        assignatedWindow.FinishTraversal(this);
        _isTraversingWindow = false;
        _wentTroughWindow = true;
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
        assignatedWindow.AssignedEnemyKilled(this);
        gameManager.EnemyKilled();
        
        if (Random.value < 0.1f)
        {
            Instantiate(healingItemPrefab, transform.position, Quaternion.identity).gameObject.SetActive(true);
        }
        
        Destroy(gameObject);
    }

    private IEnumerator Attack()
    {
        _attackTimer = attackDelay;
        _animator.SetTrigger(_attackHash);

        yield return new WaitForSeconds(0.2f);
        
        Physics.Linecast(transform.position, transform.position + transform.forward * attackDistance, out RaycastHit hit);
        Debug.DrawLine(transform.position, transform.position + transform.forward * attackDistance, Color.blue, 5);

        if (_wentTroughWindow)
        {
            if (hit.collider && hit.collider.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            {
                player.TryGetComponent<IDamageable>(out IDamageable damageable);
                damageable.TakeDamage(damages);
            }
        }
        else
        {
            if (hit.collider && hit.collider.gameObject.TryGetComponent<Window>(out Window window))
            {
                window.RemovePlank();
            }
        }
    }
}
