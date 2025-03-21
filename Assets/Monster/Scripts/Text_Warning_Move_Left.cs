using UnityEngine;

public class Text_Warning_Move_Left : MonoBehaviour
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
		transform.Translate( Vector2.left * currentSpeed );
	}
}
