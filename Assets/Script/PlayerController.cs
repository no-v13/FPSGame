using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{
    public float walkingSpeed = 7;
    public float mouseSens = 10; //마우스 감도 설정 -2)캐릭터 시야 방향 변화, (커서이용)

    public Transform cameraTransform;//시야변화를 카메라가 따라옴-2)
    float verticalAngle;
    float horizontalAngle; //수평,수직 각도필요

    float verticalSpeed; //-3) 캐릭터 중력 계산

    bool isGrounded; //-4) 점프 위해서 이게 땅에 붙었는지 확인
    float groundedTimer;
    public float jumpSpeed =10;
    CharacterController characterController;

    public GameObject[] weapons; //-6)플레이어가 총,폭탄을 번갈아서 쓸수 있게 할거임
    int currentWeapon;

    void Start()
    {   
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;  //게임 시작시 커서 숨김

        verticalAngle = 0; //겜 시작시 수평각도=0
        horizontalAngle =transform.localEulerAngles.y;//시작시 캐릭터의 현재위치 받음(수평각도는 0 아닐수도 있으니까)
        
        verticalSpeed =0; //-3) 기본0, 낙하시 속도변화함

        isGrounded = true;//-4) 기본값 땅에 붙음
        groundedTimer = 0;

        characterController = GetComponent<CharacterController>();

        currentWeapon = 0;//-6) 처음 무기->총! 
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////[캐릭터 평행이동]////////////////////
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        //move= 입력에 따른 벡터 값 가짐/Horizontal;a,d 움직임->x값/Vertical;커서 위아래->z값

        if (move.magnitude > 1) //움직임을 부드럽게 함
        {
            move.Normalize(); //대각선으로 가면 떠 빨리 가는 상황 막음
        }
        move = move*walkingSpeed*Time.deltaTime;
        move = transform.TransformDirection(move);//move벡터 캐릭터가 바라보는 방향으로 바꿈:내가 보고 있는 방향으로 가도록
        characterController.Move(move);//캐릭터 컨트롤러에게 지금 요청한 벡터대로 움직이라고 명령

        ////////////////////////////[좌우 마우스]-2)/////////////////
        float turnPlayer = Input.GetAxis("Mouse X") * mouseSens; //Mouse X:가로 움직임 알 수 있음, 마우스 왼:-/중앙:0/오;+값
        horizontalAngle += turnPlayer; //좌우각도 연산

        if (horizontalAngle > 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360; //각도가 0~360 사이에 있게끔 만듦

        Vector3 currentAngle = transform.localEulerAngles; //현재 캐릭터가 보는 가로 방향 가져옴
        currentAngle.y = horizontalAngle; //y방향을 축으로 각도 변경-> y 방향 바꿔서 돌게 함
        transform.localEulerAngles = currentAngle;

        ///////////////[상하 마우스]-2):움직임 각도와 노상관, 쳐다보는 각도만 변경///////
        float turnCam = Input.GetAxis("Mouse Y") * mouseSens;
        verticalAngle -= turnCam; //마우스 y좌표=>위+/아래- ,각도 y: 위-/아래+
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f); //마우스로 위아래 시야 90도 넘지 않게  

        currentAngle = cameraTransform.transform.localEulerAngles;
        currentAngle.x = verticalAngle; //x축: 옆구리, x축 중심으로 시야 위아래 돌림
        cameraTransform.localEulerAngles = currentAngle;

        ////////////[낙하-3)]///////////
        verticalSpeed -= 10 * Time.deltaTime;//아래로 떨어짐:-값
        if (verticalSpeed < -10)//종단속도값 지정: 속도가 무한히 빠르게 떨어지기 않도록(공기저항,,,)
        {
            verticalSpeed = -10;
        }

        Vector3 verticalMove = new Vector3 (0, verticalSpeed, 0);
        verticalMove = verticalMove * Time.deltaTime; 
        CollisionFlags flag = characterController.Move(verticalMove); //컨트롤러에 수직벡터방향 입력&리턴 값-> CollisionFlags을 앞에 받아줌
        // CollisionFlags: 어디에 부딪힘 알려줌 -> 바닥 충돌시 멈춤
        
        if ((flag & CollisionFlags.Below) != 0)
        {//Below와 충돌 정보가 있냐? 0이 아니면-> 있다
            verticalSpeed = 0;//낙하 멈춤
        }

        //////[캐릭 점프-part1:땅이랑 붙어있니?]////////
        if (!characterController.isGrounded)
        {
            if (isGrounded)
            { //캐릭터 컨트롤러가 0.5초 이상동안 계속 땅에 안 붙어있다고 함=> 땅이랑 떨어짐
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.5f)
                {
                    isGrounded = false;
                }//jump-if(c)
            }//jump-if(b)
        }//jump-if(a)
        else
        {//근데 한번이라도 붙었다고 말하면 진짜 땅에 붙어있음
            isGrounded = true;
            groundedTimer = 0;// 전부 초기화
        }//else

        //////[캐릭 점프-part2; 진점프]////////
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false; //뛰었으니까 땅이랑 떨어짐
        }

        //////[무기변경]-6)///////
        if (Input.GetButtonDown("ChangeWeapon"))
        {
            currentWeapon++;
            if (currentWeapon >= 2) //2대신weapons.Length로 표시 
            {
                currentWeapon = 0;
            }
            UpdateWeapon();
        }
        void UpdateWeapon()
        {//현재무기:켬, 해당X무기:끔
            foreach (GameObject w in weapons)
            {
                w.SetActive(false);
            }
            weapons[currentWeapon].SetActive(true);
        }
    }//update

    public void Die()//죽으면 게임매니저한테 알려야함: 프로세스- 나 죽음- 애니재생- 겜매니저 알림
    {
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("TellmeDied", 1.0f);
    }
    void TellmeDied()
    {
        GameManager.instance.GameOverScene(); //겜오버 씬으로 넘김
    }
}
