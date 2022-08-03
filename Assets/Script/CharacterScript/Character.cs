using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //생존여부
   public bool isAlive;

    //능력치
   public int hp;
   public float speed;

    //공격
   public GameObject attackPos;        //공격위치
   public GameObject nomalAttack;      //일반공격 프리팹
   
   public int mana;                    //풀링을 얼만큼 할지
   public int characterDamage;
   public float attackSpeed;
   public float attackAnimationSpeed;

    //스킬
   public float skillCost;
   public int characterSkillDamage;
   public GameObject skillAttack;

    //감지
   public float attackRange;
   public float traceRange;
   
    //이동방향
    public Vector3 yourTargetPoint;
}
