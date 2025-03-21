using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager _instance; // 스폰 매니저 객체

	[SerializeField] float spawnMinX; // 몬스터 생성 최소 x좌표
	[SerializeField] float spawnMaxX; // 몬스터 생성 최대 x좌표
	[SerializeField] GameObject[] enemies; // 몬스터 객체 배열 (1단계 : 잡몹 / 2단계 : ~~ 등등)
	[SerializeField] GameObject bossWarning; // 보스 등장 전 경고 이미지

	float minDelay = 0.5f; // 몬스터 생성 최소 딜레이
	float maxDelay = 3f; // 몬스터 생성 최대 딜레이

	public int enemyIndex = 0; // 몬스터 인덱스
	public int enemyCount = 0; // 몬스터 카운트(몇 점 달성 시 다음 몬스터)

	float delay; // 몬스터 생성 딜레이

	void Awake() // 간단한 싱글톤 패턴
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy( this.gameObject );
		}
	}
	void Start()
	{
		StartCoroutine( CreateMonster() );
	}

	void Update()
	{
		if (enemyIndex == 3)
		{
			StopCoroutine( CreateMonster() );
		}
	}

	IEnumerator CreateMonster() // 몬스터 생성 코루틴
	{
		while (true)
		{
			delay = Mathf.Max( minDelay, maxDelay * 0.97f );
			float randomX = Random.Range( spawnMinX, spawnMaxX );
			yield return new WaitForSeconds( delay );
			GameObject mon = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
			Destroy( mon, 4f );
			if (enemyCount >= 10 && enemyCount < 20)
			{
				enemyIndex = 1;
			}
			else if (enemyCount >= 20 && enemyCount < 30)
			{
				enemyIndex = 2;
			}
			else if (enemyCount >= 30)
			{
				enemyIndex = 3;
				bossWarning.SetActive( true );
				yield return new WaitForSeconds( 3f );
				bossWarning.SetActive( false );
			}
		}


	}

}
