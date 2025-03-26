using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 생성 및 관리를 담당하는 매니저 클래스
/// 스테이지별 몬스터 생성, 처치 카운트 관리, 보스 생성 등을 담당합니다.
/// </summary>
public class SpawnManager : MonoBehaviour
{
	// 싱글톤 인스턴스
	public static SpawnManager _instance; // 스폰 매니저 객체
	public static StageManager _stageManager; // 스테이지 매니저 객체

	// 몬스터 스폰 관련 설정
	[SerializeField] float spawnMinX; // 몬스터 생성 최소 x좌표
	[SerializeField] float spawnMaxX; // 몬스터 생성 최대 x좌표
	[SerializeField] GameObject[] enemies; // 몬스터 오브젝트 배열 (1단계: 닭 / 2단계: 여우 / 3단계: 늑대)
	[SerializeField] GameObject bossWarning; // 보스 등장 시 경고 이미지
	List<GameObject> enemyList = new List<GameObject>(); // 몬스터 리스트 (관리용)

	// 몬스터 생성 딜레이 관련 변수
	float minDelay = 0.2f; // 몬스터 생성 최소 딜레이
	float maxDelay = 1.5f; // 몬스터 생성 최대 딜레이
	float startDelay = 1.0f; // 몬스터 생성 시작 딜레이

	// 몬스터 관리 변수
	public int enemyIndex = 0; // 몬스터 인덱스 (현재 스테이지에 맞는 몬스터 유형)
	public int enemyCount = 0; // 몬스터 처치 카운트(각 단계별 필요한 처치 수)

	// 기타 몬스터 생성 관련 변수
	float delay; // 몬스터 생성 딜레이
	bool bossCreate = false; // 보스 생성 여부
	
	// 몬스터 생성 코루틴 관리
	Coroutine cr; // 몬스터 생성 코루틴

