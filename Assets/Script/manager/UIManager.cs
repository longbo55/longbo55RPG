using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    public Text skillPointText;
    public Text enemyCountText;

    public GameObject damageText;
    List<GameObject> damageTextList = new List<GameObject>();
   protected override void OnAwake()
    {
        Generate();
    }
    void Generate() {
        // 초기 갯수 20개 생성
        for (int i = 0; i < 20; i++)
        {
            //텍스트오브젝트 생성하고 리스트에 추가
            damageTextList.Add(Instantiate(damageText));
            //생성한 오브젝트의 부모를 UICnvas로 설정
            damageTextList[i].transform.SetParent(GameObject.Find("UICanvas").transform);
            //마지막으로 비활성화
            damageTextList[i].SetActive(false);
        }
    }
    public void DamageUISetActive(GameObject target,float _direction ,float damage)
    {
        //활성화를 할 수 있는 오브젝트가 없을 때 리스트에 추가하는 기능을 구현해야됌.
        for (int i = 0; i < damageTextList.Count; i++)
        {
            if (!damageTextList[i].activeSelf)
            {
                damageTextList[i].GetComponent<DamageText>().direction = _direction; // 피격오브젝트의 방향을 계산해서 넣어야됌 -1은 임시로 넣어둔것.
                damageTextList[i].transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
                damageTextList[i].GetComponent<Text>().text = damage.ToString();
                damageTextList[i].SetActive(true);
                break;
            }
        }
    }
}
