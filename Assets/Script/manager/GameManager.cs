using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    //인게임에 적용되는 모든 UI를 담은 오브젝트
    public GameObject[] inGameUI;
    //남아있는 적의 수
    public int enemyCount;

    //남아있는 플레이어 캐릭터의 수
    public int playerCount;

    //왼쪽으로 이동하여 게임의 시작을 알리는 UI
    public GameObject StageStartUI;

    //게임오버 UI
    public GameObject missionSuccessUI;
    public GameObject missionFailUI;

    //플레이어 스킬 포인트
    public GameObject skillUI;
    public float skillPoint=0;

    //캐릭터 스폰 장소
    public GameObject[] PlayerSpawnPoint;
    //플레이어 캐릭터
    public GameObject[] PlayerCharacter;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {   
        Time.timeScale = 1;
        playerCount = PlayerCharacter.Length;
        //게임의 시작을 알리는 UI를 활성화함.
        StageStartUI.SetActive(true);

       //캐릭터의 스킬을  UI와 연동해야하는데 그럴려면 저장기능부터 만들어야 하지만 아직 안만들었기에 일단 비활성화해둠.
       // PlayerCharacterLoad();
    }

    private void FixedUpdate()
    {
        if (skillPoint<=10) {
            skillPoint += Time.deltaTime;
            //Math.Truncate() <=소수점을 전부 버린다.
            UIManager.instance.skillPointText.text = Math.Truncate(skillPoint).ToString();
        }
    }

    //플레이어 캐릭터 생성 // 메인메뉴에서 설정한대로 캐릭터 정보를 가져와서 해당 씬에 생성해야됌.
    public void PlayerCharacterLoad() {
        for (int i = 0; i < PlayerCharacter.Length; i++)
        {
            Instantiate(PlayerCharacter[i], PlayerSpawnPoint[i].transform.position,Quaternion.identity);
        }
    }

    //시작 알림 UI를 비활성화하고 플레이어의 스킬UI를 활성화함.
    public void PlayerSkillUIActive()
    {
        StageStartUI.SetActive(false);
        for (int i = 0; i < inGameUI.Length; i++)
        {
            inGameUI[i].SetActive(true);
        }
        UIManager.instance.enemyCountText.text = enemyCount.ToString();
    }

    public void GameOverCheck() {
        UIManager.instance.enemyCountText.text = enemyCount.ToString();
        if (enemyCount<=0)
        {
            StartCoroutine(MissionSuccess());
            StartCoroutine(SlowTime());
        }

        if (playerCount<=0 && enemyCount>0) { 
            missionFailUI.SetActive(true);
            StageStartUI.SetActive(false);
        }
    }
    public void TimeTwiceButton() {
        if (Time.timeScale == 1)  
        {
            Time.timeScale = 2;
            Debug.Log(Time.timeScale);
        } 
        else
        {
            Time.timeScale = 1;
            Debug.Log(Time.timeScale);
        }
    }
    IEnumerator SlowTime() {
        float tempTimeScale = 1;
        while (tempTimeScale != 0)
        {
            Time.timeScale = tempTimeScale;
            tempTimeScale -= Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MissionSuccess() {
    
        yield return new WaitForSecondsRealtime(2);

        missionSuccessUI.SetActive(true); 
        for (int i = 0; i < inGameUI.Length; i++)
        {
            inGameUI[i].SetActive(false);
        }
    }
}
