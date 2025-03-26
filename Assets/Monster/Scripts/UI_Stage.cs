using UnityEngine;

/// <summary>
/// 스테이지 UI를 관리하는 클래스
/// 스테이지 관련 UI 요소의 동작을 제어합니다.
/// </summary>
public class UI_Stage : MonoBehaviour
{
	/// <summary>
	/// UI 요소 클릭 시 호출되는 메서드
	/// 클릭 시 해당 UI 요소를 비활성화합니다.
	/// </summary>
	public void OnClick()
	{
		gameObject.SetActive(false);  // UI 요소 비활성화
	}
}
