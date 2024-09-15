using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{//�ð� ������ ��ź �������Τ�!

    public float time;
    public float damage;
    public AudioClip explosionSound;//-@)��������
    void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0) 
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {//������� ����ǰ� ������ �� ������� �ʾƾ���-@)
                GetComponent<AudioSource>().PlayOneShot(explosionSound);//-@)��������
            }
            GetComponent<Animator>().SetTrigger("Explosion");
            Invoke("DestroyThis", 3.0f); //0.5�� �Ŀ� DestroyThi����->���߾ִ� ���� 
        }

    }//updat

    void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {//�ڽĿ����� isTrigger ��������->�θ������ Ʈ���� ���� ����
        if (other.tag == "Enemy") 
        { //�΋H�� ��ü�� �±װ� ���ʹ�
            other.GetComponent<Health>().Damage(damage); //�ｺ���� �������Լ� �ҷ���
        }
    }

}//B
