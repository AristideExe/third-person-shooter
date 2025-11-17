using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minVerticalOrientation = 0f;
    [SerializeField] private float maxVerticalOrientation = 90f;
    
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform cameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {
        var calculatedRotation = (Input.GetAxis("Mouse Y") * player.verticalSensibility * Time.deltaTime * -1) + transform.eulerAngles.x;
        //var verticalRotation = Mathf.Clamp(calculatedRotation, minVerticalOrientation, maxVerticalOrientation);
        
        transform.eulerAngles = new Vector3(calculatedRotation, player.transform.eulerAngles.y, 0f); 
    }

    public void AddRecoil(float recoil)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - recoil, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
