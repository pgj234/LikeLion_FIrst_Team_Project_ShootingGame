using System;

/// <summary>
/// 스테이지 관련 이벤트를 관리하는 클래스
/// 스테이지 변경 시 발생하는 이벤트를 처리합니다.
/// </summary>
public class StageEvents
{
    /// <summary>
    /// 스테이지 변경 이벤트
    /// 매개변수로 변경된 스테이지 번호를 전달합니다.
    /// </summary>
    public event Action<int> onChangeStage;
    
    /// <summary>
    /// 스테이지 변경 이벤트를 발생시키는 메서드
    /// </summary>
    /// <param name="stage">변경할 스테이지 번호</param>
    public void ChangeStage(int stage)
    {
        onChangeStage?.Invoke(stage);  // 이벤트 리스너가 있으면 호출
    }
}
