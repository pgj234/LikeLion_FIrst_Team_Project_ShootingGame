using System.Collections;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// 플레이어 캐릭터의 기능을 관리하는 클래스
/// 이동, 총알 발사, 무기 업그레이드, 플레이어 복제 등의 기능을 담당합니다.
/// </summary>
public class Player : MonoBehaviour
{
    // ===== 외부 참조 및 인스펙터에서 설정하는 변수들 =====
    [SerializeField] GameObject gameOverUIObj;      // 게임오버 UI 오브젝트

    [Space(10)] // 인스펙터에서 10픽셀 공간 추가
    [Tooltip("0:베이스, 1:레이저, 2:나선탄, 3:화이트")] // 인스펙터에 툴팁 표시
    [SerializeField] public GameObject[] bulletObjArray; // 무기 레벨에 따른 총알 프리팹 배열

    [Space(10)]
    public float speed;      // 플레이어 이동 속도
    public int attack;       // 플레이어 공격력
    public int weaponLevel;  // 무기 레벨 (1~4)

    // 공격 속도 관련 변수들
    public float attackSpeed = 1;           // 공격 속도 (낮을수록 더 빠름)
    float shotBasicReduceSpeed = 0.2f;      // 기본적인 빨라지는 공속감소 값치 (임시 테스트 수치 보정:0.05)
    float maxShotSpeed = 0.15f;             // 공속 최대치 (이보다 낮아지지 않음)

    public Transform pos;    // 총알 발사 위치

    Animator moveAni;        // 이동 애니메이션 컴포넌트
    WaitForSeconds wait;     // 공속 대기 시간 (코루틴에서 사용)

    // HideInInspector 속성은 인스펙터에서 해당 변수를 숨깁니다
    [HideInInspector] public int monsterKillCount; // 몬스터 처치 횟수
    [HideInInspector] public bool isDead;          // 플레이어 사망 상태
    
    // 플레이어의 체력. 각 플레이어는 1의 체력을 가집니다.
    private int health = 1;
    
    // 메인 플레이어인지 여부 (첫 번째 플레이어 = true, 복제된 플레이어 = false)
    private bool isMainPlayer = false;
    
    // 메인 플레이어 참조 (복제된 플레이어만 사용)
    private Player mainPlayerReference;
    
    // 총알 발사 코루틴 참조 (중지/재시작에 사용)
    private Coroutine shootCoroutine;
    
    // 이미 초기화 되었는지 여부
    private bool isInitialized = false;
    
    // 디버그 로그 활성화 여부
    [SerializeField] private bool enableDebugLog = true;

    /// <summary>
    /// 게임 시작시 호출되는 메서드
    /// </summary>
    void Start()
    {
        Initialize(); // 플레이어 초기화
    }
    
    /// <summary>
    /// 플레이어 초기화 메서드 (Start 또는 SetAsClone에서 호출)
    /// 기본 스탯 설정, 컴포넌트 연결, 이벤트 등록 등을 수행합니다.
    /// </summary>
    private void Initialize()
    {
        // 이미 초기화되었으면 중복 실행 방지
        if (isInitialized)
            return;
            
        isInitialized = true;
        isDead = false;

        // 기본 스탯 설정
        attack = 1;
        weaponLevel = 1;
        monsterKillCount = 0;

        // 공격 속도 관련 변수 초기화
        attackSpeed = 1;
        shotBasicReduceSpeed = 0.2f;
        maxShotSpeed = 0.15f;

        wait = new WaitForSeconds(attackSpeed);
        moveAni = GetComponent<Animator>();

        // 플레이어 매니저의 존재 여부 체크하여 메인/복제 플레이어 구분
        if (PlayerManager.instance != null && PlayerManager.instance.transform.childCount > 0)
        {
            // 첫 번째 자식이 이 오브젝트인 경우, 메인 플레이어로 설정
            if (PlayerManager.instance.transform.GetChild(0).gameObject == gameObject)
            {
                isMainPlayer = true;
                LogDebug("이 플레이어는 메인 플레이어로 설정됨");
            }
            else
            {
                LogDebug("이 플레이어는 복제 플레이어로 간주됨 (PlayerManager 자식)");
            }
        }
        else
        {
            // 플레이어 매니저가 없으면 항상 메인 플레이어
            isMainPlayer = true;
            LogDebug("PlayerManager가 없어 메인 플레이어로 설정됨");
        }

        // 총알 발사 코루틴 시작
        StartShootingCoroutine();
        
        // 이벤트는 메인 플레이어만 처리
        if (isMainPlayer)
        {
            // 이벤트 매니저 이벤트에 메서드 등록
            EventManager.instance.playerEvents.onWeaponUpgrade += ShootSpeedSet;
            EventManager.instance.playerEvents.onPlayerDead += GameOverUiOpen;
            EventManager.instance.playerEvents.onMonsterDead += ChangeBullet;
        }
    }
    
