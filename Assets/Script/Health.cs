using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float hp = 10;
    public IHealthListener healthListener;  //Die 함수 가질애, 죽음 알림 들을 애

    public float invincibleTime;            //무적시간 설정:인스펙터에서 0.5로 설정함-(#)
    float lastAttackedTime;                 //마지막으로 맞은 시간=>무적시간 설정 위함-(#)

    public Image hpGauge;                   //UI게이지바 관리-($)
    float maxHP;//-$)
    public AudioClip hitSound;              //-@)오디오 재생
    public AudioClip dieSound;

    void Start()
    {
        maxHP = hp;
        // 명시적으로 캐스팅하여 인터페이스 가져오기-> 쌤 표현:healthListener = GetComponent<Health.IHealthListener>(); 작동X
        healthListener = GetComponent(typeof(IHealthListener)) as IHealthListener;
    }

    void Update()
    {

    }

    public void Damage(float damage)
    {
        if (hp > 0 && lastAttackedTime + invincibleTime < Time.time) 
        {             //마지막 공격시간으로부터 무적시간만큼 시간이 지나면-(#)
            hp -= damage;

            if (hpGauge != null)//- ($) 존재하는 체력만 관리하게 함
            {
                hpGauge.fillAmount = hp / maxHP; //체력 꽉차면 1, 체력 0 ->0  -($)
            }
            lastAttackedTime = Time.time;//지금 다시 공격함-(#)

            if (hp <= 0)
            {
                if (dieSound != null)
                { //-@)오디오 재생
                    GetComponent<AudioSource>().PlayOneShot(dieSound);
                }

                if (healthListener != null)  //죽음 소식을 들을 사람이 있으면

                {
                    healthListener.Die();  // 인터페이스(healthListener)의 Die 함수 호출
                }
            }
            else 
            {
                if (hitSound != null)
                {//-@)오디오 재생
                    GetComponent<AudioSource>().PlayOneShot(hitSound);
                }
            }
        }
    }

    public interface IHealthListener//죽음처리 알림_15강 인터페이스 참조
    {
        void Die(); // 죽음 알림 받고 싶으면 Die 함수 가지고 있어야 함
    }
}
