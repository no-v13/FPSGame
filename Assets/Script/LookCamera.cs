using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour //HP�ٰ� ����ķ�� ���ϵ���!
{
   
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        //LookAt:����ġ���� ����ķ�� �������� �ٶ󺸴� ���� ���ϰ� ��
    }
}