    /// <summary>
    /// 디버그 로그를 출력하는 유틸리티 메서드
    /// enableDebugLog가 true일 때만 로그를 출력합니다.
    /// </summary>
    /// <param name="message">출력할 메시지</param>
    private void LogDebug(string message)
    {
        if (enableDebugLog)
        {
            Debug.Log("[Player " + gameObject.name + "] " + message);
        }
    }
    
    /// <summary>
    /// 총알 발사 코루틴을 시작하는 메서드
    /// 기존 코루틴이 실행 중이면 중지하고 새로 시작합니다.
    /// </summary>
    public void StartShootingCoroutine()
    {
        // 기존에 실행 중인 코루틴이 있으면 중지
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
        
        // 총알 발사 위치(pos) 컴포넌트 확인
        if (pos == null)
        {
            // pos가 없으면 찾거나 새로 생성
            pos = transform.Find("FirePosition");
            if (pos == null)
            {
                // FirePosition 오브젝트 생성
                GameObject firePos = new GameObject("FirePosition");
                firePos.transform.SetParent(transform);
                firePos.transform.localPosition = new Vector3(0, 0.5f, 0); // 플레이어 위쪽에 위치
                pos = firePos.transform;
                LogDebug("FirePosition이 없어 새로 생성함");
            }
        }
        
        // 총알 발사 코루틴 시작
        shootCoroutine = StartCoroutine(Shoot());
        LogDebug("총알 발사 코루틴 시작됨");
    }

    /// <summary>
    /// 매 프레임마다 호출되는 업데이트 메서드
    /// 플레이어 이동 및 무기 동기화를 처리합니다.
    /// </summary>
    void Update()
    {
        // 메인 플레이어만 입력 처리하고 나머지는 메인 플레이어와 함께 움직임
        if (isMainPlayer)
        {
            HandleMovement(); // 이동 처리
        }
        
        // 복제 플레이어는 무기 레벨과 공격력을 메인 플레이어와 동기화
        if (!isMainPlayer && mainPlayerReference != null)
        {
            SyncWeaponWithMain(); // 무기 속성 동기화
        }
    }
    
    /// <summary>
    /// 메인 플레이어와 무기 속성을 동기화하는 메서드
    /// 복제 플레이어에서만 사용됩니다.
    /// </summary>
    private void SyncWeaponWithMain()
    {
        // 무기 레벨이 달라지면 변경
        if (weaponLevel != mainPlayerReference.weaponLevel)
        {
            weaponLevel = mainPlayerReference.weaponLevel;
            LogDebug("무기 레벨 동기화: " + weaponLevel);
        }
        
        // 공격력이 달라지면 변경
        if (attack != mainPlayerReference.attack)
        {
            attack = mainPlayerReference.attack;
            LogDebug("공격력 동기화: " + attack);
        }
        
        // 공격 속도가 달라지면 변경
        if (attackSpeed != mainPlayerReference.attackSpeed)
        {
            attackSpeed = mainPlayerReference.attackSpeed;
            wait = new WaitForSeconds(attackSpeed);
            LogDebug("공격 속도 동기화: " + attackSpeed);
        }
    }
    
    /// <summary>
    /// 이동 처리를 담당하는 메서드 (메인 플레이어용)
    /// 키보드 입력에 따라 플레이어를 좌우로 이동시킵니다.
    /// </summary>
    private void HandleMovement()
    {
        // 수평 입력에 따라 이동 거리 계산
        float distanceX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        
        // 왼쪽 이동 애니메이션 설정
        if (Input.GetAxis("Horizontal") <= -2f)
            moveAni.SetBool("left", true);
        else
            moveAni.SetBool("left", false);

        // 오른쪽 이동 애니메이션 설정
        if (Input.GetAxis("Horizontal") >= 2f)
            moveAni.SetBool("right", true);
        else
            moveAni.SetBool("right", false);

        // 부모 객체가 있으면 부모를 이동 (여러 플레이어가 함께 움직이도록)
        if (transform.parent != null)
        {
            transform.parent.Translate(distanceX, 0, 0);
            
            // 경계 제한 (화면 밖으로 나가지 않도록)
            Vector3 parentPos = transform.parent.position;
            parentPos.x = Mathf.Clamp(parentPos.x, -2, 2);
            transform.parent.position = parentPos;
        }
        else
        {
            // 부모가 없으면 직접 이동
            transform.Translate(distanceX, 0, 0);
            
            // 경계 제한
            Vector3 playerPos = transform.position;
            playerPos.x = Mathf.Clamp(playerPos.x, -2, 2);
            transform.position = playerPos;
        }
    }

