using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, Idamage
{  
    //상태
    public enum States
    {
        Idle,
        Move,
        Trace,
        NomalAttack,
        SkillAttack,
        Death,
    }
    public States states = States.Move;
    Animator animator;

    private void Awake()
    {
        nomalAttackPool = new GameObject[mana];
        Generate();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(StateCheck());
        StartCoroutine(Action());
    }
    void Generate()
    {
        for (int i = 0; i < nomalAttackPool.Length; i++)
        {
            nomalAttackPool[i] = Instantiate(nomalAttack);
            nomalAttackPool[i].GetComponent<AttackPrefab>().tagName = "Player";
            nomalAttackPool[i].SetActive(false);
        }
    }
    //이제 이 함수만 호출하면 언제든 총알을 꺼내 쓸 수 있음
    public GameObject ActiveNomalAttackObject()
    {
        for (int i = 0; i < nomalAttackPool.Length; i++)
        {
            //일단은 단순히 꺼져있으면 다시 키는 방식으로 했는데 mana변수만큼 카운트가 올라가면 재장전 후 공격할 수 있게끔 할까 고민중.
            if (!nomalAttackPool[i].activeSelf)
            {
                if (FindTarget() != null)
                {
                    transform.rotation = Quaternion.LookRotation(FindTarget().transform.position - transform.position);
                    nomalAttackPool[i].GetComponent<AttackPrefab>().targetPoint = FindTarget().transform.position;
                }
                nomalAttackPool[i].GetComponent<AttackPrefab>().attackPrefabDamage = characterDamage;
                nomalAttackPool[i].transform.position = AttackPos.transform.position;
                nomalAttackPool[i].SetActive(true);
                return nomalAttackPool[i];
            }
        }
        return null;
    }
  
    //피격
    public void Hit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            isAlive = false;
        }
    }
    Collider FindTarget()
    {
        Collider[] hitCharacter = Physics.OverlapBox(transform.position, new Vector3(traceRange, 5, traceRange), Quaternion.identity, LayerMask.GetMask("Character"));

        if (hitCharacter.Length != 0)
        {
            int index = -1;
            float distanceMin = float.MaxValue;

            for (int i = 0; i < hitCharacter.Length; i++)
            {
                if (hitCharacter[i].CompareTag("Player"))
                {
                    if (Vector3.Distance(transform.position, hitCharacter[i].transform.position) < distanceMin)
                    {
                        distanceMin = Vector3.Distance(transform.position, hitCharacter[i].transform.position);
                        index = i;
                    }
                }
            }
            if (index == -1)
            {
                return null;
            }
            return hitCharacter[index];
        }
        return null;
    }

    void Idle()
    {
        animator.Play("Idle");
        transform.position = Vector3.zero;
    }
    void Trace()
    {

    }
 
    //이동
    void Move()
    {
        //if(만약 경로에 엄폐물이 있으면){ 넘어가는 애니메이션 재생 }
        animator.Play("Run");
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(Vector3.left);
    }
    //공격
    public void NomalAttack()
    {
        animator.speed = AttackAnimationSpeed;
        animator.Play("Attack");
    }
    public void SkillAttack()
    {

    }
    public void Death() {
        gameObject.layer = 11;
        animator.Play("Death");
        GameManager.instance.enemyCount--;
        GameManager.instance.GameOverCheck();
    }
    //캐릭터 상태관리
    IEnumerator StateCheck()
    {
        while (isAlive)
        {
            if (FindTarget() != null)
            {
                //공격 사거리에 닿지않으면 추적 닿으면 공격
                if (Vector3.Distance(transform.position, FindTarget().gameObject.transform.position) > attackRange)
                {
                    states = States.Move;//아직 trace기능이 없는 관계로 일단은 move로 설정함.
                }
                else
                {
                    states = States.NomalAttack;
                }
            }
            else
            {
                states = States.Move;
            }
            yield return new WaitForSeconds(0.1f);
        }
        states = States.Death;
    }
    //캐릭터 행동
    IEnumerator Action()
    {
        while (true)
        {
            switch (states)
            {
                case States.NomalAttack:
                    NomalAttack();
                    yield return new WaitForSeconds(attackSpeed);
                    animator.speed = 1;
                    break;
                case States.SkillAttack:
                    SkillAttack();
                    break;
                case States.Move:
                    Move();
                    break;
                case States.Trace:
                    Trace();
                    break;
                case States.Idle:
                    Idle();
                    //idle에서는 일단 1초 대기
                    yield return new WaitForSeconds(1);
                    break;
                case States.Death:
                    Death();
                    yield return new WaitForSeconds(4);
                    gameObject.SetActive(false);
                    break;
            }
            yield return null;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(traceRange * 2, 5, traceRange * 2));
    }
}
