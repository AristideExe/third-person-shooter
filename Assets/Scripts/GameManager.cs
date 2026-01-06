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
    [SerializeField] private int numberOfEnemiesPerWave = 6;

    private int _numberOfEnemiesAlive;
    private int _waveNumber;
    
    public int WaveNumber => _waveNumber;

    void Awake()
    {
        StartWave();
    }

    private void StartWave()
    {
        _waveNumber++;
        for (int i = 0; i < numberOfEnemiesPerWave; i++)
        {
            windows[Random.Range(0, windows.Count)].SpawnEnemy(enemyPrefab);
        }
        _numberOfEnemiesAlive = numberOfEnemiesPerWave;
    }
    
    public void EnemyKilled()
    {
        _numberOfEnemiesAlive--;
        if (_numberOfEnemiesAlive <= 0)
        {
            numberOfEnemiesPerWave = (int) Math.Round(numberOfEnemiesPerWave * 1.3f);
            StartWave();
        }
    }
}
