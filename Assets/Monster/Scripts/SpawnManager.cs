using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager _instance; // ���� �Ŵ��� ��ü
	public static StageManager _stageManager; // �������� �Ŵ��� ��ü

	[SerializeField] float spawnMinX; // ���� ���� �ּ� x��ǥ
	[SerializeField] float spawnMaxX; // ���� ���� �ִ� x��ǥ
	[SerializeField] GameObject[] enemies; // ���� ��ü �迭 (1�ܰ� : ��� / 2�ܰ� : ~~ ���)
	[SerializeField] GameObject bossWarning; // ���� ���� �� ��� �̹���
	List<GameObject> enemyList = new List<GameObject>(); // ���� ����Ʈ


	float minDelay = 0.2f; // ���� ���� �ּ� ������
	float maxDelay = 1.5f; // ���� ���� �ִ� ������
	float startDelay = 1.0f; // ���� ���� ���� ������

	public int enemyIndex = 0; // ���� �ε���
	public int enemyCount = 0; // ���� ī��Ʈ(�� �� �޼� �� ���� ����)

	float delay; // ���� ���� ������
	bool bossCreate = false; // ���� ���� ����

	Coroutine cr; // ���� ���� �ڷ�ƾ


	void Awake() // ������ �̱��� ����
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
		EventManager.instance.stageEvents.onChangeStage += MonsterDead;
		cr = StartCoroutine( CreateMonster() );
	}

	void MonsterDead(int iss)
	{
		if (iss == 1)
		{
			enemyIndex = 0;
		}
		else if (iss == 2)
		{
			for (int i = 0; enemyList.Count > i; i++)
			{
				Destroy( enemyList[i] );
			}
			enemyList.Clear();
			enemyIndex = 1;
		}
		else if (iss == 3)
		{
			for (int i = 0; enemyList.Count > i; i++)
			{
				Destroy( enemyList[i] );
			}
			enemyList.Clear();
			enemyIndex = 2;
		}
		else if (iss == 4)
		{
			for (int i = 0; enemyList.Count > i; i++)
			{
				Destroy( enemyList[i] );
			}
			enemyList.Clear();
			enemyIndex = 3;
		}
	}

	IEnumerator CreateMonster() // ���� ���� �ڷ�ƾ
	{
		yield return new WaitForSeconds( startDelay );
		while (true)
		{

			delay = Mathf.Max( minDelay, maxDelay * 0.8f );
			float randomX = Random.Range( spawnMinX, spawnMaxX );

			if (enemyIndex == 0)
			{
				GameObject go = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				enemyList.Add( go );
				Destroy( go, 5f );
			}

			else if (enemyIndex == 1)
			{
				GameObject go1 = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				enemyList.Add( go1 );
				Destroy( go1, 5f );
			}
			else if (enemyIndex == 2)
			{
				GameObject go2 = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				enemyList.Add( go2 );
				Destroy( go2, 5f );
			}
			else if (enemyIndex == 3)
			{
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
