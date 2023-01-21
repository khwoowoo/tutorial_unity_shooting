using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 컴포넌트가 무조건 필요하다 했을 때 작성
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{

    public float moveSpeed = 5;
    public Crosshairs crosshairs;
    PlayerController controller;
    GunController gunController;
    Camera viewCamera;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //수평, 수직 버튼 입력을 받으면, -1f~1f의 값을 반환
        //부드러운 이동이 필요한 경우 사용
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        //레이를 사용해서 화면상의 위치를 반환
        //레이가 무한정으로 뻗어 나가는 데
        //바닥에 닿을 때는 멈춰줘야 함
        //근데 모든 바닥지점을 볼 필요는 없고
        //법선 백터만 확인하면 됨
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        //Plane ground = new Plane(Vector3.up, Vector3.zero);
        Plane ground = new Plane(Vector3.up, Vector3.up * gunController.GunHieght);
        float rayDistance;

        //out이라는 키워드는 참조라는 뜻
        if(ground.Raycast(ray, out rayDistance)) {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
            //Debug.DrawLine(ray.origin, point, Color.red);
            crosshairs.transform.position = point;
            crosshairs.DetecTargets(ray);
        }

        //weapon input
        if (Input.GetMouseButton(0))
        {
            gunController.OnTriggerHold();
        }
        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerRelease();
        }

    }
}
