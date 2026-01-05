using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWaveCounter : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI waweText;
    [SerializeField] private GameManager gameManager;

    private void Update()
    {
        waweText.text = $"Wave {gameManager.WaveNumber}";
    }
}
