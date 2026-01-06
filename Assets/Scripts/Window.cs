using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class Window : MonoBehaviour, IInteractable
{
    [SerializeField] public Transform start;
    [SerializeField] public Transform end;
    [SerializeField] private Transform spawner;
    [SerializeField] private List<GameObject> planks;
    [SerializeField] private float placePlankDelay = 2f;
    [SerializeField] private PlayerController player;

    private List<EnnemyController> _enemiesAssigned =  new List<EnnemyController>();
    private bool _isTraversing;
    private EnnemyController _traversingEnemy;
    
    private int _plankCount;
    private float _placePlankTimer;

    public void SpawnEnemy(GameObject enemyPrefab, int waveNumber)
    {
        var instance = Instantiate(enemyPrefab, spawner.position,  Quaternion.identity);
        instance.TryGetComponent<EnnemyController>(out var enemy);
        
        enemy.assignatedWindow = this;
        // L'ennemi voit ses stats augmenter avec les vagues
        enemy.MaxHealth += (float) Math.Pow(enemy.MaxHealth, waveNumber * 0.2f);
        enemy.Damages += (float)Math.Pow(enemy.Damages, waveNumber * 0.2f);
        instance.SetActive(true);
        
        
        _enemiesAssigned.Add(enemy);
    }
    
    private void Update()
    {
        _placePlankTimer -= Time.deltaTime;
    }

    public void StartTraversal(EnnemyController enemy)
    {
        _isTraversing = true;
    }

    public void FinishTraversal(EnnemyController enemy)
    {
        _isTraversing = false;
        _traversingEnemy = null;
        _enemiesAssigned.Remove(enemy);
        NextTraversal();
    }

    public void AssignedEnemyKilled(EnnemyController enemy)
    {
        // Si l'ennemi tué était en train de traverser, on passe au suivant
        if (_traversingEnemy == enemy)
        {
            // Il faut bien le retirer avant l'appel de next traversal sinon il sera toujous dans la
            // liste des ennemis disponibles
            _enemiesAssigned.Remove(enemy);
            _isTraversing = false;
            _traversingEnemy = null;
            NextTraversal();
        }
        else
        {
            _enemiesAssigned.Remove(enemy);
        }
    }

    public void StartWave()
    {
        NextTraversal();
    }

    private void NextTraversal()
    {
        if (_enemiesAssigned.Count > 0)
        {
            _enemiesAssigned[0].MustTraverseWindow = true;
            _traversingEnemy = _enemiesAssigned[0];
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
            player.AddMoney(10);
        }
    }

    public bool ShouldAttackWindow()
    {
        return _plankCount > 0;
    }

    public void RemovePlank()
    {
        planks[_plankCount - 1].gameObject.SetActive(false);
        _plankCount--;
    }
}
