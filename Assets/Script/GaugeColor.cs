using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeColor : MonoBehaviour
{
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.color = Color.HSVToRGB(image.fillAmount / 3,1,1);
                      //�÷�����-HSV:����,ä��,��,����(Hue):0�϶� ����,~���� ������ 1��, �ʷ��� 1/3����
    }
}
