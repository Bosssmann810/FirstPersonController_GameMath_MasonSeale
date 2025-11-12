using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    public CharacterController cc;
    public float speed;
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
        grounded = cc.isGrounded;
        if(grounded && movingin.y < 0 )
        {
            //make sure velcoity is 0 if its grounded
            movingin.y = 0; 
        }
        //convert the inputs to a vector 2 
        Vector2 inputs = moveinput.action.ReadValue<Vector2>();
        //convert the vector 2 to a vector 3 to use
        movingin = new Vector3(inputs.x, 0.0f, inputs.y);
        //sets it to the max length of the vector so its not to high.
        movingin = Vector3.ClampMagnitude(movingin, max);
        Debug.Log(grounded);
        //if they hit jump and are on the ground
        if(jump.action.triggered && grounded)
        {
            //set velocity = tp the square root of jump hight and gravity (hekpfulguy is there to make sure it isnt negative
            movingin.y = Mathf.Sqrt(jumphight * helpfulguy * gravity);

        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            cc.height = 0.5f;
            cansprint = false;
            speed = crouchspeed; 
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Debug.Log("huh");
            cc.height = 10f;
            cansprint = true;
            speed = defaultspeed;
            
        }
        //add the force of gravity
        if (grounded != true)
        {
            movingin.y += gravity * Time.deltaTime;
        }
        //add everthing up 
        Vector3 alltogethernow = (movingin * speed);
        //move that guy
        cc.Move(alltogethernow);
        //wow thats alot of stuff for jumping and walking...
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
