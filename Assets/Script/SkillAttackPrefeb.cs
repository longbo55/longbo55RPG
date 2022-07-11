using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackPrefeb : MonoBehaviour
{

    public int skillAttackPrefabDamage;
    public float speed;
    public float radius;

    public GameObject skillAttackPrefab;
    public ParticleSystem ExplosionParticle;

    public Vector3 targetPoint;
    public string tagName;

    private void OnEnable()
    {
        //공격프리팹오브젝트 활성화
        skillAttackPrefab.SetActive(true);

        //코루틴 시작
        StartCoroutine(Move());
        StartCoroutine(SetActiveFalse());
    }
    IEnumerator Move()
    {
        targetPoint -= transform.position;
        while (true)
        {

            //타겟의 위치방향으로 이동한다.
            transform.Translate(targetPoint * speed * Time.deltaTime);

            //높이가 0.1f보다 낮다면
            if (transform.position.y <= 0.1f)
            {
                Collider[] hit = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Character"));
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].CompareTag(tagName))
                    {
                        //감지된 오브젝트의 Idamage컴포넌트를 받아와서 Hit함수를 실행한다.
                        Idamage onDamage = hit[i].GetComponent<Idamage>();
                        onDamage.Hit(skillAttackPrefabDamage);
                        UIManager.instance.DamageUISetActive(gameObject, targetPoint.x - transform.position.x, skillAttackPrefabDamage);
                    }
                }
                targetPoint = Vector3.zero;
                StartCoroutine(OnDamage());
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator OnDamage()
    {
        //공격프리팹오브젝트 비활성화
        skillAttackPrefab.SetActive(false);
        //폭발파티클 실행
        ExplosionParticle.Play();

        //1초뒤에 비활성화
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
    IEnumerator SetActiveFalse()
    {
        //안맞았을경우를 대비해 5초뒤에 자동 비활성화
        yield return new WaitForSeconds(5);
        StartCoroutine(OnDamage());
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}