using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] float spawnMinX; // 몬스터 생성 최소 x좌표
	[SerializeField] float spawnMaxX; // 몬스터 생성 최대 x좌표
	[SerializeField] GameObject[] enemies; // 몬스터 객체 배열 (1단계 : 잡몹 / 2단계 : ~~ 등등)

	List<GameObject> monsters = new List<GameObject>();


	float minDelay = 0.4f; // 몬스터 생성 최소 딜레이
	float maxDelay = 1.0f; // 몬스터 생성 최대 딜레이

	public int enemyIndex = 0; // 몬스터 인덱스
	public int enemyCount = 0; // 몬스터 카운트(몇 점 달성 시 다음 몬스터)

	float delay; // 몬스터 생성 딜레이

	Coroutine cr; // 몬스터 생성 코루틴

	IEnumerator Start()
	{
		EventManager.instance.playerEvents.onMonsterDead += MonsterDead;
		EventManager.instance.stageEvents.onSpawnPause += SpawnPause;
		EventManager.instance.stageEvents.onChangeStage += ChangeStage;
		ItemSpawner.onGunItemAllDestroy += GunAllDestroy;
		ItemSpawner.onPeopleItemAllDestroy += PeopleAllDestroy;
		UIManager.instance.OpenUI(UIType.StartStage);
		yield return new WaitForSeconds(1f);
		UIManager.instance.CloseUI(UIType.StartStage);
		StageManager.instance.MoveStage(1);
	}



	void OnDestroy()
	{
		EventManager.instance.playerEvents.onMonsterDead -= MonsterDead;
		EventManager.instance.stageEvents.onSpawnPause -= SpawnPause;
		EventManager.instance.stageEvents.onChangeStage -= ChangeStage;
		ItemSpawner.onGunItemAllDestroy -= GunAllDestroy;
		ItemSpawner.onPeopleItemAllDestroy -= PeopleAllDestroy;
	}


	void ChangeStage(int stage)
	{
		waitForUI = false;
		gunItemDestroyed = false;
		peopleItemDestroyed = false;
		cr = StartCoroutine(CreateMonster());
	}

	void SpawnPause()
	{
		if (cr != null) StopCoroutine(cr);
		StartCoroutine(WaitForAllDestroy());
	}

	bool waitForUI = false;
	void MonsterDead()
	{
		if (waitForUI) return;

		enemyCount++;

		int needCount = StageManager.instance.GetNeedKillCount();
		switch (StageManager.instance.curStage)
		{
			case 1:
				if (enemyCount == needCount)
				{
					waitForUI = true;
					enemyCount = 0;
					enemyIndex = 1;
					EventManager.instance.stageEvents.SpawnPause();
				}
				break;
			case 2:
				if (enemyCount == needCount)
				{
					waitForUI = true;
					enemyCount = 0;
					enemyIndex = 2;
					EventManager.instance.stageEvents.SpawnPause();
				}
				break;
			case 3:
				if (enemyCount == needCount)
				{
					waitForUI = true;
					enemyCount = 0;
					enemyIndex = 3;
					EventManager.instance.stageEvents.SpawnPause();
				}
				break;
		}
	}

	bool gunItemDestroyed = false;
	private void GunAllDestroy()
	{
		gunItemDestroyed = true;
	}
	bool peopleItemDestroyed = false;
	private void PeopleAllDestroy()
	{
		peopleItemDestroyed = true;
	}

	IEnumerator WaitForAllDestroy()
	{
		while (true)
		{
			yield return null;
			bool isAllNull = true;
			for (int i = 0; i < monsters.Count; i++)
			{
				if (monsters[i] != null)
				{
					isAllNull = false;
					break;
				}
			}
			if (isAllNull) break;
		}

		monsters.Clear();

		while (true)
		{
			yield return null;
			if (gunItemDestroyed) break;
		}

		while (true)
		{
			yield return null;
			if (peopleItemDestroyed) break;
		}


		UIManager.instance.OpenUI(UIType.ClearStage);
	}
	IEnumerator CreateMonster() // 몬스터 생성 코루틴
	{
		while (enemyIndex < 3)
		{
			delay = Mathf.Max(minDelay, maxDelay * 0.8f);
			SpawnMonster();
			yield return new WaitForSeconds(delay);
		}

		//보스생성.
		UIManager.instance.OpenUI(UIType.BossWarnning);
		yield return new WaitForSeconds(3.5f);
		UIManager.instance.CloseUI(UIType.BossWarnning);
		Instantiate(enemies[enemyIndex], new Vector2(0, transform.position.y - 3), Quaternion.identity);
	}
	void SpawnMonster()
	{
		float randomX = UnityEngine.Random.Range(spawnMinX, spawnMaxX);
		GameObject go = Instantiate(enemies[enemyIndex], new Vector2(randomX, transform.position.y), Quaternion.identity);
		float monsterSpeed = StageManager.instance.curStage switch
		{
			1 => Constants.STAGE1_MONSTER_SPEED,
			2 => Constants.STAGE2_MONSTER_SPEED,
			3 => Constants.STAGE3_MONSTER_SPEED,
		};
		go.GetComponent<Monster_1>().Init(monsterSpeed);
		monsters.Add(go);
		Destroy(go, 5f);
	}


}
