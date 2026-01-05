using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Window> windows;
    [SerializeField] private List<Window> accessibleWindows;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemySpawner;

    private int _numberOfEnemiesAlive;
    private int _numberOfEnemiesPerWave = 6;
    private int _waveNumber;

    void Awake()
    {
        StartWave();
    }

    private void StartWave()
    {
        _waveNumber++;
        for (int i = 0; i < _numberOfEnemiesPerWave; i++)
        {
            var instance = Instantiate(enemyPrefab, enemySpawner.position,  Quaternion.identity);
            instance.TryGetComponent<EnnemyController>(out var enemy);
            enemy.assignatedWindow = accessibleWindows[i % accessibleWindows.Count];
            instance.SetActive(true);
        }
        _numberOfEnemiesAlive = _numberOfEnemiesPerWave;
    }
    
    public void EnemyKilled()
    {
        _numberOfEnemiesAlive--;
        if (_numberOfEnemiesAlive <= 0)
        {
            _numberOfEnemiesPerWave = (int) Math.Round(_numberOfEnemiesPerWave * 1.3f);
            StartWave();
        }
    }
}
