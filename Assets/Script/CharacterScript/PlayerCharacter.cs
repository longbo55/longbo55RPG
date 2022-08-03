using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCharacter : Character, Idamage
{
    //하나의 행동을 한 뒤에 1초동안 머무를 함수를 하나 만드는건 어떨까? 공격 -> 머무름(상황판단: 적이없음)-> 이동 ->

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
    public States states=States.Move;
    Animator animator;


    void Awake() {

        ObjectPoolingManager.instance.Generate(nomalAttack, mana, gameObject.tag);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(StateCheck());
        StartCoroutine(Action());
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
    GameObject FindTarget() {
        Collider[] hitCharacter = Physics.OverlapBox(transform.position, new Vector3(traceRange, 5, traceRange), Quaternion.identity, LayerMask.GetMask("Character"));

        if (hitCharacter.Length != 0)
        {
               int index = -1;
               float distanceMin = float.MaxValue;

               for (int i = 0; i < hitCharacter.Length; i++)
               {
                   if (!hitCharacter[i].CompareTag(transform.tag)) 
                   { 
                        if (Vector3.Distance(transform.position, hitCharacter[i].transform.position) < distanceMin )
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
            
            //캐릭터의 방향을 설정
            transform.rotation = Quaternion.LookRotation(hitCharacter[index].transform.position - transform.position);
            return hitCharacter[index].gameObject;
        }
        return null;
    }

    //정지
    void Idle() {
        animator.Play("Idle");
    }

    //추적
    void Trace() {
    
    }

    //이동
    void Move()
    {
        animator.Play("Run");
        //ray를 발사하고 캐릭터나 엄폐물에 닿았을 경우 자신의 z값을 기준으로 대각선이동한다. (z값이 -라면 왼쪽 +라면 오른쪽 대각선으로 이동) <- 이거 빨리 A스타 알고리즘으로 바꾸자.
        if (Physics.Raycast(transform.position, yourTargetPoint, 2, LayerMask.GetMask("Character", "Cover")))
        {
            if (transform.position.z>0) 
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, -1));
                transform.Translate(new Vector3(1, 0, -1) * speed * Time.deltaTime, Space.World);
            } 
            else
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(1, 0, 1));
                transform.Translate(new Vector3(1, 0, 1) * speed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(yourTargetPoint);
            transform.Translate(yourTargetPoint * speed * Time.deltaTime, Space.World);
        }
    }
    //공격
    public void NomalAttack()
    {
        //공격 오브젝트 소환
        // <- 애니메이션의 동작에 맞춰서 오브젝트가 생성되게끔 애니메이션 이벤트에서 작동하게 했음.
        ObjectPoolingManager.instance.ActiveObject(nomalAttack, attackPos.transform.position, transform.rotation, characterDamage);
        animator.speed = attackAnimationSpeed; 
        animator.Play("Attack");
    }
    //스킬공격
    /*
    public void SkillAttack() {
        if (GameManager.instance.skillPoint > skillCost) {
        GameManager.instance.skillPoint -= skillCost;
        states = States.SkillAttack;
        ActiveSkillAttackObject();
        animator.Play("SkillAttack");
        }
    }*/
    //죽음
    public void Death() {
        gameObject.layer = 11;
        animator.Play("Death");
        GameManager.instance.GameOverCheck(gameObject.tag);
    }
    //캐릭터 상태관리
    IEnumerator StateCheck()
    {
        while (isAlive)
        {
            if (states != States.SkillAttack)
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
            }
         yield return null; 
        }
        //죽었을 때
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
                    yield return new WaitForSeconds(2);
                    states= States.Move;
                    break;
                case States.Move:
                    Move();
                    break;
                case States.Trace:
                    Trace();
                    break;
                case States.Idle:
                    Idle();
                    //idle에서는 일단 0.1초 대기
                    yield return new WaitForSeconds(0.1f);
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
