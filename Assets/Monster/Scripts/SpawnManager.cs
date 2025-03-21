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

	IEnumerator CreateMonster() // ���� ���� �ڷ�ƾ
	{
		while (true)
		{
			delay = Mathf.Max( minDelay, maxDelay * 0.97f );
			float randomX = Random.Range( spawnMinX, spawnMaxX );
			yield return new WaitForSeconds( delay );
			if (3 > enemyIndex)
			{
				GameObject mon = Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				Destroy( mon, 4f );
			}

			if (enemyCount >= 1 && enemyCount < 2)
			{
				enemyIndex = 1;
			}
			else if (enemyCount >= 2 && enemyCount < 3)
			{
				enemyIndex = 2;
			}
			else if (enemyCount >= 3 && enemyIndex != 3)
			{
				enemyIndex = 3;
				bossWarning.SetActive( true );
				yield return new WaitForSeconds( 3.5f );
				bossWarning.SetActive( false );

				Instantiate( enemies[enemyIndex], new Vector2( randomX, transform.position.y ), Quaternion.identity );
				break;
			}
		}
	}

}
