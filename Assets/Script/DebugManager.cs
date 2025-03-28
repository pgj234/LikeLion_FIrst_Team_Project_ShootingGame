//using UnityEngine;
//using System.Collections.Generic;

///// <summary>
///// 디버그 관리 및 테스트 기능을 제공하는 클래스
///// 플레이어 추가, 총알 제거, GunItem 생성 등의 디버그 기능을 제공합니다.
///// </summary>
//public class DebugManager : MonoBehaviour
//{
//    // 싱글톤 인스턴스
//    public static DebugManager instance;
    
//    // 디버그 설정
//    [SerializeField] private bool showDebugInfo = true;  // 화면에 디버그 정보 표시 여부
//    [SerializeField] private bool useDebugKeys = true;   // 디버그 키 사용 여부
    
//    // 플레이어 상태 체크 간격 관련 변수
//    private float timeSinceLastCheck = 0;               // 마지막 체크 이후 시간
//    private float checkInterval = 1f;                   // 체크 간격 (1초마다)

//    /// <summary>
//    /// 게임 시작 시 호출되는 메서드
//    /// 싱글톤 패턴을 구현합니다.
//    /// </summary>
//    private void Awake()
//    {
//        // 싱글톤 패턴 구현
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
//        }
//        else
//        {
//            Destroy(gameObject); // 기존 인스턴스가 있으면 현재 오브젝트 파괴
//        }
//    }

//    /// <summary>
//    /// 매 프레임마다 호출되는 업데이트 메서드
//    /// 플레이어 상태 체크 및 디버그 키 처리를 담당합니다.
//    /// </summary>
//    private void Update()
//    {
//        // 디버그 기능이 모두 비활성화된 경우 실행하지 않음
//        if (!showDebugInfo && !useDebugKeys)
//            return;
            
//        // 정해진 시간마다 플레이어 상태 체크
//        timeSinceLastCheck += Time.deltaTime;
//        if (timeSinceLastCheck >= checkInterval)
//        {
//            CheckPlayerState();
//            timeSinceLastCheck = 0;
//        }
        
//        // 디버그 키 처리
//        if (useDebugKeys)
//        {
//            HandleDebugKeys();
//        }
//    }

//    /// <summary>
//    /// 디버그 키 입력 처리 메서드
//    /// F1~F6 키를 사용하여 다양한 디버그 기능을 제공합니다.
//    /// </summary>
//    private void HandleDebugKeys()
//    {
//        // F1 키: 플레이어 추가
//        if (Input.GetKeyDown(KeyCode.F1))
//        {
//            Debug.Log("디버그: 수동으로 플레이어 추가 시도");
//            if (PlayerManager.instance != null)
//            {
//                PlayerManager.instance.AddPlayer();
//            }
//            else
//            {
//                Debug.LogError("PlayerManager를 찾을 수 없습니다!");
//            }
//        }
        
//        // F2 키: 모든 총알 지우기
//        if (Input.GetKeyDown(KeyCode.F2))
//        {
//            Debug.Log("디버그: 모든 총알 제거");
//            Bullet[] bullets = FindObjectsOfType<Bullet>();
//            foreach (Bullet bullet in bullets)
//            {
//                Destroy(bullet.gameObject);
//            }
//        }
        
//        // F3 키: 화면에 GunItem 생성
//        if (Input.GetKeyDown(KeyCode.F3))
//        {
//            Debug.Log("디버그: GunItem 생성");
//            GameObject gunItem = null;
            
//            // Resources 폴더에서 GunItem 프리팹 찾기 (첫 번째 시도)
//            GameObject gunPrefab = Resources.Load<GameObject>("GunItem");
//            if (gunPrefab != null)
//            {
//                gunItem = Instantiate(gunPrefab, new Vector3(0, 5, 0), Quaternion.identity);
//            }
//            else
//            {
//                Debug.LogError("GunItem 프리팹을 찾을 수 없습니다!");
                
//                // ItemSpawner에서 GunItem 프리팹 찾기 시도 (두 번째 시도)
//                ItemSpawner spawner = FindObjectOfType<ItemSpawner>();
//                if (spawner != null && spawner.gunPrefab != null)
//                {
//                    gunItem = Instantiate(spawner.gunPrefab, new Vector3(0, 5, 0), Quaternion.identity);
//                }
//            }
            
//            // 생성된 GunItem 초기화
//            if (gunItem != null)
//            {
//                GunItem gunItemScript = gunItem.GetComponent<GunItem>();
//                if (gunItemScript != null)
//                {
//                    gunItemScript.Init();
//                    Debug.Log("디버그: GunItem 생성 성공!");
//                }
//            }
//            else
//            {
//                Debug.LogError("GunItem을 생성할 수 없습니다!");
//            }
//        }
        
