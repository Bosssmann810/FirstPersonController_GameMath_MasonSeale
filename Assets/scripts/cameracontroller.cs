using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    public float CamMax = 60f;
    public float CamMin = -60f;
    public float sensativity;
    public float rotationY = 0f;
    public float rotationX = 0f;
    public Camera pov;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //lock the mouce cursor in place
        Cursor.lockState = CursorLockMode.Locked;
        //hide the cursor
        Cursor.visible = false;
        //make sure the rigidbody is avilible to manipulate.
        pov = gameObject.GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        //find the rotation of the mouses x variable (times sensativity)
        rotationY += Input.GetAxis("Mouse X") * sensativity;
        //same thing but for Y
        rotationX += Input.GetAxis("Mouse Y") * sensativity;
        //clamp x so you cant look to far
        rotationX = Mathf.Clamp(rotationX, CamMin, CamMax);
        //convert the rotations to a usable angle vector 3 and move the camera
        gameObject.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0f);

        //to make sure i can still use the mouse
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }
}
