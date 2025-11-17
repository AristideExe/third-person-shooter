using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICrosshair : MonoBehaviour
    {
        [SerializeField] private Image crosshair;
        [SerializeField] private Sprite emptyCrosshair;
        [SerializeField] private PlayerController player;
        
        void Update()
        {
            Sprite playerCrosshair = player.Crosshair;
            crosshair.sprite = playerCrosshair ? playerCrosshair : emptyCrosshair;
        }
    }
}