//        // F4 키: 모든 플레이어 상태 확인
//        if (Input.GetKeyDown(KeyCode.F4))
//        {
//            Debug.Log("디버그: 모든 플레이어 상태 확인");
//            if (PlayerManager.instance != null)
//            {
//                // PlayerManager의 내장 상태 체크 메서드 호출
//                PlayerManager.instance.CheckAllPlayersStatus();
                
//                // 플레이어 상태만 확인 (bulletObjArray 접근 제거)
//                List<GameObject> players = PlayerManager.instance.GetAllPlayers();
//                Player mainPlayer = PlayerManager.instance.GetMainPlayer();
                
//                // 플레이어 리스트 확인
//                if (mainPlayer != null && players != null)
//                {
//                    Debug.Log("총 플레이어 수: " + players.Count);
                    
//                    // 각 플레이어의 상태 확인
//                    for (int i = 0; i < players.Count; i++)
//                    {
//                        if (players[i] == null) continue;
                        
//                        Player playerScript = players[i].GetComponent<Player>();
//                        if (playerScript != null)
//                        {
//                            string playerType = (i == 0) ? "메인 플레이어" : "복제 플레이어 #" + i;
//                            Debug.Log(playerType + 
//                                " | 무기 레벨: " + playerScript.weaponLevel + 
//                                " | 공격력: " + playerScript.attack + 
//                                " | FirePosition: " + (playerScript.pos != null ? "있음" : "없음"));
//                        }
//                    }
//                }
//            }
//            else
//            {
//                Debug.LogError("PlayerManager를 찾을 수 없습니다!");
//            }
//        }
        
//        // F5 키: 복제 플레이어 총알 발사 재설정
//        if (Input.GetKeyDown(KeyCode.F5))
//        {
//            Debug.Log("디버그: 복제 플레이어 총알 발사 재설정");
//            if (PlayerManager.instance != null)
//            {
//                List<GameObject> players = PlayerManager.instance.GetAllPlayers();
//                Player mainPlayer = PlayerManager.instance.GetMainPlayer();
                
//                // 메인 플레이어와 복제 플레이어 확인
//                if (mainPlayer != null && players != null && players.Count > 1)
//                {
//                    // 모든 복제 플레이어 (인덱스 1부터)에 대해 처리
//                    for (int i = 1; i < players.Count; i++)
//                    {
//                        if (players[i] == null) continue;
                        
//                        Player clonePlayer = players[i].GetComponent<Player>();
//                        if (clonePlayer != null)
//                        {
//                            // FirePosition이 없으면 생성
//                            if (clonePlayer.pos == null)
//                            {
//                                GameObject firePos = new GameObject("FirePosition");
//                                firePos.transform.SetParent(players[i].transform);
//                                firePos.transform.localPosition = new Vector3(0, 0.5f, 0);
//                                clonePlayer.pos = firePos.transform;
//                            }
                            
//                            // 메인 플레이어로부터 복제 설정 강제 적용
//                            //clonePlayer.SetAsClone(mainPlayer);
//                            Debug.Log("플레이어 #" + i + " 재설정 완료");
//                        }
//                    }
//                }
//                else
//                {
//                    Debug.LogError("메인 플레이어 또는 복제 플레이어가 없습니다!");
//                }
//            }
//            else
//            {
//                Debug.LogError("PlayerManager를 찾을 수 없습니다!");
//            }
//        }
        
//        // F6 키: 복제 플레이어의 총알 발사 재설정 (bulletObjArray를 직접 복사하지 않고 수정)
//        if (Input.GetKeyDown(KeyCode.F6))
//        {
//            Debug.Log("디버그: 복제 플레이어 총알 발사 재설정");
//            if (PlayerManager.instance != null)
//            {
//                List<GameObject> players = PlayerManager.instance.GetAllPlayers();
//                Player mainPlayer = PlayerManager.instance.GetMainPlayer();
                
//                // 메인 플레이어와 복제 플레이어 확인
//                if (mainPlayer != null && players != null && players.Count > 1)
//                {
//                    int fixedCount = 0;
//                    // 모든 복제 플레이어 (인덱스 1부터)에 대해 처리
//                    for (int i = 1; i < players.Count; i++)
//                    {
//                        if (players[i] == null) continue;
                        
