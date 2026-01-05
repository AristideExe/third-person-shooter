using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Window : MonoBehaviour
{
    [SerializeField] public Transform start;
    [SerializeField] public Transform end;
    [SerializeField] private Transform spawner;

    private List<EnnemyController> _enemiesAssigned =  new List<EnnemyController>();
    private bool _isTraversing = false;

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
}
