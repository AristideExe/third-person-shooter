using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStats : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;
    [SerializeField] private TMPro.TextMeshProUGUI ammoText;
    [SerializeField] private PlayerController playerController;

    void Update()
    {
        moneyText.text = $"{playerController.Money} $";
    }
}
