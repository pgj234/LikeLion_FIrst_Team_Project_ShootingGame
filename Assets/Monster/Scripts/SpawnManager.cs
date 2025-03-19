using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager _instance; // ���� �Ŵ��� ��ü

	[SerializeField] float delay; // ���� ���� ������
	[SerializeField] float spawnMinX; // ���� ���� �ּ� x��ǥ
	[SerializeField] float spawnMaxX; // ���� ���� �ִ� x��ǥ
	[SerializeField] GameObject[] enemies; // ���� ��ü �迭 (1�ܰ� : ��� / 2�ܰ� : ~~ ���)

	public int enemyIndex = 0; // ���� �ε���
	public int enemyCount = 0; // ���� ī��Ʈ(�� �� �޼� �� ���� ����)

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
		yield return new WaitForSeconds( delay );

		float randomX = Random.Range( spawnMinX, spawnMaxX );
		Instantiate( enemies[enemyIndex], new Vector2( transform.position.y, randomX ), Quaternion.identity );

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
		}
	}
}
