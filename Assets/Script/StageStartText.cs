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
        StartCoroutine(Move());
    }
    IEnumerator Move() {
        while (rect.anchoredPosition.x > (-Screen.width * 2)) {
            //화면 가운데에서 잠시 멈췄다가 계속 진행함
            if (rect.anchoredPosition.x <= 10)
            {
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2((-Screen.width * 3), 0), 2 * Time.deltaTime);
            }
            else
            {
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(0, 0), 2.3f * Time.deltaTime);
            }
            yield return null;
        }
        GameManager.instance.InGameUIActive();
        transform.parent.gameObject.SetActive(false);
    }
}
