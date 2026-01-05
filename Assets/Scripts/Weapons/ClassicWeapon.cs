using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class ClassicWeapon : Weapon
    {
        private Camera _mainCamera;

        private new void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
        }

        public override List<(IDamageable, float)> Shoot()
        {
            _weaponAmmo--;
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

        public override void Reload()
        {
            if (maxAmmo < totalAmmo)
            {
                totalAmmo -= maxAmmo - _weaponAmmo;
                _weaponAmmo = maxAmmo;
            }
            else
            {
                _weaponAmmo = totalAmmo;
                totalAmmo = 0;
            }
        }
    }
}
