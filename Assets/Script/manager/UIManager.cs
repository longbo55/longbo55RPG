using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    public Text skillPointText;
    public Text enemyCountText;

    public GameObject damageText;
    public List<GameObject> damageTextList = new List<GameObject>();
    void Awake()
    {
        instance = this;
        // 초기 갯수 20개 생성
        Generate(20);
    }
    void Generate(int addNumber) {
        for (int i = 0; i < addNumber; i++)
        {
           //텍스트오브젝트 생성하고 리스트에 추가
           GameObject temp = Instantiate(damageText, transform.position, Quaternion.identity);
           //생성한 오브젝트의 부모를 UICnvas로 설정
           temp.transform.SetParent(GameObject.Find("UICanvas").transform);
           //비활성화
           temp.SetActive(false);
           //마지막으로 리스트에 추가
           damageTextList.Add(temp);
        }
    }
    public void DamageUISetActive(GameObject target,float _direction ,float damage)
    {
        while (true) { 
             for (int i = 0; i < damageTextList.Count; i++)
             {
                 if (!damageTextList[i].activeSelf)
                 {
                     damageTextList[i].GetComponent<DamageText>().direction = _direction;
                     damageTextList[i].transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
                     damageTextList[i].GetComponent<Text>().text = damage.ToString();
                     damageTextList[i].SetActive(true);
                     return;
                 }
             }
            //만약 for문이 끝났는데도 함수가 끝나지 않으면 Generate함수를 통해 damageText를 추가 생성
            Generate(3);
        }
    }
}
