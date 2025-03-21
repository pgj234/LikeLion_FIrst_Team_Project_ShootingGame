using UnityEngine;

public class Text_WarningMove : MonoBehaviour
{

	[SerializeField] float currentSpeed;


	void Start()
	{

	}

	void Update()
	{
		MonsterMove();
	}

	void MonsterMove()
	{
		transform.Translate( Vector2.right * currentSpeed );
	}
}
