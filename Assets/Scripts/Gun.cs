using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MuzzleFlash))]
public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100f;
    public float muzzleVelocity = 35f;

    public Transform shell;
    public Transform shellEjection;

    public MuzzleFlash muzzleFlash;

    float nextShotTime;

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
    }

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProejctile = Instantiate(projectile, muzzle.position, muzzle.rotation);
            newProejctile.SetSpeed(muzzleVelocity);

            Instantiate(shell, shellEjection.position, transform.rotation);
            muzzleFlash.Activate();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
