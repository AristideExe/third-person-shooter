using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Window> windows;
    [SerializeField] private GameObject enemyPrefab;

    private int _numberOfEnemiesAlive;
    private int _numberOfEnemiesPerWave = 6;
    private int _waveNumber;
    
    public int WaveNumber => _waveNumber;

    void Awake()
    {
        StartWave();
    }

    private void StartWave()
    {
        _waveNumber++;
        for (int i = 0; i < _numberOfEnemiesPerWave; i++)
        {
            windows[Random.Range(0, windows.Count)].SpawnEnemy(enemyPrefab);
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
