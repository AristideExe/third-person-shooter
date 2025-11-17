using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class ClassicWeapon : Weapon
    {
        private Camera _mainCamera;

        public void Awake()
        {
            _mainCamera = Camera.main;
        }
        

        public override List<(IDamageable, float)> Shoot()
        {
            Physics.Linecast(_mainCamera.transform.parent.position, _mainCamera.transform.forward * shootDistance, out RaycastHit hit);
            Debug.DrawLine(_mainCamera.transform.parent.position, _mainCamera.transform.forward * shootDistance, Color.red, 5);
            if (hit.collider && hit.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                return new List<(IDamageable, float)>(){ (damageable, damage) };
            }
            return new List<(IDamageable, float)>();
        }
    }
}
