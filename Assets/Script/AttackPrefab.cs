using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPrefab : MonoBehaviour
{
    public int attackPrefabDamage;
    public float speed;

    public GameObject attackPrefab;
    public ParticleSystem ExplosionParticle;
 
    public Vector3 targetPoint;
    public string tagName;

    public RaycastHit hit;
    public Vector3 previousPos;
    private void OnEnable()
    {
        //오브젝트가 활정화된다면 이전 위치를 현재위치로 초기화해줌
        previousPos = transform.position;
        
        //공격프리팹오브젝트 활성화
        attackPrefab.SetActive(true);

        //targetPoint에서 y값을 0으로 만들기위한 작업
        targetPoint = targetPoint - transform.position;
        targetPoint = new Vector3(targetPoint.x, 0, targetPoint.z);
        
        //코루틴 시작
        StartCoroutine(Move()); 
        StartCoroutine(SetActiveFalse());
    }
    IEnumerator Move()
    {
        while (true)
        {

            //타겟의 위치방향으로 이동한다.
            transform.Translate(targetPoint * speed * Time.deltaTime, Space.World);
            Debug.DrawRay(transform.position, (previousPos - transform.position), Color.green,0.1f,false);
            //프레임만큼 이동한 거리내에 오브젝트가 있나 확인
            if (Physics.Raycast(transform.position, (previousPos - transform.position), out hit, Vector3.Distance(previousPos, transform.position),LayerMask.GetMask("Character")))
            {
                //오브젝트의 태그가 tagName변수와 같다면
                if (hit.collider.CompareTag(tagName))
                {
                    //감지된 오브젝트의 Idamage컴포넌트를 받아와서 Hit함수를 실행한다.
                    Idamage onDamage = hit.collider.GetComponent<Idamage>();
                    onDamage.Hit(attackPrefabDamage);

                    UIManager.instance.DamageUISetActive(gameObject,previousPos.x-transform.position.x, attackPrefabDamage);
                    //이 오브젝트의 위치는 감지된 에너미 오브젝트의 위치로, 높이는 그대로 설정한다.(이펙트가 터지는 위치를 설정하는것임)
                    transform.position = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                    StartCoroutine(OnDamage());
                    break;
                }
            }
            //위치를 업데이트한다.
            previousPos = transform.position;
            yield return null;
        }
    }
    IEnumerator OnDamage()
    {
        //공격프리팹오브젝트 비활성화
        attackPrefab.SetActive(false);
        //폭발파티클 실행
        ExplosionParticle.Play();

        //1초뒤에 비활성화
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
    IEnumerator SetActiveFalse() {
        //안맞았을경우를 대비해 5초뒤에 자동 비활성화
        yield return new WaitForSeconds(5);
        StartCoroutine(OnDamage());
    }
}/* 
     IEnumerator Move()
    {
        while (true)
        {
           
            /* Collider[] hitCharacter = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, LayerMask.GetMask(layerName));
             if (hitCharacter.Length != 0)
             {
if (hit.collider.CompareTag(tagName))
                {
                    Idamage onDamage = hit.collider.GetComponent<Idamage>();
                    onDamage.Hit(attackPrefabDamage, hit.collider);
                    transform.position = previousPos;
                    StartCoroutine(OnDamage());
                    break;
                }
            }

            previousPos = transform.position;
            yield return new WaitForSeconds(0.01f);
        }
    }
*/