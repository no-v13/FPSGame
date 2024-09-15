using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{//시간 지나면 폭탄 터지도로ㄱ!

    public float time;
    public float damage;
    public AudioClip explosionSound;//-@)오디오재생
    void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0) 
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {//오디오가 재생되고 있으면 또 재생되지 않아야함-@)
                GetComponent<AudioSource>().PlayOneShot(explosionSound);//-@)오디오재생
            }
            GetComponent<Animator>().SetTrigger("Explosion");
            Invoke("DestroyThis", 3.0f); //0.5초 후에 DestroyThi실행->폭발애니 삭제 
        }

    }//updat

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {//자식옵젝에 isTrigger 켜져있음->부모옵젝이 트리거 감지 가능
        if (other.tag == "Enemy") 
        { //부딫힌 물체의 태그가 에너미
            other.GetComponent<Health>().Damage(damage); //헬스에서 데미지함수 불러와
        }
    }

}//B
