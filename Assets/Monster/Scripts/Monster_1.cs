using UnityEngine;

public class Monster_1 : MonoBehaviour
{
	[SerializeField] float speedMin;
	[SerializeField] float speedMax;
	[SerializeField] float HP;

	[SerializeField] GameObject bullet;

	void Start()
	{
		float speed = Random.Range( speedMin, speedMax );
	}

	void Update()
	{
		MonsterMove();
	}

	void MonsterMove()
	{
		transform.Translate( Vector2.down * speedMax * Time.deltaTime );
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag( "Bullet" ))
		{
			HP -= 1;
			if (HP <= 0)
			{
				SpawnManager._instance.enemyCount += 1;
				Destroy( gameObject );
			}
		}
	}
}
