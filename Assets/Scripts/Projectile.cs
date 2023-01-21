using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //LayterMask�� � ������Ʈ, � ���̾ �߻�ü�� �ѵ����� ���� �� ����.
    public LayerMask collisionMask;
    public Color trailColor;
    float speed = 10f;
    float damage = 1f;

    float skinWidth = .1f;

    float lifeTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);

        //�߻�ü�� �����ִ� �ݶ����� ������
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);

        //�ϳ��� ������, �������� �ְ� ����
        if(initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }

        //GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        

        if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }

    }

    //void OnHitObject(RaycastHit hit)
    //{
    //    IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();

    //    if(damageableObject != null)
    //    {
    //        damageableObject.TakeHit(damage, hit);
    //    }

    //    GameObject.Destroy(gameObject);
    //}

    void OnHitObject(Collider collider, Vector3 hitPoint)
    {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();

        if (damageableObject != null)
        {
            //damageableObject.TakeDamage(damage);
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }

        GameObject.Destroy(gameObject);
    }
}
