using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using TMPro;

public class Gun : MonoBehaviour
{
    //Debug
    public TMP_Text debug_text;

    //gun variables
    public GunData gunData;
    public Camera cam;
    protected Ray ray;


    //ammo
    protected int ammo_in_clip;


    //Shooting
    protected bool primary_fire_is_shooting = false;
    protected bool primary_fire_hold = false;
    protected float shoot_delay_timer = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        ammo_in_clip = gunData.ammo_per_clip;
    }

    // Update is called once per frame
    void Update()
    {
        debug_text.text = "Ammo in Clip: " + ammo_in_clip.ToString();
        PrimaryFire();


        //subtract from shoot timer
        if (shoot_delay_timer > 0)
        {
            shoot_delay_timer -= Time.deltaTime;
        }
    }

    public void GetPrimaryFireInput(InputAction.CallbackContext context)
    {
        //checkingfor initial button press
        if (context.phase == InputActionPhase.Started)
        {
            primary_fire_is_shooting = true;
        }

        //check if gun is automatic
        if (gunData.automatic)
        {
            //check if hold action was complete
            if (context.interaction is HoldInteraction && context.phase == InputActionPhase.Performed)
            {
                primary_fire_hold = true;
            }
        }

        //check if button was released
        if (context.phase == InputActionPhase.Canceled)
        {
            primary_fire_hold = false;
            primary_fire_is_shooting = false;
        }
    }

    public void GetSecondaryFireInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) SecondaryFire();
    }

    protected virtual void PrimaryFire()
    {

    }

    protected virtual void SecondaryFire()
    {

    }
}
