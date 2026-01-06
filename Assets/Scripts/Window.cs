using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Window : MonoBehaviour, IInteractable
{
    [SerializeField] public Transform start;
    [SerializeField] public Transform end;
    [SerializeField] private Transform spawner;
    [SerializeField] private List<GameObject> planks;
    [SerializeField] private float placePlankDelay = 2f;

    private List<EnnemyController> _enemiesAssigned =  new List<EnnemyController>();
    private bool _isTraversing = false;
    private int _plankCount;
    private float _placePlankTimer;

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        var instance = Instantiate(enemyPrefab, spawner.position,  Quaternion.identity);
        instance.TryGetComponent<EnnemyController>(out var enemy);
        enemy.assignatedWindow = this;
        instance.SetActive(true);
        _enemiesAssigned.Add(enemy);
    }
    
    private void Update()
    {
        _placePlankTimer -= Time.deltaTime;
        if (_enemiesAssigned.Count > 0 && !_isTraversing)
        {
            if (_enemiesAssigned[0])
            {
                _enemiesAssigned[0].TryGetComponent<NavMeshAgent>(out var enemyAgent);
                enemyAgent.stoppingDistance = 0f;
                _enemiesAssigned.Remove(_enemiesAssigned[0]);
                _isTraversing = true;
            }
            else
            {
                _enemiesAssigned.Remove(_enemiesAssigned[0]);
            }
        }
    }

    public void FinishedTraversal()
    {
        _isTraversing = false;
    }

    public void AssignedEnemyKilled(EnnemyController ennemy)
    {
        _enemiesAssigned.Remove(ennemy);
        if (_enemiesAssigned.Count == 0)
        {
            _isTraversing = false;
        }
    }
    
    public void Interact()
    {
        TryPlacingPlank();
    }

    public void TryPlacingPlank()
    {
        if (_plankCount < planks.Count && _placePlankTimer <= 0f &&  !_isTraversing)
        {
            _placePlankTimer = placePlankDelay;
            planks[_plankCount].SetActive(true);
            _plankCount++;
        }
    }
}
