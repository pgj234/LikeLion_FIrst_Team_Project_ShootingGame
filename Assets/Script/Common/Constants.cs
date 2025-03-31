/// <summary>
/// 게임 내 상수 값들을 정의하는 클래스
/// 스테이지별 설정 값과 게임 내 다양한 상수들을 관리합니다.
/// </summary>
public static class Constants
{
	//===== 스테이지 1 설정값 =====
	/// <summary>스테이지 1 맵 스크롤 속도</summary>
	public static float STAGE1_MAP_SPEED = 1f;
	/// <summary>스테이지 1 몬스터 이동 속도 (맵 속도의 2배)</summary>
	public static float STAGE1_MONSTER_SPEED = STAGE1_MAP_SPEED * 2f;

	/// <summary>스테이지 1 울타리 체력</summary>
	public static int STAGE1_FENCE_HP = 3;
	/// <summary>스테이지 1 울타리 최대 등장 횟수</summary>
	public static int STAGE1_FENCE_COUNT = 3; //최대등장횟수

	/// <summary>스테이지 1 클리어에 필요한 몬스터 처치 수</summary>
	public static int STAGE1_NEED_KILL_COUNT = 10;

	public static int STAGE1_PEOPLE_FENCE_INIT_VALUE = -1;//인구증가 초기값
	public static int STAGE1_PEOPLE_FENCE_COUNT = 3; //최대등장횟수


	//===== 스테이지 2 설정값 =====
	/// <summary>스테이지 2 맵 스크롤 속도</summary>
	public static float STAGE2_MAP_SPEED = 2f;
	/// <summary>스테이지 2 몬스터 이동 속도 (맵 속도의 2배)</summary>
	public static float STAGE2_MONSTER_SPEED = STAGE2_MAP_SPEED * 2f;

	/// <summary>스테이지 2 울타리 체력</summary>
	public static int STAGE2_FENCE_HP = 4;
	/// <summary>스테이지 2 울타리 최대 등장 횟수</summary>
	public static int STAGE2_FENCE_COUNT = 3; //최대등장횟수

	/// <summary>스테이지 2 클리어에 필요한 몬스터 처치 수</summary>
	public static int STAGE2_NEED_KILL_COUNT = 10;

	public static int STAGE2_PEOPLE_FENCE_INIT_VALUE = -5; //인구증가 초기값
	public static int STAGE2_PEOPLE_FENCE_COUNT = 3; //최대등장횟수



	//===== 스테이지 3 설정값 =====
	/// <summary>스테이지 3 맵 스크롤 속도</summary>
	public static float STAGE3_MAP_SPEED = 3f;
	/// <summary>스테이지 3 몬스터 이동 속도 (맵 속도의 2배)</summary>
	public static float STAGE3_MONSTER_SPEED = STAGE3_MAP_SPEED * 2f;

	/// <summary>스테이지 3 울타리 체력</summary>
	public static int STAGE3_FENCE_HP = 5;
	/// <summary>스테이지 3 울타리 최대 등장 횟수</summary>
	public static int STAGE3_FENCE_COUNT = 3; //최대등장횟수

	/// <summary>스테이지 3 클리어에 필요한 몬스터 처치 수</summary>
	public static int STAGE3_NEED_KILL_COUNT = 10;

	public static int STAGE3_PEOPLE_FENCE_INIT_VALUE = -15; //인구증가 초기값
	public static int STAGE3_PEOPLE_FENCE_COUNT = 3; //최대등장횟수

	//===== 스테이지 4 (보스 스테이지) 설정값 =====
	/// <summary>스테이지 4 맵 스크롤 속도</summary>
	public static float STAGE4_MAP_SPEED = 4f;


	/// <summary>보스 스테이지 번호</summary>
	public static int BOSS_STAGE = 4;
}