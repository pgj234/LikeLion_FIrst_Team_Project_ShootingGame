using UnityEngine;

/// <summary>
/// 몬스터 기본 동작을 관리하는 클래스
/// 몬스터의 이동, 충돌 처리, 파괴 등을 담당합니다.
/// </summary>
public class Monster_1 : MonoBehaviour
{
	//[SerializeField] float speedMin;       // 최소 속도 (사용하지 않음)
	//[SerializeField] float speedMax;       // 최대 속도 (사용하지 않음)
	//[SerializeField] float HP;             // 체력 (사용하지 않음)
	private float currentSpeed;     // 현재 이동 속도
	public void Init(float speed)
	{
        currentSpeed = speed;
    }

	/// <summary>
	/// 게임 시작 시 호출되는 메서드
	/// 몬스터의 초기 설정을 담당합니다.
	/// </summary>
	void Start()
	{
		//currentSpeed = Random.Range( speedMin, speedMax );   // 랜덤 속도 설정 (주석 처리됨)
	}

	/// <summary>
	/// 매 프레임마다 호출되는 업데이트 메서드
	/// 몬스터의 이동을 처리합니다.
	/// </summary>
	void Update()
	{
		MonsterMove();   // 몬스터 이동 처리
	}

	/// <summary>
	/// 몬스터 이동을 처리하는 메서드
	/// 몬스터를 아래쪽으로 일정 속도로 이동시킵니다.
	/// </summary>
	void MonsterMove()
	{
		// 몬스터를 아래쪽으로 이동 (Vector2.down은 (0, -1)을 의미)
		transform.Translate(Vector2.down * currentSpeed * Time.deltaTime);
	}

	/// <summary>
	/// 충돌 감지 시 호출되는 메서드
	/// 총알과 충돌 시 몬스터를 파괴하고 이벤트를 발생시킵니다.
	/// </summary>
	/// <param name="collision">충돌한 객체의 Collider2D</param>
	void OnTriggerEnter2D(Collider2D collision)
	{
		// 플레이어의 총알과 충돌 시
		if (collision.CompareTag("PBullet"))
		{
            SoundManager.instance.PlaySFX(Sound.EnemyDie);

            Debug.Log("몬스터 처치됨: " + gameObject.name);
			
			// 이벤트 발생 전에 총알 제거
			//Destroy(collision.gameObject);
			
			// 몬스터 처치 이벤트 발생
			EventManager.instance.playerEvents.MonsterDead();
			
			// 몬스터 제거
			Destroy(this.gameObject);
		}
	}
}
