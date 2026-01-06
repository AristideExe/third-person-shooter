using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Weapons;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float maxHealth = 100f;
    
    [SerializeField] public float horizontalSensibility = 250f;
    [SerializeField] public float verticalSensibility = 250f;
    [SerializeField] private CameraController cameraController;
    
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private Weapon weaponPrefab;
    
    private CharacterController _characterController;
    private Camera _mainCamera;
    private Vector3 _playerVelocity;
    private Weapon _weapon;
    private float _shootTimer;
    private float _health;
    private int _money = 5000;
    private float _damageMultiplier = 1;

    private bool _isReloading;
    private float _lastTimeJumpPressed = 1f;
    private const float GravityValue = -20f;
    private const float InteractDistance = 5f;
    
    private Animator _animator;
    private int _reloadingHash;

    public Sprite Crosshair => _weapon ? _weapon.crosshair : null;
    public int Money => _money;
    public int WeaponAmmo => _weapon.WeaponAmmo;
    public int TotalAmmo => _weapon.TotalAmmo;
    public float HealthPercent => _health / maxHealth;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        var weaponObject = Instantiate(weaponPrefab, weaponSocket.position, transform.rotation).gameObject;
        weaponObject.SetActive(true);
        weaponObject.transform.parent = weaponSocket;
        _weapon = weaponObject.GetComponent<Weapon>();
        _health = maxHealth;
        
        _animator = GetComponentInChildren<Animator>();
        _reloadingHash = Animator.StringToHash("Reloading");
    }

    private void Update()
    {
        var horizontalRotation = (Input.GetAxis("Mouse X") * horizontalSensibility * Time.deltaTime) + transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0f, horizontalRotation, 0f);
        
        if (Input.GetKeyDown("space"))
        {
            _lastTimeJumpPressed = 0f;
        }
        else _lastTimeJumpPressed += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading && _weapon.WeaponAmmo != _weapon.MaxAmmo && TotalAmmo > 0)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKey(KeyCode.E))
        {
            Interact();
        }
        
        // Shooting
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f && WeaponAmmo > 0 && !_isReloading)
        {
            List<(IDamageable, float)> touched = new List<(IDamageable, float)>();
            // Automatic weapon
            if (Input.GetMouseButton(0) && _weapon.isAutomatic)
            {
                Shoot(out touched);
            }
        
            // Semi-auto weapon
            if (Input.GetMouseButtonDown(0) && !_weapon.isAutomatic)
            {
                Shoot(out touched);
            }

            for (int i = 0; i < touched.Count; i++)
            {
                touched[i].Item1.TakeDamage(touched[i].Item2 * _damageMultiplier);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_characterController.isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
        
        var movement = Vector3.ClampMagnitude(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")), 1f);
        
        // If player tries to jump 0.2s before hitting the ground, he will jump anyway
        if (_lastTimeJumpPressed < 0.2f && _characterController.isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpForce * -2.0f * GravityValue);
        }
        
        // Apply gravity
        _playerVelocity.y += GravityValue * Time.deltaTime;
        
        _characterController.Move(((transform.rotation * movement * speed) + (_playerVelocity.y * Vector3.up)) * Time.deltaTime);
    }

    private void Shoot(out List<(IDamageable, float)> touched)
    {
        touched = _weapon.Shoot();
        _shootTimer = _weapon.shootDelay;
        cameraController.VerticalRecoil(_weapon.verticalRecoil);
        HorizontalRecoil(_weapon.horizontalRecoil);
    }
    
    private void HorizontalRecoil(float horizontalRecoil)
    {
        var recoil = Random.Range(-0.5f, 0.5f) * horizontalRecoil;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - recoil, transform.eulerAngles.z);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void AddMoney(int money = 10)
    {
        _money += money;
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        _animator.SetBool(_reloadingHash, true);
        yield return new WaitForSeconds(_weapon.ReloadTime);
        
        _isReloading = false;
        _animator.SetBool(_reloadingHash, false);
        _weapon.Reload();
    }

    private void Interact()
    {
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Debug.DrawRay(ray.origin, ray.direction * InteractDistance, Color.blue, 5);
        if (Physics.Raycast(ray, out RaycastHit hit, InteractDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
               interactable.Interact();
            }
        }
    }

    public void Heal()
    {
        this._health = maxHealth;
    }

    public void RefillAmmo()
    {
        _weapon.RefillAmmo();
    }

    public void RemoveMoney(int money)
    {
        _money -= money;
    }

    public void IncreaseDamageMultiplier()
    {
        _damageMultiplier += 0.20f;
    }

    public void IncreaseMaxHealth()
    {
        maxHealth += 50;
        _health += 50;
    }


    public void IncreaseSpeed()
    {
        speed += 1f;
    }
}