	/// <summary>
	/// 게임 시작 시 호출되는 메서드
	/// 싱글톤 패턴을 구현합니다.
	/// </summary>
	void Awake() // 싱글톤 패턴 구현
	{
		// 싱글톤 패턴 구현
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy(this.gameObject); // 중복 인스턴스 제거
		}
	}

	/// <summary>
	/// 컴포넌트 초기화 후 호출되는 메서드
	/// 이벤트 등록 및 몬스터 생성 코루틴을 시작합니다.
	/// </summary>
	void Start()
	{
		// 몬스터 처치 이벤트 등록
		EventManager.instance.playerEvents.onMonsterDead += MonsterDead;
		// 몬스터 생성 코루틴 시작
		cr = StartCoroutine(CreateMonster());
	}

	/// <summary>
	/// 오브젝트 파괴 시 호출되는 메서드
	/// 이벤트 리스너를 해제합니다.
	/// </summary>
	void OnDestroy()
	{
		// 몬스터 처치 이벤트 해제
		EventManager.instance.playerEvents.onMonsterDead -= MonsterDead;
	}

	/// <summary>
	/// 몬스터 처치 시 호출되는 메서드
	/// 처치 카운트를 증가시키고 스테이지 전환 조건을 확인합니다.
	/// </summary>
	void MonsterDead()
	{
		// 몬스터 처치 카운트 증가
		enemyCount++;
		Debug.Log("몬스터 처치! 현재 처치 수: " + enemyCount);
		
		// 현재 스테이지에 따른 처리
		switch (StageManager.instance.curStage)
		{
			// 스테이지 1
			case 1:
				// 필요한 처치 수 달성 시 스테이지 2로 전환
				if (enemyCount >= Constants.STAGE1_NEED_KILL_COUNT)
				{
					Debug.Log("스테이지 1 클리어! 스테이지 2로 전환");
					enemyCount = 0; // 처치 카운트 초기화
					enemyIndex = 1; // 새로운 몬스터 유형으로 변경
					// 스테이지 전환 시에만 모든 적 제거
					ClearAllEnemies();
					// 스테이지 2로 전환
					StageManager.instance.MoveStage(2);
				}
				break;
			// 스테이지 2
			case 2:
				// 필요한 처치 수 달성 시 스테이지 3으로 전환
				if (enemyCount >= Constants.STAGE2_NEED_KILL_COUNT)
				{
					Debug.Log("스테이지 2 클리어! 스테이지 3으로 전환");
					enemyCount = 0; // 처치 카운트 초기화
					enemyIndex = 2; // 새로운 몬스터 유형으로 변경
					// 스테이지 전환 시에만 모든 적 제거
					ClearAllEnemies();
					// 스테이지 3으로 전환
					StageManager.instance.MoveStage(3);
				}
				break;
			// 스테이지 3
			case 3:
				// 필요한 처치 수 달성 시 스테이지 4(보스)로 전환
				if (enemyCount >= Constants.STAGE3_NEED_KILL_COUNT)
				{
					Debug.Log("스테이지 3 클리어! 스테이지 4(보스)로 전환");
					enemyCount = 0; // 처치 카운트 초기화
					enemyIndex = 3; // 보스 몬스터 유형으로 변경
					// 스테이지 전환 시에만 모든 적 제거
					ClearAllEnemies();
					// 스테이지 4(보스)로 전환
					StageManager.instance.MoveStage(4);
				}
				break;
		}
	}

	/// <summary>
	/// 모든 적을 제거하는 메서드
	/// 스테이지 전환 시 화면에 있는 모든 적을 제거합니다.
	/// </summary>
	private void ClearAllEnemies()
	{
		Debug.Log("스테이지 전환으로 모든 적 제거 - 적 수: " + enemyList.Count);
		// 모든 적 제거
		enemyList.ForEach(x => { if (x != null) Destroy(x); });
		// 리스트 초기화
		enemyList.Clear();
	}

	/// <summary>
	/// 몬스터를 생성하는 코루틴
	/// 스테이지별로 다른 몬스터를 생성합니다.
	/// </summary>
	IEnumerator CreateMonster() // 몬스터 생성 코루틴
	{
		// 시작 딜레이 대기
		yield return new WaitForSeconds(startDelay);
		// 스테이지 1부터 시작
		StageManager.instance.MoveStage(1);
		
		// 무한 루프로 계속 몬스터 생성
		while (true)
		{
			// 몬스터 생성 딜레이 계산
			delay = Mathf.Max(minDelay, maxDelay * 0.8f);
			// 랜덤 X 좌표 생성
			float randomX = Random.Range(spawnMinX, spawnMaxX);

			// 현재 스테이지에 맞는 몬스터 생성
			if (enemyIndex == 0) // 스테이지 1 몬스터
			{
				// 몬스터 생성 및 리스트에 추가
				GameObject go = Instantiate(enemies[enemyIndex], new Vector2(randomX, transform.position.y), Quaternion.identity);
				enemyList.Add(go);
				// 5초 후 자동 파괴 (화면 밖으로 나갔을 경우 대비)
				Destroy(go, 5f);
			}
			else if (enemyIndex == 1) // 스테이지 2 몬스터
			{
				// 몬스터 생성 및 리스트에 추가
				GameObject go1 = Instantiate(enemies[enemyIndex], new Vector2(randomX, transform.position.y), Quaternion.identity);
				enemyList.Add(go1);
				// 5초 후 자동 파괴
				Destroy(go1, 5f);
			}
			else if (enemyIndex == 2) // 스테이지 3 몬스터
			{
				// 몬스터 생성 및 리스트에 추가
				GameObject go2 = Instantiate(enemies[enemyIndex], new Vector2(randomX, transform.position.y), Quaternion.identity);
				enemyList.Add(go2);
				// 5초 후 자동 파괴
				Destroy(go2, 5f);
			}
			else if (enemyIndex == 3) // 보스 스테이지
			{
				// 일반 몬스터 생성 코루틴 중지
				StopCoroutine(cr);
				// 보스 생성 코루틴 시작
				StartCoroutine(CreateBoss());
			}
			// 다음 몬스터 생성까지 딜레이
			yield return new WaitForSeconds(delay);
		}
	}

	/// <summary>
	/// 보스를 생성하는 코루틴
	/// 보스 경고 이미지를 표시하고 보스를 생성합니다.
	/// </summary>
	IEnumerator CreateBoss()
	{
		// 보스 경고 이미지 활성화
		bossWarning.SetActive(true);
		// 3.5초 대기
		yield return new WaitForSeconds(3.5f);
		// 보스 경고 이미지 비활성화
		bossWarning.SetActive(false);
		// 보스 몬스터 생성 (중앙 위치)
		Instantiate(enemies[enemyIndex], new Vector2(0, transform.position.y - 3), Quaternion.identity);
		// 보스 생성 플래그 설정
		bossCreate = true;
	}
}
