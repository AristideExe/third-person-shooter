using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VendingMachines;

public class UIVendingMachine : MonoBehaviour
{
    [SerializeField] private VendingMachine vendingMachine;
    [SerializeField] private TMPro.TextMeshProUGUI priceText;
    [SerializeField] private TMPro.TextMeshProUGUI currentStatText;
    [SerializeField] private RectTransform fillTransform;
    
    void Update()
    {
        priceText.text = $"{vendingMachine.Price} $";
        currentStatText.text = $"{vendingMachine.CurrentStat}";
        fillTransform.anchorMin = new Vector2(vendingMachine.SellDelayPercentage, 0f);
    }
}
