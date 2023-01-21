using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun startingGun;

    Gun equippedGun;

    // Start is called before the first frame update
    void Start()
    {
        //시작 총이 있으면
        if(startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerHold()
    {
        if(equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }

    public void EquipGun(Gun gunToEquip)
    {
        //이미 총이 존재하면 삭제 하고 다시 생성해줌
        if(equippedGun != null) {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate<Gun>(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGun.transform.parent = weaponHold;
    }

    public float GunHieght
    {
        get
        {
            return weaponHold.position.y;
        }
    }
}
