using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Health.IHealthListener // Health Ŭ���� ���� �������̽� ����-(*)
{
    public GameObject player;                   //�� ��ġ �ҷ���
    NavMeshAgent agent;                         //��ĳ�� ��ã�⿡����Ʈ ���ϰ�
    Animator animator;                          //�ִ�ȿ�� ����-@)

    enum State //������� ����
    {
        Idle,
        Walk,
        Attack,
        Dying
    }

    State state;                                //������� ���庯��
    float timeForNextState = 2;                 //����Ǵµ� �ʿ��� �ð�

    AudioSource audio;                         //�߼Ҹ� ���������� ������ҽ� �θ�-@)

    void Start()
    {
        audio = GetComponent<AudioSource>();   //�߼Ҹ� ���������� ������ҽ� �θ�
        animator = GetComponent<Animator>();    //�ִ�ȿ��
        agent = GetComponent<NavMeshAgent>();   //��ĳ�� ��ã�⿡����Ʈ ����
        state = State.Idle;                     //ó�����۽� Idle���·� ����

    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                float distance = (player.transform.position -
                    (transform.position + GetComponent<CapsuleCollider>().center)).magnitude;
                //��ĳ���� ��ġ ������ + ��ĳ�߽����� �ݶ��̴� �߽� ������)).��ü ����ũ��(=magnitude)
                //distance = (����ġ - ��ĳ�߽ɱ��� ������ġ)�� ũ�� => �Ÿ� ���!!


                if (distance < 1.0f) //�Ÿ��� 1���ͺ��� ������ ����

                {
                    Attack();
                }
                else                 //�Ÿ��� 1���ͺ��� �ָ� 
                {
                    timeForNextState -= Time.deltaTime; //�ð��� ������ ��ٸ�
                    if (timeForNextState < 0)           //�ð� �� ������ ���� �ൿ����

                    {
                        StartWalk();
                    }
                }
                break;

            case State.Walk://��ǥ������ �����ߴ����� �߿�

                if (agent.remainingDistance < 1.0f || !agent.hasPath) //�� �Ÿ��� 1���͹̸��̸� ���� || ���� ���� ���� ������ ���±��̸�
                {//���� ��ǥ������ �����߰ų� || ������ ����� ������
                    StartIdle(); //����
                }
                break;


            case State.Attack:                      //������ �����ð� ������ �ٽ� ����-> ���� ���۰���
                timeForNextState -= Time.deltaTime;
                if (timeForNextState < 0)           //�����ð�<0�̸� ����
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

    void StartWalk()//�׷� ����ð� �������� �ϴ� ����� ��°?
    {
        state = State.Walk;                             //�ȱ�� �ٲ�
        agent.destination = player.transform.position;  //������Ʈ �������� �� ��ġ
        agent.isStopped = false;                        //������Ʈ ����(=.isStopped) ��-> �̵��ϰ�!
        animator.SetTrigger("Walk");                    //�ȱ� �ִ� ����
    }

    void StartIdle()
    {
        audio.Play();                               //����� ���-@)
        state = State.Idle;                         //������->����
        timeForNextState = Random.Range(1f, 2f);    //���½ð� 1~2�ʻ���
        agent.isStopped = true;                     //������Ʈ ����
        animator.SetTrigger("Idle");                //����ִ� ����
    }



    // public���� ������ Die �Լ�
    public void Die()
    {
        audio.Stop();                               //����� ����-@)
        state = State.Dying;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("DestroyThis",2.5f);
    }

    void DestroyThis()
    {
        GameManager.instance.EnemyDied(); //���������� ���ӸŴ������� �˷������
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {//�ڽĿ����� isTrigger ��������->�θ������ Ʈ���� ���� ����
        if (other.tag =="Player") 
        {
            other.GetComponent<Health>().Damage(5);
        }
    }
}
