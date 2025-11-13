using System;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    public float CamMax = 60f;
    public float CamMin = -60f;
    public float sensativity;
    public float rotationY = 0f;
    public float rotationX = 0f;
    public Camera pov;
    public CharacterController cc;
    public float speed;
    public float defaultheight;
    public float defaultspeed;
    public float maxspeed;
    public float minspeed;
    public float small;
    public float basespeed;
    public float decelration;
    public float crouchspeed;
    public float sprintspeed;
    public float acceleration;
    private bool cansprint = true;
    public float jumphight;
    public Vector3 movingin; 
    public float gravity = -9.81f;
    private float helpfulguy = -1f;
    private float max = 1f;
    private bool grounded;
    public InputActionReference moveinput;
    public InputActionReference jump;
    public InputActionReference crouch;
    public InputActionReference sprint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //lock the mouce cursor in place
        Cursor.lockState = CursorLockMode.Locked;
        //hide the cursor
        Cursor.visible = false;
        //make sure the rigidbody is avilible to manipulate.
        pov = gameObject.GetComponent<Camera>();

        grounded = true;
        //get the height at the start and save it for later.
        defaultheight = cc.height;
    }
    private void OnEnable()
    {
        //make sure its enabled 
        moveinput.action.Enable();
        jump.action.Enable();
        sprint.action.Enable();
        crouch.action.Enable();
        
    }
    private void FixedUpdate()
    {
        //if they hit w
        if (Input.GetKey(KeyCode.W))
        {

            //apply acceleration
            speed = Mathf.MoveTowards(speed, maxspeed, acceleration * Time.deltaTime);
            //move forward based on speed
            cc.Move(Vector3.forward * speed);

        }
        if (Input.GetKey(KeyCode.S))
        {
            //same stuff
            speed = Mathf.MoveTowards(speed, maxspeed, acceleration * Time.deltaTime);
            //but it moves backwards.
            cc.Move(-Vector3.forward * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //same stuff
            speed = Mathf.MoveTowards(speed, maxspeed, acceleration * Time.deltaTime);
            //but it moves left.
            cc.Move(Vector3.left * speed);

        }
        if (Input.GetKey(KeyCode.D))
        {
            //same stuff
            speed = Mathf.MoveTowards(speed, maxspeed, acceleration * Time.deltaTime);
            //but it moves right.
            cc.Move(Vector3.right * speed);

        }
        if (Input.GetKey(KeyCode.None))
        {
            speed -= acceleration * Time.deltaTime;
            speed = Mathf.Clamp(speed, minspeed, maxspeed);

        }

        //if you hit left control
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //make the player smaller
            cc.height = small; //for some reason they start sinking into the ground...
            //disable sprinting
            cansprint = false;
            //slow the player down
            speed = crouchspeed;
            //make a debug log for testing
            Debug.Log("fish");
        }
        //otherwise if nothing is pressed
        else
        {
            //apply decleration
            speed = Mathf.MoveTowards(speed, basespeed, decelration * Time.deltaTime);

            //set height to normal
            cc.height = defaultheight;
            //allow sprinting
            cansprint = true;

            //yes this code effects the stuff below   
        }
        //if you hit shift
        if (cansprint == true && Input.GetKey(KeyCode.LeftShift))
        {
            //increase speed
            speed = sprintspeed;
            //debug log for testing
            Debug.Log("runnin");
        }
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
