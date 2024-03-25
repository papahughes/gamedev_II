using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using TMPro;

public class Pistol : Gun
{
    protected override void PrimaryFire()
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

                //particles
                muzzle_flash.Play();
                TrailRenderer trail = Instantiate(bullet_trail, shoot_point.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, dir, hit));
            }
        }       
    }



    private void SecondaryFire()
    {

    }

}
