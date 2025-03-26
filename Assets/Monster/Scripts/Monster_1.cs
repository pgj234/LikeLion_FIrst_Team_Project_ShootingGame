using UnityEngine;

public class Monster_1 : MonoBehaviour
{
	//[SerializeField] float speedMin;
	//[SerializeField] float speedMax;
	//[SerializeField] float HP;
	[SerializeField] float currentSpeed;

	void Start()
	{
		//currentSpeed = Random.Range( speedMin, speedMax );
	}

	void Update()
	{
		MonsterMove();
	}

	void MonsterMove()
	{
		transform.Translate( Vector2.down * currentSpeed * Time.deltaTime );
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag( "PBullet" ))
		{

			EventManager.instance.playerEvents.MonsterDead();
			Destroy( this.gameObject );
		}
	}
}
