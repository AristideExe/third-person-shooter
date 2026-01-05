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
            Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, shootDistance))
            {
                Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, 5);
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    return new List<(IDamageable, float)>
                    {
                        (damageable, damage)
                    };
                }
            }
            return new List<(IDamageable, float)>();
        }
    }
}
