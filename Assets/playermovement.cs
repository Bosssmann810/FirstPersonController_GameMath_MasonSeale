using System;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    private Vector3 inputstorage;
    public float CamMax = 60f;
    public float CamMin = -60f;
    public float sensativity;
    public float rotationY = 0f;
    public float rotationX = 0f;
    public GameObject pov;
    private float notneeded = 0f;
    public CharacterController cc;
    public float speed;
    public float speedituplad;
    public float defaultheight;
    public float defaultspeed;
    public float maxspeed;
    public float sprintaccel;
    public float sprintmax;
    public float sprintdecel;
    private float regaccel;
    private float regmax;
    private float regdecel;
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
    private Vector3 cleared = new Vector3(0, 0, 0);
    private Vector2 nothingpressed = new Vector2(0, 0);
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
        //get all the veriables for later reseting
        defaultspeed = speed;
        regaccel = acceleration;
        regdecel = decelration;
        regmax = maxspeed;
        //lock the mouce cursor in place
        Cursor.lockState = CursorLockMode.Locked;
        //hide the cursor
        Cursor.visible = false;
        //make sure the rigidbody is avilible to manipulate.
        

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

    // Update is called once per frame
    void Update()
    {
        //checks weather or not its grounded or not
        grounded = cc.isGrounded;

        //if any of WASD or arrow keys are hit convert the inputs to a vector 2
        Vector2 inputs = moveinput.action.ReadValue<Vector2>();
        //make it a vector 3 to be useable
        Vector3 moveing = transform.forward * inputs.y+ transform.right * inputs.x;

        if(grounded && moveing.y <= 0)
        {
            moveing.y = 0;
        }
        
        //for testing
        Debug.Log(inputs);
        //if input 
        if (inputs != nothingpressed)
        {
            //save whatever the input was in input storage
            if (inputs.x == 1f)
            {
                inputstorage.x = 1f;
            }
            if (inputs.y == 1f)
            {
                inputstorage.z = 1f;
            }
            if (inputs.x == -1f)
            {
                inputstorage.x = -1f;
            }
            if (inputs.y == -1f)
            {
                inputstorage.z = -1f;
            }
            //apply acceleration
            speed = Mathf.MoveTowards(speed, maxspeed, acceleration * Time.deltaTime);
        }
        //if no input
        if (inputs == nothingpressed)
        {

            Debug.Log("a");
            //apply decleration
            speed = Mathf.MoveTowards(speed, basespeed, decelration * Time.deltaTime);
            //process the original variables into a usable vector3
            Vector3 Processing = (transform.forward * inputstorage.z) + transform.right * inputstorage.x;
            //continue to move using input storage
            cc.Move(Processing * speed);
            //if speed returns to normal stop moving and clear input storage.
            if (speed == basespeed)
            {
                Debug.Log("stopped");
                inputstorage = cleared;
            }
        }
        //if you hit jump and are on the ground
        if(jump.action.triggered && grounded == true)
        {
            //jump (jumpheight times 2 negative numbers to prevent errors)
            moveing.y = jumphight * helpfulguy * gravity;
        }


        //if you hit left control
        if (Input.GetKey(KeyCode.LeftControl))
        {
            
            //slow the player down
            speed = crouchspeed;
            //make the player smaller
            cc.height = small;
            //disable sprinting
            cansprint = false;
            //make a debug log for testing
            Debug.Log("fish");
        }
        //if control is released
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            //set speed to normal
            speed = defaultspeed;
            //set height to normal 
            cc.height = defaultheight;
            //allow sprinting
            cansprint = true;
            //reset max speed
            maxspeed = regmax;
            
        }
        //if speed is ever higher then maxspeed
        if(speed > maxspeed)
        {
            //apply deceleration (mostly happens when going from sprinting to walking)
            speed = Mathf.MoveTowards(speed, maxspeed, decelration * Time.deltaTime);
        }


        //if you hit shift
        if (cansprint == true && Input.GetKey(KeyCode.LeftShift))
        {
            //increase maxspeed and acceleration
            acceleration = sprintaccel;
            maxspeed = sprintmax;
            //half decelleration
            decelration = sprintdecel; ;
            //debug log for testing
            Debug.Log("runnin");
        }
        //when you let go of shift
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            acceleration = regaccel;
            maxspeed = regmax;
            decelration = regdecel;

            //set everything to normal
        }
        //find the rotation of the mouses x variable (times sensativity)
        rotationY += Input.GetAxis("Mouse X") * sensativity;
        //same thing but for Y
        rotationX += Input.GetAxis("Mouse Y") * sensativity;
        //clamp x so you cant look to far
        rotationX = Mathf.Clamp(rotationX, CamMin, CamMax);
        //convert the rotations to a usable angle vector 3 and move the camera
        Vector3 holder = new Vector3(0f, rotationY, 0f);
        //move the camera seperatly (x just moves the camera while y moves the facing direction)
        pov.transform.localEulerAngles = new Vector3(-rotationX, 0f, 0f);
        cc.transform.localEulerAngles = holder;

        //to make sure i can still use the mouse
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //show the cursor again
            Cursor.visible = true;
            //let the cursor move
            Cursor.lockState = CursorLockMode.None;
        }
        //add gravity to the moving.y to make it fall
        moveing.y += gravity * Time.deltaTime * speedituplad;
        //add the x y and zz parts of moveing together.
        Vector3 splicing = new Vector3(moveing.x * speed, moveing.y, moveing.z * speed);
        //move that fella
        cc.Move(splicing);
       // Debug.Log(speed);
        // Debug.Log(transform.forward);
        if (grounded)
        {
            Debug.Log("g");
        }
        else
        {
            Debug.Log("not");
        }
        
            

    }
}
