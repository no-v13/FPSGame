using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon //����(��)���
{//������ ������, ��� ������ �ڵ� ¥�� ��

    public GameObject projectilePrefab;     //������ ������= �������� �ǻ�ü
    public float projectileAngle=30;        //������ ����
    public float projectileForce=10;        //������ ��
    public float projectileTime=5;            //����ź ���ư��� ���� �Ŀ� ��������

    protected override void Fire()
    {//��ӹް�-> ProjectileFire �ҷ����� �������̵�
        ProjectileFire();
    }

    void ProjectileFire() //������ -�κ�
    {
        Camera cam = Camera.main;//ī�޶� ���� �������� �����ϱ� ķ ������

        Vector3 direction = cam.transform.forward; //�ϴ� ķ�� ������ �ٶ󺸴� ���� ������

        //����ź�� ���� 30���� �������� ���� ���� ����:Quaternion
        direction = Quaternion.AngleAxis(-projectileAngle,transform.right)*direction;
        //-projectileAngle: �Ʒ����� ���� 30�� ������ ����, transform.right: ���ι���(�㸮����)�� ������!
        direction.Normalize();
        direction *= projectileForce;//Normalize�� �Ϳ� �� ����

        GameObject obj = Instantiate(projectilePrefab);     //���� 
        obj.transform.position = firingPosition.position;   //���� ��ġ= �ѱ���ġ

        obj.GetComponent <Bomb>().time = projectileTime; //��ź ��ũ��Ʈ �ۼ���: ��ź�� 5���Ŀ� ������ ��
        obj.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);//�𷺼� �������� �� ����
        
    }
}
