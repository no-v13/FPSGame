using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon //웨폰(총)상속
{//무엇을 던질지, 어떻게 던질지 코드 짜야 함

    public GameObject projectilePrefab;     //무엇을 던질지= 던져지는 피사체
    public float projectileAngle=30;        //던지는 각도
    public float projectileForce=10;        //던지는 힘
    public float projectileTime=5;            //수류탄 날아가서 몇초 후에 터질건지

    protected override void Fire()
    {//상속받고-> ProjectileFire 불러오게 오버라이드
        ProjectileFire();
    }

    void ProjectileFire() //던지는 -부분
    {
        Camera cam = Camera.main;//카메라가 보는 방향으로 던지니까 캠 가져옴

        Vector3 direction = cam.transform.forward; //일단 캠이 앞쪽을 바라보는 방향 가져옴

        //수류탄을 위로 30도로 던져지게 벡터 방향 세팅:Quaternion
        direction = Quaternion.AngleAxis(-projectileAngle,transform.right)*direction;
        //-projectileAngle: 아래에서 위로 30도 던지는 방향, transform.right: 가로방향(허리뚫음)을 축으로!
        direction.Normalize();
        direction *= projectileForce;//Normalize된 것에 힘 곱함

        GameObject obj = Instantiate(projectilePrefab);     //옵젝 
        obj.transform.position = firingPosition.position;   //옵젝 위치= 총구위치

        obj.GetComponent <Bomb>().time = projectileTime; //폭탄 스크립트 작성후: 폭탄이 5초후에 터지게 함
        obj.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);//디렉션 방향으로 힘 가함
        
    }
}
