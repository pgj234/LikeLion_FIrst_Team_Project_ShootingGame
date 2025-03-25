using UnityEngine;

public class BossBullet : MonoBehaviour
{
	public GameObject target; //플레이어
	public float Speed = 4.5f;
	Vector2 dir;
	Vector2 dirNo;

	void Start()
	{
		//플레이어 태크로 찾기
		target = GameObject.FindGameObjectWithTag( "Player" );
		//A - B A를 바라보는 벡터
		dir = target.transform.position - transform.position;
		//방향 벡터만 구하기 단위벡터 정규화 노말 1의 크기로 만든다.
		dirNo = dir.normalized;



	}

	void Update()
	{
		transform.Translate( dirNo * Speed * Time.deltaTime );

		// transform.position=Vector3.MoveTowards(transform.position,
		// target.transform.position,Speed * Time.deltaTime);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Destroy( gameObject );
		}
	}

	private void OnBecameInvisible()
	{
		Destroy( gameObject );
	}
}
