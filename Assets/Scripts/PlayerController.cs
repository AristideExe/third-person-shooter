using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    [SerializeField] private float horizontalSensibility = 250f;    
   
    private CharacterController _characterController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalRotation = (Input.GetAxis("Mouse X") * horizontalSensibility * Time.deltaTime) + transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0f, horizontalRotation, 0f); 
    }
}
