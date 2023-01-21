using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MuzzleFlash))]
public class Gun : MonoBehaviour
{
    public enum FireMode
    {
        Auto,
        Burst,
        Single,
    }
    public FireMode fireMode;

    public Transform[] projectileSpawn;
    public Projectile projectile;
    public float msBetweenShots = 100f;
    public float muzzleVelocity = 35f;
    public int burstCont;

    public Transform shell;
    public Transform shellEjection;

    MuzzleFlash muzzleFlash;

    float nextShotTime;

    bool triggerReleaseSinceLastShot;
    int shotRemainingInBurst;

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotRemainingInBurst = burstCont;
    }

    void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            if(fireMode == FireMode.Burst)
            {
                if(shotRemainingInBurst == 0)
                {
                    return;
                }
                shotRemainingInBurst--;
            }
            else if(fireMode == FireMode.Single)
            {
                if (!triggerReleaseSinceLastShot)
                {
                    return;
                }
            }

            for(int i = 0; i < projectileSpawn.Length; i++)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProejctile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation);
                newProejctile.SetSpeed(muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
        }
        
    }

    public void OnTriggerHold()
    {
        Shoot();

        triggerReleaseSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        triggerReleaseSinceLastShot = true;
        shotRemainingInBurst = burstCont;
    }


}
