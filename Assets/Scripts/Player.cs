using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ������Ʈ�� ������ �ʿ��ϴ� ���� �� �ۼ�
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
        //����, ���� ��ư �Է��� ������, -1f~1f�� ���� ��ȯ
        //�ε巯�� �̵��� �ʿ��� ��� ���
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        //���̸� ����ؼ� ȭ����� ��ġ�� ��ȯ
        //���̰� ���������� ���� ������ ��
        //�ٴڿ� ���� ���� ������� ��
        //�ٵ� ��� �ٴ������� �� �ʿ�� ����
        //���� ���͸� Ȯ���ϸ� ��
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        //Plane ground = new Plane(Vector3.up, Vector3.zero);
        Plane ground = new Plane(Vector3.up, Vector3.up * gunController.GunHieght);
        float rayDistance;

        //out�̶�� Ű����� ������� ��
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
