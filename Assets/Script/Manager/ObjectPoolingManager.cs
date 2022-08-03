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
                //�̸��ڿ� Ŭ���� �ߴ°��� �������� �̸� ����.
                obj.name = prefab.name;
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

        poolDictionary.Add(prefab.name, objectPool);
    }
    //���������� Ȱ��ȭ
    public GameObject ActiveObject(GameObject prefab, Vector3 position,  Quaternion rotation, int damage)
    {
        if (!poolDictionary.ContainsKey(prefab.name)) { return null; }
     

        GameObject attackObject = poolDictionary[prefab.name].Dequeue();

        //������ �Է�
        attackObject.GetComponent<AttackPrefab>().attackPrefabDamage = damage;
        
        //��ġ�� �ʱ�ȭ
        attackObject.transform.position =position;
        attackObject.transform.rotation =rotation;

        attackObject.SetActive(true);  
        //���� ���� ����������..
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
