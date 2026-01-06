using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;
    [SerializeField] private PlayerController player;
    

    private void Update()
    { 
        fillTransform.anchorMin = new Vector2(1f - player.HealthPercent, 0f);
    }
}
