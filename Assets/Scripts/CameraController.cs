using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minVerticalOrientation = 0f;
    [SerializeField] private float maxVerticalOrientation = 90f;
    
    [SerializeField] private float verticalSensibility = 250f;
    
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {
        var calculatedRotation = (Input.GetAxis("Mouse Y") * verticalSensibility * Time.deltaTime * -1) + transform.eulerAngles.x;
        //var verticalRotation = Mathf.Clamp(calculatedRotation, minVerticalOrientation, maxVerticalOrientation);
        
        transform.eulerAngles = new Vector3(calculatedRotation, player.eulerAngles.y, 0f); 
    }
}
