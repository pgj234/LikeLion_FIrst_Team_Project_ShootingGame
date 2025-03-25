using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager _instance; // 스폰 매니저 객체

	[SerializeField] float spawnMinX; // 몬스터 생성 최소 x좌표
	[SerializeField] float spawnMaxX; // 몬스터 생성 최대 x좌표
	[SerializeField] GameObject[] enemies; // 몬스터 객체 배열 (1단계 : 잡몹 / 2단계 : ~~ 등등)
	[SerializeField] GameObject bossWarning; // 보스 등장 전 경고 이미지

	float minDelay = 0.2f; // 몬스터 생성 최소 딜레이
	float maxDelay = 1.5f; // 몬스터 생성 최대 딜레이

	public int enemyIndex = 0; // 몬스터 인덱스
	public int enemyCount = 0; // 몬스터 카운트(몇 점 달성 시 다음 몬스터)

	float delay; // 몬스터 생성 딜레이
	bool bossCreate = false; // 보스 생성 여부

	Coroutine cr; // 몬스터 생성 코루틴


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
		cr = StartCoroutine( CreateMonster() );


	}

	void Update()
	{

	}


	IEnumerator CreateMonster() // 몬스터 생성 코루틴
	{
		while (true)
		{
			delay = Mathf.Max( minDelay, maxDelay * 0.97f );
			float randomX = Random.Range( spawnMinX, spawnMaxX );

			if (enemyCount < 3)
			{
				enemyIndex = 0;
				GameObject go = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				Destroy( go, 5f );
			}

			else if (enemyCount >= 3 && enemyCount < 6)
			{
				enemyIndex = 1;
				GameObject go1 = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				Destroy( go1, 5f );
			}
			else if (enemyCount >= 6 && enemyCount < 9)
			{
				enemyIndex = 2;
				GameObject go2 = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				Destroy( go2, 5f );
			}
			else if (enemyCount >= 9 && !bossCreate)
			{
				enemyIndex = 3;
				StopCoroutine( cr );
				StartCoroutine( CreateBoss() );
				StopCoroutine( CreateBoss() );
			}
			yield return new WaitForSeconds( delay );

		}
	}

	IEnumerator CreateBoss()
	{
		bossWarning.SetActive( true );
		yield return new WaitForSeconds( 3.5f );
		bossWarning.SetActive( false );
		Instantiate( enemies[enemyIndex], new Vector2( 0, transform.position.y - 3 ), Quaternion.identity );
		bossCreate = true;
	}

}
