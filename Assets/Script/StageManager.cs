using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("씬에 1개만 배치해주세요.");
        }
        instance = this;
    }

    public int curStage { get; private set; } = 0;

    public void MoveStage(int targetStage)
    {
        if (curStage == targetStage) Debug.LogError($"curStage:{curStage}와 targetStage:{targetStage}은 달라야합니다.");
        curStage = targetStage;
        EventManager.instance.stageEvents.ChangeStage(curStage);
    }

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
