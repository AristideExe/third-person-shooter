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

    private Queue<EnnemyController> _queue =  new Queue<EnnemyController>();
    private bool _isTraversing = false;

    public void SpawnEnemy(GameObject enemyPrefab)
    {
        var instance = Instantiate(enemyPrefab, spawner.position,  Quaternion.identity);
        instance.TryGetComponent<EnnemyController>(out var enemy);
        enemy.assignatedWindow = this;
        instance.SetActive(true);
        _queue.Enqueue(enemy);
    }
    
    private void Update()
    {
        if (_queue.Count > 0 && !_isTraversing)
        {
            var enemy = _queue.Dequeue();
            enemy.TryGetComponent<NavMeshAgent>(out var enemyAgent);
            enemyAgent.stoppingDistance = 0f;
            _isTraversing = true;
        }
    }

    public void FinishedTraversal()
    {
        _isTraversing = false;
    }
}
