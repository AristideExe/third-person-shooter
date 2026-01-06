using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    
    private Animator _animator;
    private int _collectHash;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _collectHash = Animator.StringToHash("Collect");
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.Heal();
            _animator.SetTrigger(_collectHash);
            Destroy(gameObject, 0.5f);
            enabled = false;
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(lifeTime);
        _animator.SetTrigger(_collectHash);
        Destroy(gameObject, 0.5f);
        enabled = false;
    }
}
