using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    [SerializeField] public float horizontalSensibility = 250f;
    [SerializeField] public float verticalSensibility = 250f;
    [SerializeField] private CameraController cameraController;
    
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private Weapon weaponPrefab;
    
    private CharacterController _characterController;
    private Weapon _weapon;
    private float _shootTimer;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        var weaponObject = Instantiate(weaponPrefab, weaponSocket.position, transform.rotation).gameObject;
        weaponObject.SetActive(true);
        weaponObject.transform.parent = weaponSocket;
        _weapon = weaponObject.GetComponent<Weapon>();
    }

    void Update()
    {
        var horizontalRotation = (Input.GetAxis("Mouse X") * horizontalSensibility * Time.deltaTime) + transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0f, horizontalRotation, 0f);
        
        var movement = new Vector3( Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        _characterController.Move(transform.rotation * movement * (speed * Time.deltaTime));
        
        // Shooting
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            List<(IDamageable, float)> touched = new List<(IDamageable, float)>();
            // Automatic weapon
            if (Input.GetMouseButton(0) && _weapon.isAutomatic)
            {
                touched = _weapon.Shoot();
                _shootTimer = _weapon.shootDelay;
                cameraController.AddRecoil(_weapon.recoil);
            }
        
            // Semi-auto weapon
            if (Input.GetMouseButtonDown(0) && !_weapon.isAutomatic)
            {
                touched = _weapon.Shoot();
                _shootTimer = _weapon.shootDelay;
                cameraController.AddRecoil(_weapon.recoil);
            }

            for (int i = 0; i < touched.Count; i++)
            {
                touched[i].Item1.TakeDamage(touched[i].Item2);
            }
        }
    }
}