//                        Player clonePlayer = players[i].GetComponent<Player>();
//                        if (clonePlayer != null)
//                        {
//                            // FirePosition 확인 및 생성
//                            if (clonePlayer.pos == null)
//                            {
//                                GameObject firePos = new GameObject("FirePosition");
//                                firePos.transform.SetParent(players[i].transform);
//                                firePos.transform.localPosition = new Vector3(0, 0.5f, 0);
//                                clonePlayer.pos = firePos.transform;
//                            }
                            
//                            // 복제 플레이어 다시 설정 (메인 플레이어의 속성 복사)
//                            //clonePlayer.SetAsClone(mainPlayer);
                            
//                            // 총알 발사 코루틴 재시작
//                            //clonePlayer.StartShootingCoroutine();
                            
//                            fixedCount++;
//                            Debug.Log("플레이어 #" + i + " 총알 발사 재설정 완료");
//                        }
//                    }
                    
//                    Debug.Log("총 " + fixedCount + "개의 복제 플레이어 수정 완료");
//                }
//                else
//                {
//                    Debug.LogError("메인 플레이어 또는 복제 플레이어가 없습니다!");
//                }
//            }
//            else
//            {
//                Debug.LogError("PlayerManager를 찾을 수 없습니다!");
//            }
//        }
//    }
    
//    /// <summary>
//    /// 플레이어 상태를 주기적으로 확인하는 메서드
//    /// 현재 플레이어 수, 위치, 무기 레벨 등을 로그로 출력합니다.
//    /// </summary>
//    private void CheckPlayerState()
//    {
//        if (PlayerManager.instance != null)
//        {
//            // 현재 플레이어 수 체크
//            int playerCount = PlayerManager.instance.GetPlayerCount();
//            Debug.Log("현재 플레이어 수: " + playerCount);
            
//            // 플레이어 그룹(부모 오브젝트) 위치 체크
//            Transform parent = PlayerManager.instance.transform;
//            if (parent != null && parent.childCount > 0)
//            {
//                Debug.Log("플레이어 그룹 위치: " + parent.position);
                
//                // 각 자식 플레이어의 상태 체크
//                for (int i = 0; i < parent.childCount; i++)
//                {
//                    Transform child = parent.GetChild(i);
//                    Player playerScript = child.GetComponent<Player>();
                    
//                    // 플레이어 스크립트가 있으면 정보 로깅
//                    if (playerScript != null)
//                    {
//                        Debug.Log("플레이어 #" + i + " - 로컬 위치: " + child.localPosition + 
//                            ", 무기 레벨: " + playerScript.weaponLevel + 
//                            ", 공격력: " + playerScript.attack + 
//                            ", FirePosition: " + (playerScript.pos != null ? "O" : "X"));
//                    }
//                }
//            }
//        }
//        else
//        {
//            Debug.LogWarning("PlayerManager를 찾을 수 없습니다!");
//        }
//    }
    
//    /// <summary>
//    /// 화면에 디버그 정보를 표시하는 GUI 메서드
//    /// 현재 상태와 사용 가능한 디버그 키를 보여줍니다.
//    /// </summary>
//    private void OnGUI()
//    {
//        // 디버그 정보 표시 비활성화 상태면 반환
//        if (!showDebugInfo)
//            return;
            
//        // 화면 좌측 상단에 디버그 정보 표시
//        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
//        GUILayout.Label("디버그 정보");
        
//        // 사용 가능한 디버그 키 목록
//        GUILayout.Label("F1: 플레이어 추가");
//        GUILayout.Label("F2: 총알 제거");
//        GUILayout.Label("F3: GunItem 생성");
//        GUILayout.Label("F4: 플레이어 상태 확인");
//        GUILayout.Label("F5: 복제 플레이어 재설정");
//        GUILayout.Label("F6: 복제 플레이어 총알 발사 재설정");
        
//        // 현재 플레이어 수 표시
//        if (PlayerManager.instance != null)
//        {
//            GUILayout.Label("플레이어 수: " + PlayerManager.instance.GetPlayerCount());
//        }
//        else
//        {
//            GUILayout.Label("PlayerManager가 없습니다!");
//        }
        
//        // 스테이지 정보 표시
//        if (StageManager.instance != null)
//        {
//            GUILayout.Label("현재 스테이지: " + StageManager.instance.curStage);
//        }
        
//        // 몬스터 처치 수 표시
//        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
//        if (spawnManager != null)
//        {
//            GUILayout.Label("몬스터 처치 수: " + spawnManager.enemyCount);
//        }
        
//        GUILayout.EndArea();
//    }
//} 