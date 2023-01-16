using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100f;
    public float muzzleVelocity = 35f;

    float nextShotTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProejctile = Instantiate(projectile, muzzle.position, muzzle.rotation);
            newProejctile.SetSpeed(muzzleVelocity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
