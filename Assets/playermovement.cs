using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{
    public CharacterController cc;
    public float speed;
    public float defaultspeed;
    public float crouchspeedmultiplier;
    public float sprintspeedmultiplier;
    private bool cansprint = true;
    public float jumphight;
    public Vector3 Velocity; 
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
        if(grounded && Velocity.y < 0 )
        {
            //make sure velcoity is 0 if its grounded
            Velocity.y = 0; 
        }
        //convert the inputs to a vector 2 
        Vector2 inputs = moveinput.action.ReadValue<Vector2>();
        //convert the vector 2 to a vector 3 to use
        Vector3 movingin = new Vector3(inputs.x, 0.0f, inputs.y);
        //sets it to the max length of the vector so its not to high.
        movingin = Vector3.ClampMagnitude(movingin, max);
        //if they hit jump and are on the ground
        if(jump.action.triggered && grounded)
        {
            //set velocity = tp the square root of jump hight and gravity (hekpfulguy is there to make sure it isnt negative
            Velocity.y = Mathf.Sqrt(jumphight * helpfulguy * gravity);

        }
        if (crouch.action.triggered)
        {
            crouching();
        }
        //add the force of gravity
        Velocity.y += gravity * Time.deltaTime;
        //add everthing up 
        Vector3 alltogethernow = (movingin * speed) + (Velocity.y * Vector3.up);
        //move that guy
        cc.Move(alltogethernow);
        //wow thats alot of stuff for jumping and walking...
        
    }
    private void crouching()
    {
        cc.height = 0.5f; 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
