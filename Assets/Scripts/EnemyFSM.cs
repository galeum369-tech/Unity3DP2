using JetBrains.Annotations;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    /*
     유한 상태 머신 (FSM)
    =>유한한 수의 상태(state)와 상태들 사이의 전환(transition)을 정의해서 시스템 동작을 하게 하는 것
    당연히 전환이 이루어 지려면 조건(condition)이 필요하다
    이것은 FSM디자인 패턴이라한다
    - 상태 : 간단하게 말하면 행동 (걷기, 달리기, 점프, 공격 죽음 등등)
    - 전환 : 상태에서 상태로 넘어가는 변화
    - 조건 : 전환이 발생하기 위해 필요한 기준 (키입력, HP감소, 특정 아이템 획득 등 다양한 이벤트)
    => 결론은 이미 우리는 애니메이터를 사용하며 한번씩은 겪어 보았다

    => 플레이어 캐릭터 행동 제어
    => 대표적으로 에너미 AI구현
    => 예) 몬스터가 플레이어를 발견하기 전에는 (순찰)상태, 플레이어를 발견하면 (추격)상태
    공격범위안에 들어오면 (공격)상태, HP가 일정 이하로 떨어졌을 때 (도망, 버서크모드)
    */

    //몬스터 상태 이넘문

    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState state;       //몬스터 상태 변수

    public float findRange = 15f;       //플레이어를 찾는 범위
    public float moveRange = 30f;       //시작지점에서 최대 이동가능한 범위
    public float attackRange = 2f;      //공격 가능범위

    //애니메이션을 제어하기 위한 애니메이션 컴포넌트
    //Animator anim;

    public Transform player;

    bool FoundP()
    {
        float dis = Vector3.Distance(transform.position, player.position);
        return dis <= findRange;
    }

    bool AttackP()
    {
        float dis = Vector3.Distance(transform.position, player.position);
        return dis <= attackRange;
    }
    bool TooFarFromOrigin()
    {
        float dist = Vector3.Distance(transform.position, originPos);
        return dist > moveRange;
    }

    CharacterController cc;
    public float moveSpeed = 2f;

    Vector3 originPos;

    public int hp = 100;

    public float attackT = 0;
    public float maxAttackT = 2;

    public float damageT = 0f;
    public float maxDamageT = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //상태 초기화
        state = EnemyState.Idle;

        cc = GetComponent<CharacterController>();
    
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        HitDamage();
    }

    //대기상태
    void Idle()
    {
        //1.플레이어와 일정 범위가 되면 이동상태로 변경(탐지범위)
        //-플레이어 찾기
        //-일정거리 비교(Distance, magnitude, sqrMagnitued 아무거나
        //-상태변경 state = EnemyState.Move;
        //-상태전환 출력 print("Idle -> Move");
        //-애니메이션 SetTrigger("Move");
        if (FoundP())
        {
            state = EnemyState.Move;
            print("Idle -> Move");
        }

    }
    //이동상태
    void Move()
    {
        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위 넘어가면 리턴상태로 변경
        //- 플레이어처럼 캐릭터 컨트롤러 이용하기(cc.Move대신 cc.SimpleMove이용하자)
        //- 공격범위 2ㅁ;타
        //- 상태변경
        //- 상태전환 출력
        //- 애니메이션
        Vector3 dir = (player.position - transform.position).normalized;
        cc.SimpleMove(dir * moveSpeed);
        if (AttackP())
        {
            state = EnemyState.Attack;
            print("Move -> Attack");
        }
        
        if (TooFarFromOrigin())
        {
            state = EnemyState.Return;
            print("Move -> Return");
        }

        if (!FoundP())
        {
            state = EnemyState.Idle;
            print("Move -> Idle");
        }


    }
    //공격상태
    void Attack()
    {
        //1.플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2.플레이어가 공격범위를 벗어났다면 이동상태(추격)로 변경
        //-공격범위 2미터
        //-상태변경
        //-상태변환 출력
        //-애니메이션
        if (AttackP())
        {
            attackT += Time.deltaTime;
            if (attackT >= maxAttackT)
            {
                print("적의 공격!");
                attackT = 0;
            }
        }
        if (!AttackP())
        {
            state = EnemyState.Move;
        }


    }
    //복귀상태
    void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정범위를 벗어나면 다시 돌아옴
        //- 처음 위치에서 일정범위 30미터
        //- 상태변경
        //- 상태변환 출력
        //- 애니메이션
        if (TooFarFromOrigin())
        {
            cc.Move(originPos * moveSpeed * Time.deltaTime);
            if (transform.position == originPos)
            {
                state = EnemyState.Idle;
                print("Return -> Idle");
            }
        }
    }

    //플레이어 쪽ㅇ[서 충돌감지를 할 수 있으니 이 함수는 퍼블릭으로 만들자

    public void HitDamage()
    {
        damageT += Time.deltaTime;

        if (damageT >= maxDamageT)
        {

            state = EnemyState.Damaged;
        }

        //예외처리
        //피격상태거나,죽은사태일때는 데미지 중접X

        //체력 ㄱ까기
        //몬스어 ㅊ[력이 1이상이면 피격상태
        //0이하면 죽음상태
    }

    //피격상태(AnyState)
    void Damaged()
    {
        if (hp > 0)
        {
            hp -= 20;
            state = EnemyState.Idle;
            print("데미지 받음");
        }
        else
        {
            state = EnemyState.Die;
        }


        //1. 몬스터 체력 1이상
        //2. 다시 이전 상태로 변경
        //- 상태변경
        // 상태전환 출력

        //피격상태를 처리하기 위해서는 간단한 코루틴 사용
    }
    //죽음상태(Any stae
    void Die()
    {
        //1. 몬스터 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력

        if (hp <= 0)
        {
            print("죽음");
            Destroy(gameObject, 3f);
        }

        //진행중인 모든 코루티은 정지
        StopAllCoroutines();
        //
    }

    

}


