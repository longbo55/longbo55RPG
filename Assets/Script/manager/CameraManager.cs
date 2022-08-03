using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
    public float speed = 0;
    float distance = 0;
    Camera mainCamera;
    Camera subCamera;
    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        subCamera = GameObject.FindGameObjectWithTag("SubCamera").GetComponent<Camera>();
        StartCoroutine(Move());
        StartCoroutine(Zoom());
    }
    IEnumerator Move()
    {
        while (true) {
            Collider[] hitCharacter = Physics.OverlapBox(transform.position, new Vector3(10, 5, 12.5f), Quaternion.identity, LayerMask.GetMask("Character"));
           if (hitCharacter.Length != 0)
           {
                 int left = 0;
                 int right = 0;
                 float distanceMax = float.MinValue;

                  for (int i = 0; i < hitCharacter.Length; i++)
                  {
                      for (int j = 1; j < hitCharacter.Length; j++)
                      {
                          if (Vector3.Distance(hitCharacter[i].transform.position, hitCharacter[j].transform.position) > distanceMax)
                          {
                              distanceMax = Vector3.Distance(hitCharacter[i].transform.position, hitCharacter[j].transform.position);
                              distance = distanceMax;
                              left = i;
                              right = j;
                          }
                      }
                  }
            transform.position = Vector3.Lerp(transform.position, new Vector3((hitCharacter[left].transform.position.x + hitCharacter[right].transform.position.x) / 2,
                                                                               transform.position.y,transform.position.z), speed * Time.deltaTime);
           }
            yield return null;
        }
    }
    IEnumerator Zoom()
    {
        while (true)
        {
            //실시간으로 distance값의 1/10과 카메라의 최소사이즈에 해당하는 값(3.5f)을 해당 변수에 넣음
            float cameraSize = 3.5f + (distance / 10);
            //cameraSize 변수와 현재의 사이즈의 차이가 0.2f이하일때까지 반복한다
            while (Mathf.Abs(cameraSize- mainCamera.orthographicSize)>0.2f)
            {
                if (cameraSize > mainCamera.orthographicSize)
                {
                    mainCamera.orthographicSize += Time.deltaTime;
                }
                else
                {
                    mainCamera.orthographicSize -= Time.deltaTime;
                }

                subCamera.orthographicSize = mainCamera.orthographicSize;
                yield return null;
            }
            yield return null;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(20, 10, 25));
    }

}