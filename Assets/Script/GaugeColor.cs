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
                      //컬러결정-HSV:색상,채도,명도,색상값(Hue):0일때 빨강,~보라 갈수록 1로, 초록은 1/3지점
    }
}
