using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    //Camera Variables
    public Camera cam;
    private Vector2 look_input = Vector2.zero;
    private float look_speed = 60;
    private float horizontal_look_angle = 0f;
    [Range(0.01f, 1f)] public float sensitivity;

    //player input
    private Vector2 move_input;
    private bool grounded;

    //Movement Variables
    private CharacterController character_controller;
    private Vector3 player_velocity;
    private Vector3 wish_dir = Vector3.zero;

    //physics variables
    public float max_speed = 6f;
    public float acceleration = 60;
    public float gravity = 15f;
    public float stop_speed = 0.5f;
    public float jump_impulse = 10f;
    public float friction = 4;

    //Debug
    public TMP_Text debug_text;

    // Start is called before the first frame update
    void Start()
    {
        //Hiding the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //get components to the player controller
        character_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //debug
        //debug_text.text += "Wish Dir: " + wish_dir.ToString();
        //debug_text.text += "\nPlayer Velocity: " + player_velocity.ToString();
        //debug_text.text += "\nPlayer Speed: " + new Vector3(player_velocity.x, 0, player_velocity.z).magnitude.ToString();
        //debug_text.text += "\nGrounded: " + grounded.ToString();

        Look();
    }

    private void FixedUpdate()
    {
        //find the wish_dir
        wish_dir = transform.right * move_input.x + transform.forward * move_input.y;
        wish_dir = wish_dir.normalized;
    }

    public void GetLookInput(InputAction.CallbackContext context)
    {
        look_input = context.ReadValue<Vector2>();
    }

    private void Look()
    {
        //Left/Right
        transform.Rotate(Vector3.up, look_input.x * look_speed * Time.deltaTime * sensitivity);

        //Up/Down
        float angle = look_input.y * look_speed *Time.deltaTime * sensitivity;
        horizontal_look_angle -= angle;
        horizontal_look_angle = Mathf.Clamp(horizontal_look_angle, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(horizontal_look_angle, 0, 0);
    }

    public void GetMoveInput(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void Jump()
    {
        if (grounded)
        {
            //to do...
            //apply upward force to the player
        }
    }

    private Vector3 Accelerate(Vector3 wish_dir, Vector3 current_velocity, float accel, float max_speed)
    {
        //Vector projection of the current velocity onto the wish_dir, the speed the player is going
        float proj_speed = Vector3.Dot(current_velocity, wish_dir);
        //The acceleration component to add to the projected speed
        float accel_speed = accel * Time.deltaTime;

        //If necessary, truncate the accelerated velocity so the vector projection does not exceed max_speed
        if (proj_speed + accel_speed > max_speed)
            accel_speed = max_speed - proj_speed;
        
        //Return new speed
        return current_velocity + (wish_dir * accel_speed);
    }

    private Vector3 MoveGround(Vector3 wish_dir, Vector3 current_velocity)
    {
        //Create new velocity vector
        Vector3 new_velocity = new Vector3(current_velocity.x, 0, current_velocity.z); //remove the y component for now?

        //since on ground, apply friction
        float speed = new_velocity.magnitude;
        if(speed <= stop_speed)
        {
            new_velocity = Vector3.zero;
            speed = 0;
        }

        if(speed != 0)
        {
            float drop = speed * friction * Time.deltaTime;
            new_velocity *= Mathf.Max(speed - drop, 0) / speed; //scale velocity based on friction
        }
        new_velocity = new Vector3(new_velocity.x, current_velocity.y, new_velocity.z); //Add y component back in

        return Accelerate(wish_dir, new_velocity, acceleration, max_speed);
    }

    private Vector3 MoveAir(Vector3 wish_dir, Vector3 current_velocity)
    {
        return Accelerate(wish_dir, current_velocity, acceleration, max_speed);
    }
}