    /// <summary>
    /// 충돌 시 호출되는 메서드
    /// 적의 총알에 맞았을 때 피격 처리를 합니다.
    /// </summary>
    /// <param name="collision">충돌한 객체의 Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적 총알에 맞았을 때
        if (collision.CompareTag("EBullet"))
        {
            // 플레이어 매니저가 있는 경우 데미지 처리
            if (PlayerManager.instance != null)
            {
                PlayerManager.instance.TakeDamage();
                Destroy(collision.gameObject); // 총알 제거
                return; // 여기서 종료하여 아래 코드 실행 안하게 함
            }
            
            // 플레이어 매니저가 없는 경우 원래대로 동작
            isDead = true;

            // 이벤트 발생 및 이벤트 리스너 해제
            EventManager.instance.playerEvents.PlayerDead();
            EventManager.instance.playerEvents.onPlayerDead -= GameOverUiOpen;

            //Destroy(gameObject); // 오브젝트 파괴 (주석 처리됨)
            GetComponent<Collider2D>().enabled = false; // 충돌 비활성화
            EventManager.instance.playerEvents.onWeaponUpgrade -= ShootSpeedSet;
            EventManager.instance.playerEvents.onMonsterDead -= ChangeBullet;
        }
    }

    /// <summary>
    /// 이 플레이어를 복제 플레이어로 설정하는 메서드
    /// 메인 플레이어의 속성을 복사하고 복제 플레이어로서의 특성을 설정합니다.
    /// </summary>
    /// <param name="mainPlayer">복사할 메인 플레이어 객체</param>
    public void SetAsClone(Player mainPlayer)
    {
        // 메인 플레이어가 없으면 종료
        if (mainPlayer == null)
        {
            LogDebug("SetAsClone: 메인 플레이어가 null입니다!");
            return;
        }
        
        LogDebug("SetAsClone: 복제 플레이어로 설정 시작");
            
        isMainPlayer = false;
        mainPlayerReference = mainPlayer;
        
        // 중요! 메인 플레이어의 총알 배열 복사 (참조로 변경)
        if (mainPlayer.bulletObjArray != null && mainPlayer.bulletObjArray.Length > 0)
        {
            // 참조로 복사 (깊은 복사 대신)
            bulletObjArray = mainPlayer.bulletObjArray;
            LogDebug("메인 플레이어의 총알 배열 참조 설정 완료");
        }
        else
        {
            LogDebug("경고: 메인 플레이어의 총알 배열이 비어 있습니다!");
        }
        
        // 메인 플레이어의 속성 복사
        attack = mainPlayerReference.attack;
        weaponLevel = mainPlayerReference.weaponLevel;
        attackSpeed = mainPlayerReference.attackSpeed;
        
        // 기존에 등록된 이벤트 해제 (복제 플레이어는 이벤트를 처리하지 않음)
        EventManager.instance.playerEvents.onWeaponUpgrade -= ShootSpeedSet;
        EventManager.instance.playerEvents.onPlayerDead -= GameOverUiOpen;
        EventManager.instance.playerEvents.onMonsterDead -= ChangeBullet;
        
        // 복제 플레이어도 초기화하고 총알 발사 시작
        wait = new WaitForSeconds(attackSpeed);
        
        // 애니메이터 설정
        moveAni = GetComponent<Animator>();
        
        // 총알 발사 위치 확인 및 생성
        if (pos == null)
        {
            pos = transform.Find("FirePosition");
            if (pos == null)
            {
                GameObject firePos = new GameObject("FirePosition");
                firePos.transform.SetParent(transform);
                firePos.transform.localPosition = new Vector3(0, 0.5f, 0);
                pos = firePos.transform;
                LogDebug("FirePosition 생성됨");
            }
        }
        
        // 총알 발사 시작 - 중요! 복제 플레이어도 발사해야 함
        StartShootingCoroutine();
        
        // 복제 플레이어의 컴포넌트 체크 및 추가
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            LogDebug("Rigidbody2D 컴포넌트 추가됨");
        }
        
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
            LogDebug("BoxCollider2D 컴포넌트 추가됨");
        }
        
        // 초기화 완료 표시
        isInitialized = true;
        LogDebug("SetAsClone: 복제 플레이어 설정 완료");
    }

    /// <summary>
    /// 공격 속도와 발사체 속도를 설정하는 메서드
    /// 무기 업그레이드 시 호출됩니다.
    /// </summary>
    void ShootSpeedSet()
    {
        // 공격 속도 감소 (더 빠르게 발사)
        attackSpeed -= shotBasicReduceSpeed;

        // 최대 공격 속도 제한
        if (attackSpeed < maxShotSpeed)
        {
            attackSpeed = maxShotSpeed;
        }

        // 대기 시간 업데이트
        wait = new WaitForSeconds(attackSpeed);

        Debug.Log("attackSpeed : " + attackSpeed);
    }

    /// <summary>
    /// 총알을 발사하는 코루틴
    /// 플레이어가 살아있는 동안 반복적으로 총알을 발사합니다.
    /// </summary>
    IEnumerator Shoot()
    {
        LogDebug("총알 발사 코루틴 실행 중");
        
        // bulletObjArray 확인
        if (bulletObjArray == null || bulletObjArray.Length == 0)
        {
            LogDebug("심각: 총알 배열이 null이거나 비어 있습니다! 코루틴 종료");
            yield break; // 코루틴 종료
        }
        
        // 플레이어가 살아있는 동안 반복
        while (!isDead)
        {
            // 공격 속도만큼 대기
            yield return wait;
            
            try {
                // 무기 레벨 및 총알 발사 위치 확인
                if (pos == null)
                {
                    LogDebug("발사 위치(pos)가 없어 생성합니다");
                    GameObject firePos = new GameObject("FirePosition");
                    firePos.transform.SetParent(transform);
                    firePos.transform.localPosition = new Vector3(0, 0.5f, 0);
                    pos = firePos.transform;
                }

                // 무기 레벨에 맞는 총알 인덱스 계산 (범위 제한)
                int bulletIndex = Mathf.Clamp(weaponLevel - 1, 0, bulletObjArray.Length - 1);
                
                // 총알 프리팹 확인
                if (bulletObjArray[bulletIndex] == null)
                {
                    LogDebug("총알 프리팹이 null입니다! 인덱스: " + bulletIndex);
                    continue; // 이번 반복은 건너뜀
                }

                // 총알 생성
                GameObject go = Instantiate(bulletObjArray[bulletIndex], pos.position, Quaternion.identity);
                LogDebug("총알 생성됨: " + go.name);
                
                // 총알 스크립트 설정 (속도, 공격력)
                Bullet bulletScript = go.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.speed += (1 - attackSpeed) * 4; // 공격 속도에 따른 총알 속도 조정
                    bulletScript.attack = this.attack; // 플레이어 공격력을 총알에 적용
                }
                else
                {
                    LogDebug("경고: Bullet 컴포넌트가 null입니다!");
                }
            }
            catch (System.Exception e)
            {
                LogDebug("총알 생성 중 오류 발생: " + e.Message);
            }
        }
        
        LogDebug("총알 발사 코루틴 종료 (isDead: " + isDead + ")");
    }

    /// <summary>
    /// 무기 변경 및 공격력 증가 메서드
    /// 몬스터 처치 시 호출되어 무기 레벨과 공격력을 증가시킵니다.
    /// </summary>
    void ChangeBullet()     //무기변경  (임시) 
    {
        monsterKillCount += 1;

        // 몬스터 처치 횟수에 따른 무기 레벨 및 공격력 증가
        if (monsterKillCount == 1)
        {
            weaponLevel += 1; // 레벨 2
            attack = 6;      // 공격력 6
        }
        else if (monsterKillCount == 2)
        {
            weaponLevel += 1; // 레벨 3
            attack = 24;     // 공격력 24
        }
        else if (monsterKillCount == 3)
        {
            weaponLevel += 1; // 레벨 4
            attack = 125;    // 공격력 125
        }

        //Debug.Log("몬스터 처치횟수: " +monsterKillCount );
        Debug.Log("공격력: " + attack);
    }

    /// <summary>
    /// 게임 오버 UI를 표시하는 메서드
    /// 플레이어 사망 시 호출됩니다.
    /// </summary>
    void GameOverUiOpen()
    {
        if (gameOverUIObj != null)
        {
            gameOverUIObj.SetActive(true);
        }
    }
    
    /// <summary>
    /// 객체 파괴 시 호출되는 메서드
    /// 코루틴 정지 및 이벤트 해제를 담당합니다.
    /// </summary>
    private void OnDestroy()
    {
        // 코루틴 정지
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
        
        // 이벤트 해제 (메인 플레이어만)
        if (isMainPlayer)
        {
            EventManager.instance.playerEvents.onWeaponUpgrade -= ShootSpeedSet;
            EventManager.instance.playerEvents.onPlayerDead -= GameOverUiOpen;
            EventManager.instance.playerEvents.onMonsterDead -= ChangeBullet;
        }
    }
}