using System;

/// <summary>
/// 플레이어 관련 이벤트를 관리하는 클래스
/// 무기 업그레이드, 플레이어 사망, 몬스터 처치 등의 이벤트를 제공합니다.
/// </summary>
public class PlayerEvents
{
    // 이벤트 선언
    public event Action onWeaponUpgrade;  // 무기 업그레이드 이벤트
    public event Action onPlayerDead;     // 플레이어 사망 이벤트
    public event Action onBossDead;    // 보스 처치 이벤트
    public event Action onMonsterDead;    // 몬스터 처치 이벤트
    
    /// <summary>
    /// 무기 업그레이드 이벤트를 발생시키는 메서드
    /// 플레이어의 무기가 업그레이드될 때 호출됩니다.
    /// </summary>
    public void WeaponUpgrade()
    {
        onWeaponUpgrade?.Invoke();  // 이벤트 리스너가 있으면 호출
    }
    
    /// <summary>
    /// 플레이어 사망 이벤트를 발생시키는 메서드
    /// 플레이어가 사망할 때 호출됩니다.
    /// </summary>
    public void PlayerDead()
    {
        onPlayerDead?.Invoke();  // 이벤트 리스너가 있으면 호출
    }

    /// <summary>
    /// 보스 처치 이벤트를 발생시키는 메서드
    /// 보스가 처치될 때 호출됩니다.
    /// </summary>
    public void BossDead()
    {
        onBossDead?.Invoke();  // 이벤트 리스너가 있으면 호출
    }

    /// <summary>
    /// 몬스터 처치 이벤트를 발생시키는 메서드
    /// 몬스터가 처치될 때 호출됩니다.
    /// </summary>
    public void MonsterDead()
    {
        onMonsterDead?.Invoke();  // 이벤트 리스너가 있으면 호출
    }
}
