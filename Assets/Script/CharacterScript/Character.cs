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
   public GameObject AttackPos;        //공격위치
   public GameObject nomalAttack;      //일반공격프리팹
   public GameObject[] nomalAttackPool;//일반공격 풀링을 위한 배열
   public int mana;                    //풀링을 얼만큼 할지
   public int characterDamage;
   public float attackSpeed;
   public float AttackAnimationSpeed;

    //스킬
   public float skillCost;
   public int characterSkillDamage;
   public GameObject skillAttack;
   public GameObject[] skillAttackPool;
    //감지
   public float attackRange;
   public float traceRange;
   
}
