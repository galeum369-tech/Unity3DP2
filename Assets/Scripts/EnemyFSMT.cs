using System.Collections;
using UnityEngine;
using UnityEngine.AI;  //네비메시에이전트 사용하려면 반드시 필요

public class EnemyFSMT : MonoBehaviour
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

    Vector3 startPoint;                 //몬스터 시작 위치

    Transform player;                   //플레이어를 찾기 위해(코드로 처리하기)
    CharacterController cc;             //몬스터 이동을 위한 컴포넌트

    //몬스터 일반변수
    int hp = 100;
    int att = 5;
    float speed = 5f;

    //공격 딜레이
    float attTime = 2f;                 //2초에 한번 공격
    float timer = 0;                    //타이머

    //유니티에서 길찾기 알고리즘이 적용이된 네비게이션을 사용하려면 반드시 UnityEngine.Ai 추가해야함
    //네비게이션은 맵전체를 베이크해서 에이전트가 어느 위치에 있던 미리 계산된 정보를 사용한다
    NavMeshAgent agent;
    //주의
    //충돌은 콜리더로 하고
    //이동만 네비메시에이전트를 사용해야
    //enemyFSM을 제대로 사용할 수 있다
    //따라서 시작할때 네비메쉬 에이전트는 꺼줘야한다(움직일 때만 사용하는 느낌/이유:충돌오류나거나 여러문제생길 수 잇음)


    //애니메이션을 제어하기 위한 애니메이션 컴포넌트
    //Animator anim;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //상태 초기화
        state = EnemyState.Idle;
        //시작지점 저장
        startPoint = transform.position; 
        //플레이어 트렌스폼
        player = GameObject.Find("Player").transform;
        //캐릭터 컨트롤러
        cc = GetComponent<CharacterController>();

        //네비메쉬에이전트
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
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
        
        //탐지 범위 안에 들어옴
        if (Vector3.Distance(transform.position, player.position) < findRange)
        {
            state = EnemyState.Move;
            print("상태전환 : Idle -> Move");
        }

    }
    //이동상태
    void Move()
    {
        //시작시 끄고 이동상태가 아닐때 꺼야함(네비메쉬에이전트)
        if (!agent.enabled) agent.enabled = true;



        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이어를 추격하더라도 처음위치에서 일정범위 넘어가면 리턴상태로 변경
        //- 플레이어처럼 캐릭터 컨트롤러 이용하기(cc.Move대신 cc.SimpleMove이용하자)
        //- 공격범위 2ㅁ;타
        //- 상태변경
        //- 상태전환 출력
        //- 애니메이션
        
        //이동중 이동할 수 있는 최대범위를 벗어났을 때
        if (Vector3.Distance(transform.position, startPoint) > moveRange)
        {
            state = EnemyState.Return;
            
        }
        //리턴상태가 아니면 플레이어 추격해야 한다
        else if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            //플레이어를 향한 이동처리
            //네비메시에이전트가 회전처리부터 이동까지 전부다 처리
            agent.SetDestination(player.position);


            //플레이어를 추격
            //이동방향(벡터 뺄셈)
           // Vector3 dir = (player.position - transform.position).normalized;
            //dir.Nomalized();

            //몬스터가 자신이 서있는 위치에서 회전값없이 백스탭오르 쫒아온다
            //몬스터가 타겟을 바라보도록 하자
            //1
            //transform.position = dir;
            //2
            //transform.LookAt(dir);

            //좀더 자연스립게 희전처리를 하자
            //transform.forward = Vector3.Lerp(transform.forward, dir, 5 * Time.deltaTime);
            //여기에 문제가 한가지 있는데
            //타겟과 몬스터가 일직선상일 경우 좌우 어디로 회전해야할지 못 정하여
            //그냥 백덤블링으로 회전을 하게 된다

            //최종적으로 자연스러운 회전처리를 하려면 결국 쿼터니온을 사용해야한다
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);

            //cc.SimpleMove(dir * speed);
        }
        //공격범위 안에 들어옴
        else
        {
            state = EnemyState.Attack;
            print("상태전환 : Move -> Attack");
        }

    }
    //공격상태
    void Attack()
    {
        //에이전트 오프
        agent.enabled = false;

        //1.플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2.플레이어가 공격범위를 벗어났다면 이동상태(추격)로 변경
        //-공격범위 2미터
        //-상태변경
        //-상태변환 출력
        //-애니메이션
        
        //공격범위 안에 들어옴
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            //공격할때 거리로만 처리되다보니 엉뚱한곳을 공격할 수 도 있다
            transform.LookAt(player.position)   ;

            //일정 시간마다 플레이어 공격
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트 컴포넌트를 가져와서 데미지를 주면 된다
                //player.GetComponent<PlayerMove>().hitDamage(att);

                //타이머 초기화
                timer = 0;

                //애니메이션 공격

            }
        }
        else
        {
            //재추격
            state = EnemyState.Move;
            print("상태전환 : Attack -> Move");
            //애니메이션 무브
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

        //시작위치까지 도착하지 않았을 떄는 이동
        //도착하면 대기상태로 변경
        if (Vector3.Distance(transform.position, startPoint) > 0.1)
        {
            agent.enabled = true;
            //이동처리
            agent.SetDestination(startPoint);

        }
        else
        {
            //위치값을 초기값으로
            transform.position = startPoint;
            transform.rotation = Quaternion.identity;

            //상태변경
            state = EnemyState.Idle;
            print("상태변환 : Return -> Idle");

            agent.enabled = false;
        }
    }

    //플레이어 쪽ㅇ[서 충돌감지를 할 수 있으니 이 함수는 퍼블릭으로 만들자

    public void HitDamage(int value)
    {

        //예외처리
        //피격상태거나,죽은사태일때는 데미지 중접X
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;
        //체력 ㄱ까기
        //몬스어 ㅊ[력이 1이상이면 피격상태
        //0이하면 죽음상태

        hp -= value;
        //몬스터 체력이 1이상이면 피격상태
        if (hp > 0)
        {
            state = EnemyState.Damaged;
            print("상태전환 : Anystate -> Damaged");
            print("HP: " + hp);
            Damaged();
        }
        //0이면 죽은상태
        else
        {
            state = EnemyState.Die;
            print("상태전환 : Anystate -> DIe");
            Die();
        }

    }

    //피격상태(AnyState)
    void Damaged()
    {
        agent.enabled = false;
        //1. 몬스터 체력 1이상
        //2. 다시 이전 상태로 변경
        //- 상태변경
        // 상태전환 출력

        //피격상태를 처리하기 위해서는 간단한 코루틴 사용
        StartCoroutine(DamageProc());
    }

    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1f);
        //현재상태를 이동으로 전환
        state = EnemyState.Move;
        print("상태전환 : Damaged -> Move");
    }

    //죽음상태(Any stae
    void Die()
    {
        //1. 몬스터 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력

        agent.enabled = false;

        //죽음상태 처리를 간단한 코루틴 사용
        StartCoroutine(DieProc());

        //진행중인 모든 코루티은 정지
        //StopAllCoroutines();
        
    }

    IEnumerator DieProc()
    {
        //2초후에 자기자신 제거
        yield return new WaitForSeconds(2f);
        print("죽음");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        //공격가능범위
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //플레이어 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
        //이동가능 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, moveRange);

    }

}
