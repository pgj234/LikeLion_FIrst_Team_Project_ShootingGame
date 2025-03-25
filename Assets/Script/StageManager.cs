using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	public static StageManager instance { get; private set; }

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError( "씬에 1개만 배치해주세요." );
		}
		instance = this;
	}

	public int curStage = 0;
	public int curKillCount = 0;
	public int needKillCount = 0;

	IEnumerator Start()
	{
		EventManager.instance.playerEvents.onMonsterDead += MonsterDead;

		yield return new WaitForSeconds( 1f );
		MoveStage( 1 );
	}
	void OnDestroy()
	{
		EventManager.instance.playerEvents.onMonsterDead -= MonsterDead;
	}

	void MonsterDead()
	{
		curKillCount++;
		if (curKillCount >= needKillCount)
		{
			MoveStage( curStage + 1 );
		}
	}

	void MoveStage(int targetStage)
	{
		if (curStage == targetStage)
		{
			// 동일한 스테이지일 경우 아무 작업도 하지 않음
			return;
		}
		curStage = targetStage;
		curKillCount = 0;
		needKillCount = curStage switch
		{
			1 => Constants.STAGE1_NEED_KILL_COUNT,
			2 => Constants.STAGE2_NEED_KILL_COUNT,
			3 => Constants.STAGE3_NEED_KILL_COUNT,
			4 => Constants.STAGE4_NEED_KILL_COUNT,


		};
		//Debug.Assert( needKillCount > 0 );
		EventManager.instance.stageEvents.ChangeStage( curStage );
	}

	public float GetCurSpeed()
	{
		//Debug.Assert( curStage > 0 );
		float speed = curStage switch
		{
			1 => Constants.STAGE1_MAP_SPEED,
			2 => Constants.STAGE2_MAP_SPEED,
			3 => Constants.STAGE3_MAP_SPEED,
			4 => Constants.STAGE4_MAP_SPEED,
		};
		//Debug.Assert( speed > 0 );
		return speed;
	}

	public int GetFenceHP()
	{
		//Debug.Assert( curStage > 0 );
		int hp = curStage switch
		{
			1 => Constants.STAGE1_FENCE_HP,
			2 => Constants.STAGE2_FENCE_HP,
			3 => Constants.STAGE3_FENCE_HP,
			_ => -1,
		};
		//Debug.Assert( hp > 0 );
		return hp;
	}
	public int GetFenceCount()
	{
		//Debug.Assert( curStage > 0 );
		int spawnCount = curStage switch
		{
			1 => Constants.STAGE1_FENCE_COUNT,
			2 => Constants.STAGE2_FENCE_COUNT,
			3 => Constants.STAGE3_FENCE_COUNT,
			_ => -1,
		};
		//Debug.Assert( spawnCount >= 0 );
		return spawnCount;
	}
}
