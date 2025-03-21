using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager _instance; // ���� �Ŵ��� ��ü

	[SerializeField] float spawnMinX; // ���� ���� �ּ� x��ǥ
	[SerializeField] float spawnMaxX; // ���� ���� �ִ� x��ǥ
	[SerializeField] GameObject[] enemies; // ���� ��ü �迭 (1�ܰ� : ��� / 2�ܰ� : ~~ ���)
	[SerializeField] GameObject bossWarning; // ���� ���� �� ��� �̹���

	float minDelay = 0.5f; // ���� ���� �ּ� ������
	float maxDelay = 3f; // ���� ���� �ִ� ������

	public int enemyIndex = 0; // ���� �ε���
	public int enemyCount = 0; // ���� ī��Ʈ(�� �� �޼� �� ���� ����)

	float delay; // ���� ���� ������

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
		StartCoroutine( CreateMonster() );
	}

	void Update()
	{
		if (enemyIndex == 3)
		{
			StopCoroutine( CreateMonster() );
		}
	}

	IEnumerator CreateMonster() // ���� ���� �ڷ�ƾ
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
