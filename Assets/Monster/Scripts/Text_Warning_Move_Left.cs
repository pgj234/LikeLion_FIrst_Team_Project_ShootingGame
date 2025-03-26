using UnityEngine;

/// <summary>
/// 경고 텍스트를 왼쪽으로 이동시키는 스크립트
/// 보스 등장 시 화면에 표시되는 경고 텍스트의 움직임을 제어합니다.
/// </summary>
public class Text_Warning_Move_Left : MonoBehaviour
{
	/// <summary>
	/// 경고 텍스트의 이동 속도
	/// </summary>
	[SerializeField] float currentSpeed;

	/// <summary>
	/// 게임 시작 시 호출되는 메서드
	/// </summary>
	void Start()
	{
		// 초기화 코드가 필요하면 여기에 작성
	}

	/// <summary>
	/// 매 프레임마다 호출되는 업데이트 메서드
	/// </summary>
	void Update()
	{
		MonsterMove();  // 경고 텍스트 이동 처리
	}

	/// <summary>
	/// 경고 텍스트를 왼쪽으로 이동시키는 메서드
	/// </summary>
	void MonsterMove()
	{
		transform.Translate(Vector2.left * currentSpeed);  // 왼쪽으로 이동
	}
}
