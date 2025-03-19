using System.Collections;
using TMPro;
using UnityEngine;

public class Text_Warning : MonoBehaviour
{
	[SerializeField] float textLerpTime;

	TextMeshProUGUI text;


	void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}
	void Start()
	{
		StartCoroutine( TextColorChange() );
	}

	void Update()
	{

	}

	IEnumerator TextColorChange()
	{
		while (true)
		{
			yield return StartCoroutine( TextColorLerp( Color.red, Color.yellow ) );
			yield return StartCoroutine( TextColorLerp( Color.yellow, Color.red ) );
		}
	}

	IEnumerator TextColorLerp(Color start, Color end)
	{
		float currentTime = 0;
		float percent = 0;

		while (percent < 1)
		{
			currentTime += Time.deltaTime;
			percent = currentTime / textLerpTime;
			text.color = Color.Lerp( start, end, percent );
			yield return null;
		}
	}
}
