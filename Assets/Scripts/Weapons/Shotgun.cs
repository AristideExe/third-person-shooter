using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class Shotgun : Weapon
    {
        [SerializeField] private int numberOfShots;
        [SerializeField] private float bulletDispersion;
        [SerializeField] private float distanceDamageReduction;
        
        private Camera _mainCamera;

        public void Awake()
        {
            _mainCamera = Camera.main;
        }

        public override List<(IDamageable, float)> Shoot()
        {
            List<(IDamageable, float)> result = new List<(IDamageable, float)>();
            
            for (int i = 0; i < numberOfShots; i++)
            {
                // Bullet dispersion
                Vector3 linecastEnd = (_mainCamera.transform.forward * shootDistance) + new Vector3(
                                          Random.Range(-bulletDispersion, bulletDispersion),
                                          Random.Range(-bulletDispersion, bulletDispersion), 
                                          Random.Range(-bulletDispersion, bulletDispersion));
                Physics.Linecast(_mainCamera.transform.parent.position, linecastEnd, out RaycastHit hit);
                Debug.DrawLine(_mainCamera.transform.parent.position, linecastEnd, Color.red, 2);
                
                if (hit.collider && hit.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    // The damages decrease with the distance
                    float distance =  Vector3.Distance(_mainCamera.transform.parent.position, hit.point);
                    float damages = Math.Clamp(1 - (distance / distanceDamageReduction), 0.1f, 1f) * damage;
                    result.Add((damageable, damages));
                }
            }

            return result;
        }
    }
}
