using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] public Transform start;
    [SerializeField] public Transform end;

    private Queue<EnnemyController> _queue =  new Queue<EnnemyController>();
    private bool _isOccupied = false;
    
    public void RequestPassage(EnnemyController enemy)
    {
        if (!_queue.Contains(enemy))
            _queue.Enqueue(enemy);

        TryNext();
    }

    private void TryNext()
    {
        if (_isOccupied || _queue.Count == 0)
            return;

        var enemy = _queue.Dequeue();
        _isOccupied = true;
        enemy.StartWindowTraversal();
    }

    public void Release()
    {
        _isOccupied = false;
        TryNext();
    }
}
