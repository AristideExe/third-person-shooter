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
        [SerializeField] public float reloadTime;
        [SerializeField] protected float shootDistance = 100f;
        [SerializeField] public float recoil;

        public abstract List<(IDamageable, float)> Shoot();
    }
}
