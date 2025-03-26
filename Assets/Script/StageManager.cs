using System.Collections;
using UnityEngine;

/// <summary>
/// 게임의 스테이지 관리를 담당하는 매니저 클래스
/// 스테이지 이동, 속도 설정, 각종 스테이지별 설정값을 제공합니다.
/// 싱글톤 패턴으로 구현되어 게임 내에서 하나의 인스턴스만 존재합니다.
/// </summary>
public class StageManager : MonoBehaviour
{
    /// <summary>
    /// 싱글톤 인스턴스
    /// </summary>
    public static StageManager instance { get; private set; }

    /// <summary>
    /// 게임 시작 시 호출되는 메서드
    /// 싱글톤 패턴을 구현합니다.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("씬에 1개만 배치해주세요.");
        }
        instance = this;
    }

    /// <summary>
    /// 현재 스테이지 번호
    /// </summary>
    public int curStage { get; private set; } = 0;

    /// <summary>
    /// 스테이지를 이동하는 메서드
    /// </summary>
    /// <param name="targetStage">이동할 스테이지 번호</param>
    public void MoveStage(int targetStage)
    {
        if (curStage == targetStage) Debug.LogError($"curStage:{curStage}와 targetStage:{targetStage}은 달라야합니다.");
        curStage = targetStage;
        EventManager.instance.stageEvents.ChangeStage(curStage);
    }

    /// <summary>
    /// 현재 스테이지의 맵 스크롤 속도를 반환하는 메서드
    /// </summary>
    /// <returns>현재 스테이지의 맵 스크롤 속도</returns>
    public float GetCurSpeed()
    {
        float speed = curStage switch
        {
            1 => Constants.STAGE1_MAP_SPEED,
            2 => Constants.STAGE2_MAP_SPEED,
            3 => Constants.STAGE3_MAP_SPEED,
            4 => Constants.STAGE4_MAP_SPEED,
            _ => -1,
        };

        if (speed < 0) Debug.LogError($"curStage:{curStage},에 대한 MAP_SPEED가 정의되어야합니다.");
        return speed;
    }

    /// <summary>
    /// 현재 스테이지의 울타리 체력을 반환하는 메서드
    /// </summary>
    /// <returns>현재 스테이지의 울타리 체력</returns>
    public int GetFenceHP()
    {
        int hp = curStage switch
        {
            1 => Constants.STAGE1_FENCE_HP,
            2 => Constants.STAGE2_FENCE_HP,
            3 => Constants.STAGE3_FENCE_HP,
            _ => -1,
        };

        if (hp < 0) Debug.LogError($"curStage:{curStage},에 대한 FENCE_HP가 정의되어야합니다.");
        return hp;
    }
    
    /// <summary>
    /// 현재 스테이지의 울타리 개수를 반환하는 메서드
    /// </summary>
    /// <returns>현재 스테이지의 울타리 개수</returns>
    public int GetFenceCount()
    {
        int spawnCount = curStage switch
        {
            1 => Constants.STAGE1_FENCE_COUNT,
            2 => Constants.STAGE2_FENCE_COUNT,
            3 => Constants.STAGE3_FENCE_COUNT,
            _ => -1,
        };
        if (spawnCount < 0) Debug.LogError($"curStage:{curStage},에 대한 FENCE_COUNT가 정의되어야합니다.");
        return spawnCount;
    }
    
    /// <summary>
    /// 현재 스테이지에서 클리어하기 위해 필요한 몬스터 처치 수를 반환하는 메서드
    /// </summary>
    /// <returns>현재 스테이지의 필요 몬스터 처치 수</returns>
    public int GetNeedKillCount()
    {
        int spawnCount = curStage switch
        {
            1 => Constants.STAGE1_NEED_KILL_COUNT,
            2 => Constants.STAGE2_NEED_KILL_COUNT,
            3 => Constants.STAGE3_NEED_KILL_COUNT,
            _ => -1,
        };
        if (spawnCount < 0) Debug.LogError($"curStage:{curStage},에 대한 FENCE_COUNT가 정의되어야합니다.");
        return spawnCount;
    }
}
