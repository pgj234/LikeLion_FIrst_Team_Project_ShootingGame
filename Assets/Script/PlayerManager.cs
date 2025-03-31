using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    //public static PlayerManager instance;
    
    // 플레이어 프리팹 및 부모 오브젝트 참조
    [SerializeField] private GameObject originalPlayer;   // 원본 플레이어 오브젝트
    //[SerializeField] private Transform playersParent; // 플레이어들을 담을 부모 오브젝트
    
    [Header("플레이어 설정")]
    [SerializeField] private int maxPlayerCount = 10; // 최대 플레이어 수
    [SerializeField] private float circleRadius = 0.5f; // 원형 대형의 반지름

    [Space(20)]
    public float speed;
    public int attack;
    public int weaponLevel;

    [HideInInspector] public int monsterKillCount;

    [Space(10)]
    float managerAttackSpeed = 1;           // 공속
    float managerShotBasicReduceSpeed = 0.2f;      // 기본적인 업글당 오르는 공속치 (임시 테스트 수치 기존:0.05)
    float managerMaxShotSpeed = 0.15f;             // 공속 최대치

    //[Header("체력 바 설정")]
    //[SerializeField] private GameObject healthBarPrefab; // 체력 바 프리팹
    //[SerializeField] private Color healthBarColor = Color.green; // 체력 바 색상

    //[Header("디버그 설정")]
    //[SerializeField] private bool enableDebugLog = true; // 디버그 로그 활성화 여부

    // 플레이어 관리 변수
    private List<GameObject> playersList = new List<GameObject>(); // 플레이어 리스트
    //private int currentPlayerCount = 1; // 현재 플레이어 수 (처음에는 1명)
    private Vector3 originalPlayerPosition; // 원래 플레이어 위치

    internal bool isDead;

    //private Player mainPlayer; // 메인 플레이어 참조

    // 플레이어 증가 및 감소
    internal void PlayerAddOrDecrease(int num)
    {
        if (0 == num)       // 변화X
        {
            return;
        }

        if (maxPlayerCount <= playersList.Count)        // 이미 최대
        {
            return;
        }

        for (int i=0; i<Mathf.Abs(num); i++)
        {
            if (0 < num)        // 증가
            {
                if (maxPlayerCount <= playersList.Count)
                {
                    break;
                }

                GameObject go = Instantiate(originalPlayer, transform);
                Player player = go.GetComponent<Player>();
                player.isCopyPlayer = true;
                player.Init();
                player.wait = originalPlayer.GetComponent<Player>().wait;
                playersList.Add(go);
            }
            else if (0 > num)   // 감소
            {
                if (1 >= playersList.Count)
                {
                    break;
                }

                Destroy(playersList[1]);
                playersList.RemoveAt(1);
            }
        }

        // 원형 대형으로 플레이어들 위치 업데이트
        UpdatePlayersFormation();
    }

    // 원형 대형으로 플레이어들 위치 업데이트
    void UpdatePlayersFormation()
    {
        if (playersList.Count <= 0)
            return;

        // 첫 번째 플레이어는 원점에 위치
        if (playersList.Count == 1)
        {
            playersList[0].transform.localPosition = Vector3.zero;
            return;
        }

        // 2명 이상일 경우 원형으로 배치
        float angleStep = 360f / playersList.Count;
        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i] == null) continue;

            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * circleRadius;
            float y = Mathf.Cos(angle) * circleRadius;

            Vector3 newPos = new Vector3(x, y, 0);
            playersList[i].transform.localPosition = newPos;
        }
    }

    internal void ChangeBullet()     //무기변경  (임시) 
    {
        monsterKillCount += 1;

        if (monsterKillCount == 1)
        {
            weaponLevel += 1;
            attack = 2;
        }
        else if (monsterKillCount == 2)
        {
            weaponLevel += 1;
            attack = 4;
        }
        else if (monsterKillCount == 3)
        {
            weaponLevel += 1;
            attack = 7;
        }

        //Debug.Log("몬스터 죽인횟수: " +monsterKillCount );
        Debug.Log("공격력: " + attack);
    }

    //private void Awake()
    //{
    //    // 싱글톤 패턴 구현
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject); // 기존 인스턴스가 있으면 현재 오브젝트 파괴
    //    }
    //}

    // 모든 플레이어 공속 및 발사체 속도 변경
    internal void ShootSpeedSet()
    {
        SoundManager.instance.PlaySFX(Sound.GetItem);

        managerAttackSpeed -= managerShotBasicReduceSpeed;

        if (managerAttackSpeed < managerMaxShotSpeed)
        {
            managerAttackSpeed = managerMaxShotSpeed;
        }

        for (int i = 0; i < playersList.Count; i++)
        {
            Player player = playersList[i].GetComponent<Player>();
            player.attackSpeed = managerAttackSpeed;
            player.wait = new WaitForSeconds(managerAttackSpeed);
        }

        Debug.Log("managerAttackSpeed : " + managerAttackSpeed);
    }

    private void Start()
    {
        isDead = false;

        speed = 5;
        attack = 1;
        weaponLevel = 1;
        monsterKillCount = 0;

        Player player = originalPlayer.GetComponent<Player>();
        player.isCopyPlayer = false;
        player.Init();
        playersList.Add(originalPlayer);

        //LogDebug("PlayerManager 시작");

        // 이벤트 등록 - 무기 업그레이드 시 플레이어 추가
        //if (EventManager.instance != null)
        //{
        //    EventManager.instance.playerEvents.onWeaponUpgrade += AddPlayer;
        //    LogDebug("WeaponUpgrade 이벤트에 AddPlayer 등록됨");
        //}
        //else
        //{
        //    LogDebug("EventManager가 존재하지 않습니다!");
        //}

        // 처음 플레이어가 들어있는지 확인 (씬에 원래 있던 플레이어 찾기)
        //Player originalPlayer = FindObjectOfType<Player>();
        //if (originalPlayer != null)
        //{
        //    // 메인 플레이어 저장
        //    mainPlayer = originalPlayer;
        //    LogDebug("메인 플레이어 찾음: " + originalPlayer.name);

        //    // 원래 플레이어의 위치 저장
        //    originalPlayerPosition = originalPlayer.transform.position;

        //    // 플레이어 리스트에 추가하고 체력바 붙이기
        //    playersList.Add(originalPlayer.gameObject);
        //    AttachHealthBar(originalPlayer.gameObject);

        //    // 플레이어 부모 오브젝트가 없다면 생성
        //    if (playersParent == null)
        //    {
        //        GameObject parent = new GameObject("PlayersParent");
        //        playersParent = parent.transform;
        //        LogDebug("PlayersParent 오브젝트 생성됨");

        //        // 부모 오브젝트를 원래 플레이어 위치로 이동
        //        playersParent.position = originalPlayerPosition;

        //        // 플레이어를 부모 오브젝트의 자식으로 설정 (로컬 위치는 원점)
        //        originalPlayer.transform.SetParent(playersParent);
        //        originalPlayer.transform.localPosition = Vector3.zero;
        //        LogDebug("메인 플레이어를 PlayersParent의 자식으로 설정");
        //    }
        //}
        //else
        //{
        //    LogDebug("씬에서 플레이어를 찾을 수 없습니다!");
        //}

        // 플레이어들 위치 업데이트
        //    UpdatePlayersFormation();
    }

    void Update()
    {
        if (true == isDead || true == UIManager.instance.UiOpenStateGet(UIType.ClearStage) || true == UIManager.instance.UiOpenStateGet(UIType.BossClear))
        {
            return;
        }

        float distanceX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        transform.Translate(distanceX, 0, 0);


        //Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        //viewPos.x = Mathf.Clamp01(viewPos.x);
        //viewPos.y = Mathf.Clamp01(viewPos.y);
        //Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        //transform.position = worldPos;

        Vector3 groundPos;
        groundPos = transform.position;
        groundPos.x = Mathf.Clamp(groundPos.x, -2, 2);
        transform.position = groundPos;

        // 메인 플레이어의 공격력, 무기 레벨을 모든 복제 플레이어에게 적용
        //if (mainPlayer != null && playersList.Count > 1)
        //{
        //    SyncPlayerAttributes();
        //}
    }

    // 디버그 로그 출력
    //private void LogDebug(string message)
    //{
    //    if (enableDebugLog)
    //    {
    //        Debug.Log("[PlayerManager] " + message);
    //    }
    //}

    // 플레이어 속성 동기화 (공격력, 무기 레벨 등)
    //private void SyncPlayerAttributes()
    //{
    //    for (int i = 1; i < playersList.Count; i++) // 첫 번째(메인 플레이어)는 제외
    //    {
    //        if (playersList[i] == null) continue;

    //        Player clonePlayer = playersList[i].GetComponent<Player>();
    //        if (clonePlayer != null)
    //        {
    //            // 메인 플레이어의 속성을 복제 플레이어에게 적용
    //            clonePlayer.attack = mainPlayer.attack;
    //            clonePlayer.weaponLevel = mainPlayer.weaponLevel;
    //            clonePlayer.attackSpeed = mainPlayer.attackSpeed;

    //            // 애니메이션 상태 동기화 (선택적)
    //            Animator mainAnim = mainPlayer.GetComponent<Animator>();
    //            Animator cloneAnim = clonePlayer.GetComponent<Animator>();
    //            if (mainAnim != null && cloneAnim != null)
    //            {
    //                cloneAnim.SetBool("left", mainAnim.GetBool("left"));
    //                cloneAnim.SetBool("right", mainAnim.GetBool("right"));
    //            }
    //        }
    //    }
    //}

    // 새 플레이어 추가 메서드
    //public void AddPlayer()
    //{
    //    LogDebug("플레이어 추가 시도! 현재 플레이어 수: " + currentPlayerCount);

    //    // 최대 플레이어 수 체크
    //    if (currentPlayerCount >= maxPlayerCount)
    //    {
    //        LogDebug("최대 플레이어 수 도달!");
    //        return;
    //    }

    //    GameObject newPlayer = null;

    //    // 메인 플레이어가 없으면 생성 실패
    //    if (mainPlayer == null)
    //    {
    //        LogDebug("메인 플레이어가 없어 복제할 수 없습니다!");
    //        return;
    //    }

    //    // playerPrefab이 설정되지 않았고 기존 플레이어가 있는 경우
    //    if (playerPrefab == null && playersList.Count > 0)
    //    {
    //        // 프리팹이 설정되지 않았다면 기존 플레이어를 복제
    //        LogDebug("기존 플레이어 복제 중...");

    //        // 플레이어 부모가 설정되어 있는지 확인
    //        if (playersParent == null)
    //        {
    //            playersParent = transform;
    //            LogDebug("PlayersParent가 null이어서 현재 객체로 설정");
    //        }

    //        // 방법 1: 기존 플레이어 게임 오브젝트 직접 복제
    //        // newPlayer = Instantiate(playersList[0], playersParent);

    //        // 방법 2: 플레이어 프리팹 생성 후 컴포넌트 직접 설정 (더 안정적)
    //        newPlayer = new GameObject("ClonePlayer_" + currentPlayerCount);
    //        newPlayer.transform.SetParent(playersParent);
    //        newPlayer.transform.localPosition = Vector3.zero;

    //        // 스프라이트 복사
    //        SpriteRenderer originalSprite = playersList[0].GetComponent<SpriteRenderer>();
    //        if (originalSprite != null)
    //        {
    //            SpriteRenderer newSprite = newPlayer.AddComponent<SpriteRenderer>();
    //            newSprite.sprite = originalSprite.sprite;
    //            newSprite.sortingLayerName = originalSprite.sortingLayerName;
    //            newSprite.sortingOrder = originalSprite.sortingOrder;
    //            LogDebug("스프라이트 복사됨");
    //        }

    //        // 애니메이터 복사
    //        Animator originalAnimator = playersList[0].GetComponent<Animator>();
    //        if (originalAnimator != null)
    //        {
    //            Animator newAnimator = newPlayer.AddComponent<Animator>();
    //            newAnimator.runtimeAnimatorController = originalAnimator.runtimeAnimatorController;
    //            LogDebug("애니메이터 복사됨");
    //        }

    //        // Rigidbody2D 추가
    //        Rigidbody2D rb = newPlayer.AddComponent<Rigidbody2D>();
    //        rb.gravityScale = 0;
    //        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    //        // BoxCollider2D 추가
    //        BoxCollider2D collider = newPlayer.AddComponent<BoxCollider2D>();
    //        BoxCollider2D originalCollider = playersList[0].GetComponent<BoxCollider2D>();
    //        if (originalCollider != null)
    //        {
    //            collider.size = originalCollider.size;
    //            collider.offset = originalCollider.offset;
    //            collider.isTrigger = true;
    //        }

    //        // 총알 발사 위치 생성
    //        GameObject firePos = new GameObject("FirePosition");
    //        firePos.transform.SetParent(newPlayer.transform);
    //        firePos.transform.localPosition = new Vector3(0, 0.5f, 0);

    //        // Player 컴포넌트 추가 및 설정
    //        Player newPlayerScript = newPlayer.AddComponent<Player>();
    //        newPlayerScript.pos = firePos.transform;

    //        // 버그 예방: 복제 후 자식 객체 설정 (일부 경우에 부모가 제대로 설정되지 않음)
    //        if (newPlayer.transform.parent != playersParent)
    //        {
    //            newPlayer.transform.SetParent(playersParent);
    //            LogDebug("복제 후 부모가 잘못 설정되어 다시 설정함");
    //        }

    //        // 복제된 플레이어의 불필요한 행동 제한 (메인 플레이어가 아님)
    //        Player playerScript = newPlayer.GetComponent<Player>();
    //        if (playerScript != null)
    //        {
    //            // 총알 배열이 복사되는지 확인
    //            //if (mainPlayer.bulletObjArray != null && mainPlayer.bulletObjArray.Length > 0)
    //            //{
    //            //    // bulletObjArray는 Player.SetAsClone에서 복사됨
    //            //    LogDebug("총알 배열 확인: " + (mainPlayer.bulletObjArray != null ? mainPlayer.bulletObjArray.Length + "개" : "없음"));
    //            //}
    //            //else
    //            //{
    //            //    LogDebug("경고: 메인 플레이어의 총알 배열이 비어 있습니다!");
    //            //}

    //            //// 복제 플레이어 설정
    //            //playerScript.SetAsClone(mainPlayer);
    //            LogDebug("복제 플레이어 설정 완료!");

    //            // 총알 발사 확인
    //            Transform cloneFirePos = newPlayer.transform.Find("FirePosition");
    //            if (cloneFirePos == null)
    //            {
    //                GameObject newFirePos = new GameObject("FirePosition");
    //                newFirePos.transform.SetParent(newPlayer.transform);
    //                newFirePos.transform.localPosition = new Vector3(0, 0.5f, 0);
    //                playerScript.pos = newFirePos.transform;
    //                LogDebug("복제 플레이어에 FirePosition 추가됨");
    //            }
    //            else
    //            {
    //                playerScript.pos = cloneFirePos;
    //                LogDebug("복제 플레이어의 FirePosition 설정됨");
    //            }
    //        }
    //        else
    //        {
    //            LogDebug("Player 컴포넌트가 없습니다!");
    //        }
    //    }
    //    else if (playerPrefab != null)
    //    {
    //        // 프리팹으로 새 플레이어 생성
    //        LogDebug("플레이어 프리팹으로 생성 중...");

    //        // 플레이어 부모가 설정되어 있는지 확인
    //        if (playersParent == null)
    //        {
    //            playersParent = transform;
    //            LogDebug("PlayersParent가 null이어서 현재 객체로 설정");
    //        }

    //        newPlayer = Instantiate(playerPrefab, playersParent);

    //        // 복제된 플레이어의 불필요한 행동 제한 (메인 플레이어가 아님)
    //        Player playerScript = newPlayer.GetComponent<Player>();
    //        if (playerScript != null)
    //        {
    //            // 총알 발사 위치 설정
    //            Transform firePos = newPlayer.transform.Find("FirePosition");
    //            if (firePos == null)
    //            {
    //                GameObject newFirePos = new GameObject("FirePosition");
    //                newFirePos.transform.SetParent(newPlayer.transform);
    //                newFirePos.transform.localPosition = new Vector3(0, 0.5f, 0);
    //                playerScript.pos = newFirePos.transform;
    //                LogDebug("플레이어 프리팹에 FirePosition 추가됨");
    //            }

    //            // 복제 플레이어 설정
    //            //playerScript.SetAsClone(mainPlayer);
    //            LogDebug("복제 플레이어 설정 완료!");
    //        }
    //        else
    //        {
    //            LogDebug("플레이어 프리팹에 Player 컴포넌트가 없습니다!");
    //        }
    //    }

    //    // 새 플레이어 추가 완료 처리
    //    if (newPlayer != null)
    //    {
    //        playersList.Add(newPlayer);
    //        AttachHealthBar(newPlayer);
    //        currentPlayerCount++;
    //        LogDebug("플레이어 추가 성공! 현재 플레이어 수: " + currentPlayerCount);
    //    }
    //    else
    //    {
    //        LogDebug("플레이어 추가 실패!");
    //    }

    //    // 플레이어들 위치 업데이트
    //    UpdatePlayersFormation();
    //}

    // 플레이어 제거 메서드
    //public void RemovePlayer(GameObject playerToRemove)
    //{
    //if (playersList.Contains(playerToRemove))
    //{
    //    playersList.Remove(playerToRemove);
    //    Destroy(playerToRemove);
    //    currentPlayerCount--;
    //    LogDebug("플레이어 제거됨! 남은 플레이어 수: " + currentPlayerCount);

    //    // 플레이어들 위치 업데이트
    //    UpdatePlayersFormation();
    //}
    //}

    // 데미지 입었을 때 호출될 메서드
    //public void TakeDamage()
    //{
    //    if (playersList.Count > 1)
    //    {
    //        // 마지막 플레이어 제거
    //        GameObject playerToRemove = playersList[playersList.Count - 1];
    //        RemovePlayer(playerToRemove);
    //    }
    //    else
    //    {
    //        // 마지막 플레이어면 게임 오버 처리
    //        Player lastPlayer = playersList[0].GetComponent<Player>();
    //        if (lastPlayer != null && !lastPlayer.isDead)
    //        {
    //            Collider2D collider = lastPlayer.GetComponent<Collider2D>();
    //            if (collider != null)
    //            {
    //                collider.enabled = false;
    //            }
    //            lastPlayer.isDead = true;
    //            EventManager.instance.playerEvents.PlayerDead();
    //        }
    //    }
    //}

    // 체력 바 추가 메서드
    //private void AttachHealthBar(GameObject player)
    //{
    //    if (healthBarPrefab == null)
    //    {
    //        LogDebug("체력바 프리팹이 설정되지 않았습니다!");
    //        return;
    //    }

    //    GameObject healthBar = Instantiate(healthBarPrefab, player.transform);
    //    healthBar.transform.localPosition = new Vector3(0, 0.5f, 0); // 머리 위에 위치

    //    // 체력 바 설정 (필요에 따라 수정)
    //    HealthBar healthBarComponent = healthBar.GetComponent<HealthBar>();
    //    if (healthBarComponent != null)
    //    {
    //        healthBarComponent.SetColor(healthBarColor);
    //    }
    //}

    // 메인 플레이어 가져오기
    //public Player GetMainPlayer()
    //{
    //    return mainPlayer;
    //}

    // 디버그용: 현재 플레이어 수 확인
    //public int GetPlayerCount()
    //{
    //    return currentPlayerCount;
    //}

    // 모든 플레이어 목록 가져오기
    //public List<GameObject> GetAllPlayers()
    //{
    //    return playersList;
    //}

    // 디버그용: 모든 플레이어의 상태 점검
    //public void CheckAllPlayersStatus()
    //{
    //    LogDebug("=== 모든 플레이어 상태 점검 ===");

    //    for (int i = 0; i < playersList.Count; i++)
    //    {
    //        if (playersList[i] == null)
    //        {
    //            LogDebug("플레이어 #" + i + ": null (제거됨)");
    //            continue;
    //        }

    //        Player playerScript = playersList[i].GetComponent<Player>();
    //        if (playerScript == null)
    //        {
    //            LogDebug("플레이어 #" + i + ": Player 컴포넌트 없음");
    //            continue;
    //        }

    //        LogDebug("플레이어 #" + i + 
    //            " | 위치: " + playersList[i].transform.localPosition + 
    //            " | 무기 레벨: " + playerScript.weaponLevel + 
    //            " | 공격력: " + playerScript.attack + 
    //            " | Fire Position: " + (playerScript.pos != null ? "있음" : "없음"));
    //    }

    //    LogDebug("=== 상태 점검 완료 ===");
    //}

    private void OnDestroy()
    {
        // 이벤트 해제
        if (EventManager.instance != null)
        {
            //EventManager.instance.playerEvents.onWeaponUpgrade -= AddPlayer;
        }
    }
} 