using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    public CharacterController cc;
    public float speed;
    public float defaultheight;
    public float defaultspeed;
    public float crouchspeed;
    public float sprintspeed;
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
        grounded = true;
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
        //is the player grounded?
        
        if(movingin.y == 0 )
        {
            //make sure velcoity is 0 if its grounded
            grounded = true;
            movingin.y = 0;
        }
        //convert the inputs to a vector 2 
        Vector2 inputs = moveinput.action.ReadValue<Vector2>();
        //convert the vector 2 to a vector 3 to use
        movingin = new Vector3(inputs.x, 0.0f, inputs.y);
        //sets it to the max length of the vector so its not to high.
        movingin = Vector3.ClampMagnitude(movingin, max);
        //if they hit jump and are on the ground
        if(Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            Debug.Log("beans");
            //set velocity = tp the square root of jump hight and gravity (hellpfulguy is there to make sure it isnt negative
            movingin.y = Mathf.Sqrt(jumphight * helpfulguy * gravity);
            grounded = false;

        }
        //if you hit left control
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //make the player smaller
            cc.height = 0.5f;
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
            //set height to normal
            cc.height = defaultheight;
            //allow sprinting
            cansprint = true;
            //and make speed normal
            speed = defaultspeed;
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

        //add the force of gravity
        if (grounded == false)
        {
            movingin.y += gravity * Time.deltaTime;
        }
        //add everthing up 
        Vector3 alltogethernow = (movingin * speed) + (movingin.y * Vector3.up);
        //move that guy
        cc.Move(alltogethernow);
        //wow thats alot of stuff for jumping and walking...
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
