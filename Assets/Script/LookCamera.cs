using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour //HP바가 메인캠을 향하도록!
{
   
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
        //LookAt:현위치에서 메인캠을 앞쪽으로 바라보는 벡터 더하게 함
    }
}
