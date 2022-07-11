using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectArray {

    public GameObject[] Array;
}
public class SpawnManager : MonoBehaviour
{
    public GameObject[] detectPos;
    public GameObject[] spawnPoint;
    public int wave = 0;

    public GameObjectArray[] enemyArray;
    
    public RaycastHit hit;
    
    private void Start()
    {
        StartCoroutine(detection());
        for (int i = 0; i < enemyArray.Length; i++)
        {
            GameManager.instance.enemyCount += enemyArray[i].Array.Length;
        }
    }
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

    void SpawnEnemy() {
            for (int i = 0; i < enemyArray[wave].Array.Length; i++)
            {
            //일단은 랜덤으로 소환하게 해놨는데 지정된 위치에 소환되게 할까 고민중 / 지정된 위치에 배치할까..
                float randomX = Random.Range(-5.0f,5.0f);
                float randomZ = Random.Range(-5.0f, 5.0f);
                Instantiate(enemyArray[wave].Array[i], new Vector3(spawnPoint[wave].transform.position.x + randomX, 0, spawnPoint[wave].transform.position.z + randomZ), Quaternion.identity);
            }
        wave++;
    }
}
