using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStartText : MonoBehaviour
{
    RectTransform rect;
    private void Start()
    {
       rect = GetComponent<RectTransform>();
        //시작 위치 초기화
       rect.anchoredPosition =new Vector2 (Screen.width,0);
    }
    void Update()
    {   //화면 가운데에서 잠시 멈췄다가 계속 진행함
        if (rect.anchoredPosition.x < 10f)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(-Screen.width, 0), 2 * Time.deltaTime);
        }
        else {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(0, 0), 2.3f * Time.deltaTime);
        }
    }
}
