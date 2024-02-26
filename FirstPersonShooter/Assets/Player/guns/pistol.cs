using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using TMPro;

public class pistol : MonoBehaviour
{

    public GunData gunData;
    public Camera cam;
    private Ray ray;
    private int ammo_in_clip;
    private float shoot_delay_timer = 0.0f;

    //Shooting
    private bool primary_fire_is_shooting = false;
    private bool primary_fire_hold = false;

    //Debug
    public TMP_Text debug_text;

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
        if(shoot_delay_timer > 0)
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
            if(context.interaction is HoldInteraction && context.phase == InputActionPhase.Performed)
            {
                primary_fire_hold = true;
            }
        }

        //check if button was released
        if(context.phase == InputActionPhase.Canceled)
        {
            primary_fire_hold = false;
            primary_fire_is_shooting = false;
        }
    }
    
    public void GetSecondaryFireInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) SecondaryFire();
    }

    private void PrimaryFire()
    {
        if(shoot_delay_timer <= 0)
        {
            if (primary_fire_is_shooting || primary_fire_hold)
            {
                shoot_delay_timer = gunData.primary_fire_delay; //delay shooting

                primary_fire_is_shooting = false;

                //determine raycast direction
                Vector3 dir = Quaternion.AngleAxis(Random.Range(-gunData.spread, gunData.spread), Vector3.up) * cam.transform.forward;
                dir = Quaternion.AngleAxis(Random.Range(-gunData.spread, gunData.spread), Vector3.right) * cam.transform.forward;

                //cast out a ray! raycast...
                ray = new Ray(cam.transform.position, dir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, gunData.range))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green, 0.05f);
                }

                ammo_in_clip--;
                if (ammo_in_clip <= 0) ammo_in_clip = gunData.ammo_per_clip;
            }
        }       
    }



    private void SecondaryFire()
    {

    }

}
