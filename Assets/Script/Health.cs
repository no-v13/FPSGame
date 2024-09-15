using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp = 10;
    public IHealthListener healthListener;  //Die �Լ� ������, ���� �˸� ���� ��

    public float invincibleTime;            //�����ð� ����:�ν����Ϳ��� 0.5�� ������-(#)
    float lastAttackedTime;                 //���������� ���� �ð�=>�����ð� ���� ����-(#)

    public Image hpGauge;                   //UI�������� ����-($)
    float maxHP;//-$)
    public AudioClip hitSound;              //-@)����� ���
    public AudioClip dieSound;

    void Start()
    {
        maxHP = hp;
        // ��������� ĳ�����Ͽ� �������̽� ��������-> �� ǥ��:healthListener = GetComponent<Health.IHealthListener>(); �۵�X
        healthListener = GetComponent(typeof(IHealthListener)) as IHealthListener;
    }

    void Update()
    {

    }

    public void Damage(float damage)
    {
        if (hp > 0 && lastAttackedTime + invincibleTime < Time.time) 
        {             //������ ���ݽð����κ��� �����ð���ŭ �ð��� ������-(#)
            hp -= damage;

            if (hpGauge != null)//- ($) �����ϴ� ü�¸� �����ϰ� ��
            {
                hpGauge.fillAmount = hp / maxHP; //ü�� ������ 1, ü�� 0 ->0  -($)
            }
            lastAttackedTime = Time.time;//���� �ٽ� ������-(#)

            if (hp <= 0)
            {
                if (dieSound != null)
                { //-@)����� ���
                    GetComponent<AudioSource>().PlayOneShot(dieSound);
                }

                if (healthListener != null)  //���� �ҽ��� ���� ����� ������

                {
                    healthListener.Die();  // �������̽�(healthListener)�� Die �Լ� ȣ��
                }
            }
            else 
            {
                if (hitSound != null)
                {//-@)����� ���
                    GetComponent<AudioSource>().PlayOneShot(hitSound);
                }
            }
        }
    }

    public interface IHealthListener//����ó�� �˸�_15�� �������̽� ����
    {
        void Die(); // ���� �˸� �ް� ������ Die �Լ� ������ �־�� ��
    }
}
