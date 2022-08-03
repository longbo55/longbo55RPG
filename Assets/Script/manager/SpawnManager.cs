using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectArray 
{   //이 클래스 만들필요없이 오브젝트풀링할때 썼던것 처럼 딕셔너리에 큐넣어서 쓰면 훨씬 좋지 않을까?
    public GameObject[] Array;
}
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance { get; private set;}
    public GameObject[] detectPos;
    public GameObject[] spawnPoint;
    public int wave = 0;

    public GameObjectArray[] enemyArray;
    
    public RaycastHit hit;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        StartCoroutine(detection());
        for (int i = 0; i < enemyArray.Length; i++)
        {
            GameManager.instance.enemyCount += enemyArray[i].Array.Length;
        }
    }

    //탐색
  IEnumerator detection()
    {
        while (wave < enemyArray.Length)
        {        
            if (Physics.Raycast(detectPos[wave].transform.position, detectPos[wave].transform.TransformDirection(Vector3.forward), out hit, 60))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    SpawnEnemy();
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //적 소환
    void SpawnEnemy() {
            for (int i = 0; i < enemyArray[wave].Array.Length; i++)
            {
            //일단은 랜덤으로 소환하게 해놨는데 지정된 위치에 소환되게 할까 고민중..아직은 필요성을 못느낌 / 원한다면 GameObjectArray에 vecter값을 같이 넣어놓고 웨이브 포인트를 중심으로 원하는 위치에 소환되도록 할 수 있음
            float randomX = Random.Range(-5.0f,5.0f);
                float randomZ = Random.Range(-5.0f, 5.0f);
                Instantiate(enemyArray[wave].Array[i], new Vector3(spawnPoint[wave].transform.position.x + randomX, 0, spawnPoint[wave].transform.position.z + randomZ), Quaternion.identity);
            }
        wave++;
    }
}
