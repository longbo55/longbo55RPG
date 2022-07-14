using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    Text text;
    public float speed;
    public float direction;
    private void Awake()
    {

        text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        StartCoroutine(Move());
        StartCoroutine(FadeOut());
    }

    // 텍스트 이동
    IEnumerator Move() {
        float nowSpeed=speed;
        while (gameObject.activeSelf)
        {
            transform.Translate(new Vector3(Mathf.Sign(-direction) * 20 * Time.unscaledDeltaTime, nowSpeed * Time.unscaledDeltaTime, 0));
            nowSpeed--;
            yield return null;
        }
    }

    //페이드아웃
    IEnumerator FadeOut()
    {
        Color color = text.color;
        color.a = 1;
        while (color.a > 0.0f)
        {
            color.a -= 0.05f;
            text.color = color;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        //페이드아웃 완료시 비활성화
        gameObject.SetActive(false);
    }
}
