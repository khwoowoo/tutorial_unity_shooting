using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{

    //idle -> 아무것도 안 하는 상태
    //Chasing -> 플레이어를 추격하는 상태
    //Attcking -> 공격하는 상태
    public enum State
    {
        Idle,Chasing, Attacking
    };
    State currentState;

    public ParticleSystem deathEffect;
    public static event System.Action OnDeathStatic;

    NavMeshAgent pathfinder;
    Transform target;
    Material skinMaterial;
    LivingEntity targetEntity;

    Color originColor;

    float attackDistanceThreshold = 0.5f;
    float timeBetweenAttacks = 1f;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRaduis;
    float damage = 1.0f;

    bool hasTarget;

    private void Awake()
    {
        //이런식으로 tag를 사용하여 객체을 참조할 수 있음
        pathfinder = GetComponent<NavMeshAgent>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRaduis = target.GetComponent<CapsuleCollider>().radius;
        }
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //이런식으로 tag를 사용하여 객체을 참조할 수 있음
        pathfinder = GetComponent<NavMeshAgent>();

        if(hasTarget)
        {
            currentState = State.Chasing;
            targetEntity.OnDeath += OnTargetDeath;
            StartCoroutine(UpdatePath());
        }

    }
    public void SetCharacteristics(float movespeed, int hitsToKillPlayer, float enemyHealth, Color skinColor)
    {
        pathfinder.speed = movespeed;

        if (hasTarget)
        {
            damage = targetEntity.startingHealth / hitsToKillPlayer;
        }

        startingHealth = enemyHealth;

        deathEffect.startColor = new Color(skinColor.r, skinColor.g, skinColor.b, 1);
        skinMaterial = GetComponent<Renderer>().material;
        skinMaterial.color = skinColor;
        originColor = skinMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {

        if (hasTarget)
        {
            //pathfinder.SetDestination(target.position);


            if (Time.time > nextAttackTime)
            {

                //거리를 구하는 Distance 메소드는 제곱근 연산을 하기 때문에
                //update에서 돌아기에는 무리가 있다
                //Vector3.Distance()
                //따라서 그냥 제곱으로만 연산한다

                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + attackDistanceThreshold, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                    AudioManager.instance.PlaySound("Emeny attack", transform.position);
                }
            }
        }

    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        AudioManager.instance.PlaySound("Impact", transform.position);
        if(damage >= health)
        {
            if(OnDeathStatic != null)
            {
                OnDeathStatic();
            }
            AudioManager.instance.PlaySound("Emeny death", transform.position);
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)), deathEffect.startLifetime);
        }

        base.TakeHit(damage, hitPoint, hitDirection);
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }


    IEnumerator Attack()
    {
        currentState = State.Attacking;
        //공격을 하는 동안 추적을 안 할 것이기 때문에
        //pathfinder는 잠깐 꺼두고 공격 애니메이션이 끝나고 다시 킨다
        pathfinder.enabled = false;

        Vector3 originPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float percent = 0;
        float attackSpeed = 3;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if(percent >= .5f && hasAppliedDamage == false)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            //공격을 할 때는 대칭함수를 사용해서
            //다시 돌아오게 만든다.
            //이동은 선형보간
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originPosition, attackPosition, interpolation);

            yield return null;
        }


        hasAppliedDamage = true;
        currentState = State.Chasing;
        pathfinder.enabled = true;
        skinMaterial.color = originColor;
    }

    IEnumerator UpdatePath()
    {
        float refeshRate = 0.25f;

        while (hasTarget)
        {
            //추격하는 상태에서만
            if(currentState == State.Chasing)
            {
                //죽지 않은 경우만 추적, 이건 혹시 몰라서
                //Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (targetCollisionRaduis + myCollisionRadius + attackDistanceThreshold / 2);
                if (!isDie)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
           
            yield return new WaitForSeconds(refeshRate);
        }
    }
}
