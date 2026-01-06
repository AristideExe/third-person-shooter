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
    [SerializeField] private float spawnDelay = 10f;

    private int _numberOfEnemiesAlive;
    private int _waveNumber;
    
    public int WaveNumber => _waveNumber;

    void Awake()
    {
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        _waveNumber++;
        yield return new WaitForSeconds(spawnDelay);
        
        for (int i = 0; i < numberOfEnemiesPerWave; i++)
        {
            windows[Random.Range(0, windows.Count)].SpawnEnemy(enemyPrefab, _waveNumber);
        }

        foreach (var window in windows)
        {
            window.StartWave();
        }
        _numberOfEnemiesAlive = numberOfEnemiesPerWave;
    }
    
    public void EnemyKilled()
    {
        _numberOfEnemiesAlive--;
        if (_numberOfEnemiesAlive <= 0)
        {
            numberOfEnemiesPerWave = (int) Math.Round(numberOfEnemiesPerWave * 1.3f);
            StartCoroutine(StartWave());
        }
    }
}
