using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{
    public float walkingSpeed = 7;
    public float mouseSens = 10; //���콺 ���� ���� -2)ĳ���� �þ� ���� ��ȭ, (Ŀ���̿�)

    public Transform cameraTransform;//�þߺ�ȭ�� ī�޶� �����-2)
    float verticalAngle;
    float horizontalAngle; //����,���� �����ʿ�

    float verticalSpeed; //-3) ĳ���� �߷� ���

    bool isGrounded; //-4) ���� ���ؼ� �̰� ���� �پ����� Ȯ��
    float groundedTimer;
    public float jumpSpeed =10;
    CharacterController characterController;

    public GameObject[] weapons; //-6)�÷��̾ ��,��ź�� �����Ƽ� ���� �ְ� �Ұ���
    int currentWeapon;

    void Start()
    {   
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;  //���� ���۽� Ŀ�� ����

        verticalAngle = 0; //�� ���۽� ���򰢵�=0
        horizontalAngle =transform.localEulerAngles.y;//���۽� ĳ������ ������ġ ����(���򰢵��� 0 �ƴҼ��� �����ϱ�)
        
        verticalSpeed =0; //-3) �⺻0, ���Ͻ� �ӵ���ȭ��

        isGrounded = true;//-4) �⺻�� ���� ����
        groundedTimer = 0;

        characterController = GetComponent<CharacterController>();

        currentWeapon = 0;//-6) ó�� ����->��! 
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////[ĳ���� �����̵�]////////////////////
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
        //move= �Է¿� ���� ���� �� ����/Horizontal;a,d ������->x��/Vertical;Ŀ�� ���Ʒ�->z��

        if (move.magnitude > 1) //�������� �ε巴�� ��
        {
            move.Normalize(); //�밢������ ���� �� ���� ���� ��Ȳ ����
        }
        move = move*walkingSpeed*Time.deltaTime;
        move = transform.TransformDirection(move);//move���� ĳ���Ͱ� �ٶ󺸴� �������� �ٲ�:���� ���� �ִ� �������� ������
        characterController.Move(move);//ĳ���� ��Ʈ�ѷ����� ���� ��û�� ���ʹ�� �����̶�� ���

        ////////////////////////////[�¿� ���콺]-2)/////////////////
        float turnPlayer = Input.GetAxis("Mouse X") * mouseSens; //Mouse X:���� ������ �� �� ����, ���콺 ��:-/�߾�:0/��;+��
        horizontalAngle += turnPlayer; //�¿찢�� ����

        if (horizontalAngle > 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360; //������ 0~360 ���̿� �ְԲ� ����

        Vector3 currentAngle = transform.localEulerAngles; //���� ĳ���Ͱ� ���� ���� ���� ������
        currentAngle.y = horizontalAngle; //y������ ������ ���� ����-> y ���� �ٲ㼭 ���� ��
        transform.localEulerAngles = currentAngle;

        ///////////////[���� ���콺]-2):������ ������ ����, �Ĵٺ��� ������ ����///////
        float turnCam = Input.GetAxis("Mouse Y") * mouseSens;
        verticalAngle -= turnCam; //���콺 y��ǥ=>��+/�Ʒ�- ,���� y: ��-/�Ʒ�+
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f); //���콺�� ���Ʒ� �þ� 90�� ���� �ʰ�  

        currentAngle = cameraTransform.transform.localEulerAngles;
        currentAngle.x = verticalAngle; //x��: ������, x�� �߽����� �þ� ���Ʒ� ����
        cameraTransform.localEulerAngles = currentAngle;

        ////////////[����-3)]///////////
        verticalSpeed -= 10 * Time.deltaTime;//�Ʒ��� ������:-��
        if (verticalSpeed < -10)//���ܼӵ��� ����: �ӵ��� ������ ������ �������� �ʵ���(��������,,,)
        {
            verticalSpeed = -10;
        }

        Vector3 verticalMove = new Vector3 (0, verticalSpeed, 0);
        verticalMove = verticalMove * Time.deltaTime; 
        CollisionFlags flag = characterController.Move(verticalMove); //��Ʈ�ѷ��� �������͹��� �Է�&���� ��-> CollisionFlags�� �տ� �޾���
        // CollisionFlags: ��� �ε��� �˷��� -> �ٴ� �浹�� ����
        
        if ((flag & CollisionFlags.Below) != 0)
        {//Below�� �浹 ������ �ֳ�? 0�� �ƴϸ�-> �ִ�
            verticalSpeed = 0;//���� ����
        }

        //////[ĳ�� ����-part1:���̶� �پ��ִ�?]////////
        if (!characterController.isGrounded)
        {
            if (isGrounded)
            { //ĳ���� ��Ʈ�ѷ��� 0.5�� �̻󵿾� ��� ���� �� �پ��ִٰ� ��=> ���̶� ������
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.5f)
                {
                    isGrounded = false;
                }//jump-if(c)
            }//jump-if(b)
        }//jump-if(a)
        else
        {//�ٵ� �ѹ��̶� �پ��ٰ� ���ϸ� ��¥ ���� �پ�����
            isGrounded = true;
            groundedTimer = 0;// ���� �ʱ�ȭ
        }//else

        //////[ĳ�� ����-part2; ������]////////
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false; //�پ����ϱ� ���̶� ������
        }

        //////[���⺯��]-6)///////
        if (Input.GetButtonDown("ChangeWeapon"))
        {
            currentWeapon++;
            if (currentWeapon >= 2) //2���weapons.Length�� ǥ�� 
            {
                currentWeapon = 0;
            }
            UpdateWeapon();
        }
        void UpdateWeapon()
        {//���繫��:��, �ش�X����:��
            foreach (GameObject w in weapons)
            {
                w.SetActive(false);
            }
            weapons[currentWeapon].SetActive(true);
        }
    }//update

    public void Die()//������ ���ӸŴ������� �˷�����: ���μ���- �� ����- �ִ����- �׸Ŵ��� �˸�
    {
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("TellmeDied", 1.0f);
    }
    void TellmeDied()
    {
        GameManager.instance.GameOverScene(); //�׿��� ������ �ѱ�
    }
}
