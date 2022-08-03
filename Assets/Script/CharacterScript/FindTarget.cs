using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    
    public GameObject Find(GameObject thisObject)
    {
         int indexI = -1;
         float distanceMin = float.MaxValue;

         //LongLength : 모든 차원에 있는 배열의 수를 가져온다.
         for (int i = 0; i < SpawnManager.instance.enemyArray.LongLength; i++)
         {
                 if (Vector3.Distance(thisObject.transform.position, SpawnManager.instance.enemyArray[SpawnManager.instance.wave].Array[i].transform.position) < distanceMin)
                 {
                     distanceMin = Vector3.Distance(thisObject.transform.position, SpawnManager.instance.enemyArray[SpawnManager.instance.wave].Array[i].transform.position);
                     indexI = i;
                 }
         }
         if (indexI == -1)
         {
             return null;
         }
         return SpawnManager.instance.enemyArray[SpawnManager.instance.wave].Array[indexI];
    }
}
