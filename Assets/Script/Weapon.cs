using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public TextMeshProUGUI bulletNumLabel;//�Ѿ����� ȭ�鿡 ǥ���Ϸ��� �ν����Ϳ� �˷���
    public GameObject trailPrefabs; //�Ѿ� ���� ������ ����� �˷���
    public Transform firingPosition; //������ �ѱ����� ���� �� �ֵ��� �ѱ� ��ġ ������;
    public AudioClip gunShotSound;//�ѼҸ� ���-@)

    public GameObject particlePrefab; //����ķ�� ��ƼŬ ȿ�� ���� �� �ֵ��� ��-4)

    public int bullet; //���� ���� �Ѿ� ��
    public int totalBullet;
    public int maxBulletInMagazine;//�� źâ�� �� ���� 
    //=> �ν����Ϳ��� �Ѿ� ���� ����O, ������ �� 8,32,8�� �ص�

    Animator animator;//�ִϸ����Ͱ� ���� ��� ���� �� ����=> ����ź�� ȿ�� �� ���� ����-5)����ź ���

    public float damage;//���� �� ������ ������-5)

    void Start()
    {
        animator = GetComponent<Animator>();//-5) �ִϸ����Ͱ� ȿ���� ������ �ִ��� ���� Ȯ��-5)   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && bullet>0)
        {//�Ѿ˹߻� 
            Fire();
            bullet--;
            if (animator != null)
            {
                animator.SetTrigger("Shot");//�ִ�ȿ��& -5)����ź������ �ִϸ����� �ް� �ִ��� Ȯ���� �ִ�ȿ��
            }
        }

        if (Input.GetButtonDown("Reload"))
        {
            if (animator != null)
            {
                GetComponent<Animator>().SetTrigger("Reload");
            }

            if (totalBullet >= maxBulletInMagazine - bullet)
            {//�����մ� �Ѿ�>=źâ���ڸ�
                totalBullet-=maxBulletInMagazine-bullet;
                bullet = maxBulletInMagazine;//źâ����ŭ ��ä��
            }

            else
            {//�Ѿ� ���ڶ���
                bullet += totalBullet; //������ ���� �־�
                totalBullet = 0; //�׷� ��ü�� 0
            }
        }//reloa-if1

        bulletNumLabel.text =bullet + "/" + totalBullet; //-> [���簳��/��������] ǥ��

    }//updat

    virtual protected void Fire()
    {//��ӹް� ������Ʈ�������� ���ľ�����(�������̵�)=>virtual ����
        RaycastFire();
    }
    void RaycastFire()
    {
        GetComponent<AudioSource>().PlayOneShot(gunShotSound);          //�ѼҸ� ���-@)
        Camera cam = Camera.main; //����ķ �ҷ���-> �ѽ� ķ �߾ӿ��� ��ϱ�

        RaycastHit hit; //���� ���� ��� �浹�ߴ��� ���� �˰� ��
        Ray r = cam.ViewportPointToRay(Vector3.one / 2); //�߻��� �� -> ķ�߾�,ķ�� ���� ����Ʈ �߾�
                 //����Ʈ: ȭ����ü��(1,1)�̶�� ����, Vector3.one:(1,1,1), [Vector3.one / 2: (0.5,0.5)= ȭ�����߾�]

        Vector3 hitPoisition = r.origin + r.direction*200;
        //r.origin:����(r)�� ��������,  r.direction*200: 200���� ������ �������� �Ÿ�����

        if (Physics.Raycast(r,out hit, 1000))
        {//Physics.Raycast: ���� ��ڴ�.��� ���� �΋H���� true, �� �΋H? fasle=> if ���?�浹�ߴ� ��!
         //r: �� ��(ķ �߾ӿ��� ������ ��), out hit:����ĳ��Ʈ�� �� �� ����� hit�� ����, 1000:�ִ�Ÿ�

            hitPoisition = hit.point; //hit:�浹��� ����, hit.point => �浹���� ����
            // ����) �浹O: if���� hitPoisition�� �浹���� ����/ �浹X:0~200���ͱ��� �� �׸�

            /////[��ƼŬ ȿ��]-4)/////
            GameObject particle = Instantiate(particlePrefab);//���������� ����
            particle.transform.position = hitPoisition;//������ ���� ��ġ(hitPoisition = hit.point)�� ��ƼŬ ������-4)
            particle.transform.forward = hit.normal; //��ƼŬ�� �°� Ƣ�� ����=>�浹����� 90��(����):hit.normal
        
            /////[�Ѹ´°� ���ϋ� ó��-5)]/////
            if(hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Health>().Damage(damage);
            }


        }

        if (trailPrefabs != null)
        {//���� �ȵ帮�� ����
            GameObject obj = Instantiate(trailPrefabs); //Instantiate: �� ����
            Vector3[] pos = new Vector3[] { firingPosition.position, hitPoisition };
            //transform.position: ���� �ִ� ����(�������),hitPoisition: ���� ����(�� ��������)
            //-> transfor�� firingPosition���� ��ü :�߻������� �ѱ� ��ġ�� ����
            
            obj.GetComponent<LineRenderer>().SetPositions(pos); //���� �� ��ġ�� LineRenderer�� �˸�-> �� �׾���

            //�Ѿ˱��� �����-> Coroutine���, �ٵ� ��� IEnumerate Ÿ�� ��ȯ
            //���� ������� �ð� �帣�� �Լ� ����? Invoke����,�ٵ� ��� ��Ʈ������ �Լ��� �����ؼ� �Ķ���� �� �ѱ�
        
        StartCoroutine(RemoveTrail(obj));//RemoveTrail ȣ���ϰ� ��� ��ٸ��� �ʰ� �׳� ���డ��
            //(���� ������� �ð� �帣�� �Լ� ����? Invoke����, �ٵ� ��� ��Ʈ������ �Լ��� �����ؼ� �Ķ���� �� �ѱ�
        }

    }//raycast

    IEnumerator RemoveTrail(GameObject obj) 
    {
        yield return new WaitForSeconds(0.3f); //(yield:�纸) -> ���⼭ ����� �ð� �� �� ����
        Destroy(obj);//����; 0.3�� ��ٸ��� ����
    }

}//waepon
