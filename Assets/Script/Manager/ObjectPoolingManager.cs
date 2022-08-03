using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager instance { get; private set; }

    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        instance = this;
    }
    public void Generate(GameObject prefab, int size, string tag) {
        if (poolDictionary.ContainsKey(prefab.name)) { return; }
        Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.tag = tag;
                //이름뒤에 클론이 뜨는것을 막기위해 이름 수정.
                obj.name = prefab.name;
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

        poolDictionary.Add(prefab.name, objectPool);
    }
    //공격프리팹 활성화
    public GameObject ActiveObject(GameObject prefab, Vector3 position,  Quaternion rotation, int damage)
    {
        if (!poolDictionary.ContainsKey(prefab.name)) { return null; }
     

        GameObject attackObject = poolDictionary[prefab.name].Dequeue();

        //데미지 입력
        attackObject.GetComponent<AttackPrefab>().attackPrefabDamage = damage;
        
        //위치를 초기화
        attackObject.transform.position =position;
        attackObject.transform.rotation =rotation;

        attackObject.SetActive(true);  
        //만약 전부 쓰고있으면..
        if (poolDictionary[prefab.name].Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(attackObject);
                obj.name = attackObject.name;
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                poolDictionary[attackObject.name].Enqueue(obj);
            }
        }
        return attackObject;
    }
}
