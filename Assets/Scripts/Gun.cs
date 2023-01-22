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
    public int projectilesPerMag;
    public float reloadTime = .3f; 

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(0.05f, 2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotateSettleTime = .1f;

    [Header("Effects")]
    public Transform shell;
    public Transform shellEjection;

    MuzzleFlash muzzleFlash;

    float nextShotTime;

    bool triggerReleaseSinceLastShot;
    int shotRemainingInBurst;
    int projectilesRemainingInMag;
    bool isReloading;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotRemainingInBurst = burstCont;
        projectilesRemainingInMag = projectilesPerMag;
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotateSettleTime);
        //transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if(!isReloading && projectilesRemainingInMag == 0)
        {
            Reload();
        }
    }

    public void Aim(Vector3 aimPoint)
    {
        if (!isReloading)
        {
            transform.LookAt(aimPoint);
        }
    }

    void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
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
                if(projectilesRemainingInMag == 0)
                {
                    break;
                }
                projectilesRemainingInMag--;
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProejctile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation);
                newProejctile.SetSpeed(muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30f);
        }
        
    }

    public void Reload()
    {
        //탄이 맥스가 아닌 경우, 재장전 중이 아닌 경우
        if(!isReloading && projectilesPerMag != projectilesRemainingInMag)
        {
            StartCoroutine(AnimateReload());
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(0.2f);

        float percent = 0;
        float reloadSpeed = 1f / reloadTime;
        Vector3 initailRotate = transform.localEulerAngles;
        float maxReloadAngle = 30f;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initailRotate + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMag = projectilesPerMag;
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
