using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] public Sprite crosshair;
        [SerializeField] public bool isAutomatic;
        [SerializeField] protected float damage;
        [SerializeField] public float shootDelay;
        // Le nombre de balles max dans le chargeur
        [SerializeField] public int maxAmmo;
        // Le nombre de balles que le joueur possède pour cette arme
        [SerializeField] public int totalAmmo;
        [SerializeField] public float reloadTime;
        [SerializeField] protected float shootDistance = 100f;
        [SerializeField] public float horizontalRecoil;
        [SerializeField] public float verticalRecoil;

        // Le nombre de balles dans le chargeur de l'arme
        protected int _weaponAmmo;

        public int WeaponAmmo => _weaponAmmo;
        public int MaxAmmo => maxAmmo;
        public int TotalAmmo => totalAmmo;
        public float ReloadTime => reloadTime;

        protected void Awake()
        {
            _weaponAmmo = maxAmmo;
        }

        public abstract List<(IDamageable, float)> Shoot();
        public abstract void Reload();
    }
}
