using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;
    
    private EnnemyController _enemy;
    private Camera _camera;

    private void Awake()
    {
        _enemy = GetComponentInParent<EnnemyController>();
        _camera = Camera.main;
    }

    private void Update()
    {
        fillTransform.anchorMin = new Vector2(1f - _enemy.HealthPercent, 0f);
        transform.LookAt(_camera.transform.position);
    }
}
