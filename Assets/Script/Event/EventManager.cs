using UnityEngine;

/// <summary>
/// 게임 내 이벤트 시스템을 관리하는 매니저 클래스
/// 플레이어 이벤트와 스테이지 이벤트를 중앙에서 관리합니다.
/// 싱글톤 패턴으로 구현되어 게임 내에서 하나의 인스턴스만 존재합니다.
/// </summary>
public class EventManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static EventManager instance { get; private set; }

    // 이벤트 객체들
    public PlayerEvents playerEvents;   // 플레이어 관련 이벤트
    public StageEvents stageEvents;     // 스테이지 관련 이벤트

    /// <summary>
    /// 게임 시작 시 호출되는 메서드
    /// 싱글톤 패턴을 구현하고 이벤트 객체를 초기화합니다.
    /// </summary>
    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (instance != null)
        {
            Debug.LogError("씬에 1개만 배치해주세요.");
        }
        instance = this;

        // 이벤트 객체 초기화
        playerEvents = new PlayerEvents();
        stageEvents = new StageEvents();
    }
}
