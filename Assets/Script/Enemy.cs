using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Health.IHealthListener // Health 클래스 내부 인터페이스 구현-(*)
{
    public GameObject player;                   //내 위치 불러옴
    NavMeshAgent agent;                         //적캐에 길찾기에이전트 붙일거
    Animator animator;                          //애니효과 적용-@)

    enum State //현재상태 저장
    {
        Idle,
        Walk,
        Attack,
        Dying
    }

    State state;                                //현재상태 저장변수
    float timeForNextState = 2;                 //이행되는데 필요한 시간

    AudioSource audio;                         //발소리 가져오려고 오디오소스 부름-@)

    void Start()
    {
        audio = GetComponent<AudioSource>();   //발소리 가져오려고 오디오소스 부름
        animator = GetComponent<Animator>();    //애니효과
        agent = GetComponent<NavMeshAgent>();   //적캐에 길찾기에이전트 붙임
        state = State.Idle;                     //처음시작시 Idle상태로 시작

    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                float distance = (player.transform.position -
                    (transform.position + GetComponent<CapsuleCollider>().center)).magnitude;
                //적캐릭터 위치 가져옴 + 적캐중심으로 콜라이더 중심 가져옴)).전체 벡터크기(=magnitude)
                //distance = (내위치 - 적캐중심기준 적개위치)의 크기 => 거리 계산!!


                if (distance < 1.0f) //거리가 1미터보다 가까우면 공격

                {
                    Attack();
                }
                else                 //거리가 1미터보다 멀면 
                {
                    timeForNextState -= Time.deltaTime; //시간이 지나길 기다림
                    if (timeForNextState < 0)           //시간 다 지나면 다음 행동이행

                    {
                        StartWalk();
                    }
                }
                break;

            case State.Walk://목표지점에 도달했는지가 중요

                if (agent.remainingDistance < 1.0f || !agent.hasPath) //적 거리가 1미터미만이면 도착 || 적이 가는 길이 실제론 없는길이면
                {//적이 목표지점에 도착했거나 || 도착할 방법이 없으면
                    StartIdle(); //멈춤
                }
                break;


            case State.Attack:                      //공격후 일정시간 지나면 다시 정지-> 다음 동작결정
                timeForNextState -= Time.deltaTime;
                if (timeForNextState < 0)           //남은시간<0이면 정지
                {
                    StartIdle();
                }
                break;
        }
    }

    void Attack()
    {
        state = State.Attack;
        timeForNextState = 1.5f;
        animator.SetTrigger("Attack");
    }

    void StartWalk()//그럼 멈춤시간 랜덤으로 하는 방법은 우째?
    {
        state = State.Walk;                             //걷기로 바꿈
        agent.destination = player.transform.position;  //에이전트 목적지는 내 위치
        agent.isStopped = false;                        //에이전트 멈춤(=.isStopped) 끔-> 이동하게!
        animator.SetTrigger("Walk");                    //걷기 애니 적용
    }

    void StartIdle()
    {
        audio.Play();                               //오디오 재생-@)
        state = State.Idle;                         //현상태->정지
        timeForNextState = Random.Range(1f, 2f);    //쉬는시간 1~2초사이
        agent.isStopped = true;                     //에이전트 멈춤
        animator.SetTrigger("Idle");                //멈춤애니 적용
    }



    // public으로 유지된 Die 함수
    public void Die()
    {
        audio.Stop();                               //오디오 정지-@)
        state = State.Dying;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("DestroyThis",2.5f);
    }

    void DestroyThis()
    {
        GameManager.instance.EnemyDied(); //죽을때마다 게임매니저한테 알려줘야함
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {//자식옵젝에 isTrigger 켜져있음->부모옵젝이 트리거 감지 가능
        if (other.tag =="Player") 
        {
            other.GetComponent<Health>().Damage(5);
        }
    }
}
