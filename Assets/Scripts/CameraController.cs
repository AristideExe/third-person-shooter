using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float minVerticalOrientation = 0f;
    [SerializeField] private float maxVerticalOrientation = 90f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private Vector2 zoomLimits = new Vector2(1, 2);
    
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform cameraTransform;
    
    // Update is called once per frame
    void Update()
    {
        var calculatedRotation = (Input.GetAxis("Mouse Y") * player.verticalSensibility * Time.deltaTime * -1) + transform.eulerAngles.x;
        //var verticalRotation = Mathf.Clamp(calculatedRotation, minVerticalOrientation, maxVerticalOrientation);
        
        transform.eulerAngles = new Vector3(calculatedRotation, player.transform.eulerAngles.y, 0f); 
        
        if (Input.GetMouseButton(1))
        {
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y, Mathf.Clamp(cameraTransform.localPosition.z + zoomSpeed * Time.deltaTime, -zoomLimits.y, -zoomLimits.x));
        }
        else
        {
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y, Mathf.Clamp(cameraTransform.localPosition.z - zoomSpeed * Time.deltaTime, -zoomLimits.y, -zoomLimits.x));
        }
    }

    public void VerticalRecoil(float verticalRecoil)
    {
        var recoil = Random.Range(0.1f, 0.5f) * verticalRecoil;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - recoil, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
