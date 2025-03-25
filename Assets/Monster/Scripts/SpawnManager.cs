using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager _instance; // ���� �Ŵ��� ��ü

	[SerializeField] float spawnMinX; // ���� ���� �ּ� x��ǥ
	[SerializeField] float spawnMaxX; // ���� ���� �ִ� x��ǥ
	[SerializeField] GameObject[] enemies; // ���� ��ü �迭 (1�ܰ� : ��� / 2�ܰ� : ~~ ���)
	[SerializeField] GameObject bossWarning; // ���� ���� �� ��� �̹���

	float minDelay = 0.2f; // ���� ���� �ּ� ������
	float maxDelay = 1.5f; // ���� ���� �ִ� ������

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
		cr = StartCoroutine( CreateMonster() );


	}

	void Update()
	{

	}


	IEnumerator CreateMonster() // ���� ���� �ڷ�ƾ
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
