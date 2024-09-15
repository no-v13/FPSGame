using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public TextMeshProUGUI bulletNumLabel;//총알점수 화면에 표시하려고 인스펙터에 알려줌
    public GameObject trailPrefabs; //총알 궤적 프리팹 만든거 알려줌
    public Transform firingPosition; //궤적이 총구에서 나갈 수 있도록 총구 위치 가져옴;
    public AudioClip gunShotSound;//총소리 재생-@)

    public GameObject particlePrefab; //메인캠이 파티클 효과 받을 수 있도록 함-4)

    public int bullet; //현재 가진 총알 수
    public int totalBullet;
    public int maxBulletInMagazine;//한 탄창에 들어갈 갯수 
    //=> 인스펙터에서 총알 개수 조절O, 지금은 각 8,32,8로 해둠

    Animator animator;//애니메이터가 없는 경우 있을 수 있음=> 수류탄은 효과 안 넣을 예정-5)수류탄 상속

    public float damage;//적이 총 맞은때 데미지-5)

    void Start()
    {
        animator = GetComponent<Animator>();//-5) 애니메이터가 효과를 가지고 있는지 먼저 확인-5)   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && bullet>0)
        {//총알발사 
            Fire();
            bullet--;
            if (animator != null)
            {
                animator.SetTrigger("Shot");//애니효과& -5)수류탄때문에 애니메이터 달고 있는지 확인후 애니효과
            }
        }

        if (Input.GetButtonDown("Reload"))
        {
            if (animator != null)
            {
                GetComponent<Animator>().SetTrigger("Reload");
            }

            if (totalBullet >= maxBulletInMagazine - bullet)
            {//남아잇는 총알>=탄창빈자리
                totalBullet-=maxBulletInMagazine-bullet;
                bullet = maxBulletInMagazine;//탄창수망큼 꽉채움
            }

            else
            {//총알 모자랄시
                bullet += totalBullet; //남은거 전부 넣어
                totalBullet = 0; //그럼 전체는 0
            }
        }//reloa-if1

        bulletNumLabel.text =bullet + "/" + totalBullet; //-> [현재개수/남은개수] 표시

    }//updat

    virtual protected void Fire()
    {//상속받고 프로젝트웨폰에서 고쳐쓸거임(오버라이드)=>virtual 붙임
        RaycastFire();
    }
    void RaycastFire()
    {
        GetComponent<AudioSource>().PlayOneShot(gunShotSound);          //총소리 재생-@)
        Camera cam = Camera.main; //메인캠 불러옴-> 총쏠때 캠 중앙에서 쏘니까

        RaycastHit hit; //빛을 쏴서 어디에 충돌했는지 정보 알게 함
        Ray r = cam.ViewportPointToRay(Vector3.one / 2); //발사할 빛 -> 캠중앙,캠이 보는 뷰포트 중앙
                 //뷰포트: 화면전체를(1,1)이라고 가정, Vector3.one:(1,1,1), [Vector3.one / 2: (0.5,0.5)= 화면정중앙]

        Vector3 hitPoisition = r.origin + r.direction*200;
        //r.origin:레이(r)의 원점부터,  r.direction*200: 200미터 떨어진 지점까지 거리지정

        if (Physics.Raycast(r,out hit, 1000))
        {//Physics.Raycast: 빛을 쏘겠다.어딘가 빛이 부딫히면 true, 안 부딫? fasle=> if 통과?충돌했단 뜻!
         //r: 쏠 빛(캠 중앙에서 앞으로 쏨), out hit:레이캐스트가 쏜 빛 결과를 hit이 받음, 1000:최대거리

            hitPoisition = hit.point; //hit:충돌결과 저장, hit.point => 충돌지점 저장
            // 정리) 충돌O: if지나 hitPoisition에 충돌지점 저장/ 충돌X:0~200미터까지 선 그림

            /////[파티클 효과]-4)/////
            GameObject particle = Instantiate(particlePrefab);//프리팹으로 궤적
            particle.transform.position = hitPoisition;//궤적이 맞은 위치(hitPoisition = hit.point)에 파티클 가져옴-4)
            particle.transform.forward = hit.normal; //파티클이 맞고 튀는 방향=>충돌평면의 90도(법선):hit.normal
        
            /////[총맞는게 적일떄 처리-5)]/////
            if(hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Health>().Damage(damage);
            }


        }

        if (trailPrefabs != null)
        {//궤적 안드리는 무기
            GameObject obj = Instantiate(trailPrefabs); //Instantiate: 선 그음
            Vector3[] pos = new Vector3[] { firingPosition.position, hitPoisition };
            //transform.position: 웨폰 있는 지점(출발지점),hitPoisition: 맞은 지점(선 도착지점)
            //-> transfor을 firingPosition으로 교체 :발사지점을 총구 위치로 변경
            
            obj.GetComponent<LineRenderer>().SetPositions(pos); //위의 두 위치를 LineRenderer에 알림-> 선 그어줌

            //총알궤적 지우기-> Coroutine사용, 근데 얘는 IEnumerate 타입 반환
            //원래 어느정도 시간 흐르고 함수 실행? Invoke썼음,근데 얘는 스트링으로 함수명 지정해서 파라미터 못 넘김
        
        StartCoroutine(RemoveTrail(obj));//RemoveTrail 호출하고 결과 기다리지 않고 그냥 실행가능
            //(원래 어느정도 시간 흐르고 함수 실행? Invoke썼음, 근데 얘는 스트링으로 함수명 지정해서 파라미터 못 넘김
        }

    }//raycast

    IEnumerator RemoveTrail(GameObject obj) 
    {
        yield return new WaitForSeconds(0.3f); //(yield:양보) -> 여기서 맘대로 시간 끌 수 있음
        Destroy(obj);//궤적; 0.3초 기다리고 없앰
    }

}//waepon
