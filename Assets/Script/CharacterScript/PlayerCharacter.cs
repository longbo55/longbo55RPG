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

    private void Awake()
    {
        nomalAttackPool = new GameObject[mana];
        skillAttackPool = new GameObject[2];
        Generate();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(StateCheck());
        StartCoroutine(Action());
    }
    //공격프리팹 생성
    void Generate()
    {
        //일반공격 프리팹 생성
        for (int i = 0; i < nomalAttackPool.Length; i++)
        {
            nomalAttackPool[i] = Instantiate(nomalAttack);
            nomalAttackPool[i].GetComponent<AttackPrefab>().tagName = "Enemy";
            nomalAttackPool[i].SetActive(false);
        }

         //스킬공격 프리팹 생성
        for (int j = 0; j < skillAttackPool.Length; j++)
        {
          skillAttackPool[j] = Instantiate(skillAttack);
          skillAttackPool[j].GetComponent<SkillAttackPrefeb>().tagName = "Enemy";
          skillAttackPool[j].SetActive(false);
        }
    }
    //공격프리팹 활성화
    public GameObject ActiveNomalAttackObject()
    {
        Collider target = FindTarget();
        for (int i = 0; i < nomalAttackPool.Length; i++)
        {
            // mana변수만큼 카운트가 올라가면 재장전 후 공격할 수 있게끔 할까 고민중.
            if (!nomalAttackPool[i].activeSelf)
            {
                if(target!=null)
                {
                    transform.rotation = Quaternion.LookRotation(target.transform.position-transform.position);
                    nomalAttackPool[i].GetComponent<AttackPrefab>().targetPoint = target.transform.position;
                }
                nomalAttackPool[i].GetComponent<AttackPrefab>().attackPrefabDamage = characterDamage;
                nomalAttackPool[i].transform.position = AttackPos.transform.position;
                nomalAttackPool[i].SetActive(true);
                return nomalAttackPool[i];
            }
        }
        return null;
    }
    //스킬공격 프리팹 활성화
    public GameObject ActiveSkillAttackObject()
    {
        Collider target = FindTarget();
        for (int i = 0; i < skillAttackPool.Length; i++)
        {
            // mana변수만큼 카운트가 올라가면 재장전 후 공격할 수 있게끔 할까 고민중.
            if (!skillAttackPool[i].activeSelf)
            {
                if (target != null)
                {
                    transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    skillAttackPool[i].GetComponent<SkillAttackPrefeb>().targetPoint = target.transform.position;
                }
                skillAttackPool[i].GetComponent<SkillAttackPrefeb>().skillAttackPrefabDamage  = characterSkillDamage;
                skillAttackPool[i].transform.position = new Vector3(transform.position.x, 10, transform.position.z);
                skillAttackPool[i].SetActive(true);
                return skillAttackPool[i];
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
    Collider FindTarget() {
        //이거layermast,getmask(enemy)말고 compartag()로 해서 아군과 적군을 나누는게 좋지 않을까? 
        Collider[] hitCharacter = Physics.OverlapBox(transform.position, new Vector3(traceRange, 5, traceRange), Quaternion.identity, LayerMask.GetMask("Character"));

        if (hitCharacter.Length != 0)
        {
               int index = -1;
               float distanceMin = float.MaxValue;

               for (int i = 0; i < hitCharacter.Length; i++)
               {
                   if (hitCharacter[i].CompareTag("Enemy")) { 
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
          return hitCharacter[index];
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
        //if(만약 경로에 엄폐물이 있으면){ 넘어가는 애니메이션 재생 }
        animator.Play("Run");
        //ray를 발사하고 캐릭터나 엄폐물에 닿았을 경우 자신의 z값을 기준으로 대각선이동한다. (z값이 -라면 왼쪽 +라면 오른쪽 대각선으로 이동)
        if (Physics.Raycast(transform.position, Vector3.right, 2, LayerMask.GetMask("Character", "Cover")))
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
            transform.rotation = Quaternion.LookRotation(Vector3.right);
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
    }
    //공격
    public void NomalAttack()
    {
        //공격 오브젝트 소환
        //MakeGameOBJ(); <- 애니메이션의 동작에 맞춰서 오브젝트가 생성되게끔 애니메이션 이벤트에서 작동하게 했음.
        animator.speed = AttackAnimationSpeed; 
        animator.Play("Attack");
    }
    //스킬공격
    public void SkillAttack() {
        if (GameManager.instance.skillPoint > skillCost) {
        GameManager.instance.skillPoint -= skillCost;
        states = States.SkillAttack;
        ActiveSkillAttackObject();
        animator.Play("SkillAttack");
        }
    }
    //죽음
    public void Death() {
        gameObject.layer = 11;
        animator.Play("Death");
        GameManager.instance.playerCount--;
        GameManager.instance.GameOverCheck();
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
         yield return new WaitForSeconds(0.1f); 
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
